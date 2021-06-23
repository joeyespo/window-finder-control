// Win32.cs
// By Joe Esposito
// [2021-06-21] Updated by Jimm Chen, now works with Win1.1607 mixed-mode DPI scaling.

using System;
using System.Drawing;
using System.Runtime.InteropServices;


namespace WindowFinder
{
    /// <summary>
    /// Win32 API.
    /// </summary>
    internal static class Win32
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct POINTAPI
        {
            public int x;
            public int y;
        }

        // Type definitions for Windows' basic types.
        public const int ANYSIZE_ARRAY = unchecked((int) (1));

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// Gets the text of the specified window.
        /// </summary>
        internal static string GetWindowText(IntPtr hWnd)
        {
            int cch;
            IntPtr lpString;
            string sResult;

            // !!!!! System.Text.Encoding
            if (IsWindow(hWnd) == 0)
                return "";

            if (IsWindowUnicode(hWnd) != 0)
            {
                // Allocate new Unicode string
                lpString = Marshal.AllocHGlobal((cch = (GetWindowTextLengthW(hWnd) + 1)) * 2);

                // Get window Unicode text
                GetWindowTextW(hWnd, lpString, cch);

                // Get managed string from Unicode string
                sResult = Marshal.PtrToStringUni(lpString, cch);

                // Free allocated Unicode string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            }
            else
            {
                // Allocate new ANSI string
                lpString = Marshal.AllocHGlobal((cch = (GetWindowTextLengthA(hWnd) + 1)));

                // Get window ANSI text
                GetWindowTextA(hWnd, lpString, cch);

                // Get managed string from ANSI string
                sResult = Marshal.PtrToStringAnsi(lpString, cch);

                // Free allocated ANSI string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            }
        }

        /// <summary>
        /// Gets the class name of the specified window.
        /// </summary>
        internal static string GetClassName(IntPtr hWnd)
        {
            const int windowClassNameLength = 255;
            int cch;
            IntPtr lpString;
            string sResult;

            // !!!!! System.Text.Encoding
            if (IsWindow(hWnd) == 0)
                return "";

            if (IsWindowUnicode(hWnd) != 0)
            {
                // Allocate new Unicode string
                lpString = Marshal.AllocHGlobal((cch = (windowClassNameLength + 1)) * 2);

                // Get window class Unicode text
                GetClassNameW(hWnd, lpString, cch);

                // Get managed string from Unicode string
                sResult = Marshal.PtrToStringUni(lpString, cch);

                // Free allocated Unicode string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            }
            else
            {
                // Allocate new ANSI string
                lpString = Marshal.AllocHGlobal((cch = (GetWindowTextLengthA(hWnd) + 1)));

                // Get window class ANSI text
                GetClassNameA(hWnd, lpString, cch);

                // Get managed string from ANSI string
                sResult = Marshal.PtrToStringAnsi(lpString, cch);

                // Free allocated ANSI string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            }
        }

        /// <summary>
        /// Retrieves the window from the client point.
        /// </summary>
        internal static IntPtr WindowFromPoint(IntPtr hClientWnd, int xPoint, int yPoint)
        {
            Point pt = new Point(xPoint, yPoint);

            ClientToScreen(hClientWnd, ref pt);

            return WindowFromPoint(pt);
        }

        /// <summary>
        /// Highlights the specified window.
        /// </summary>
        internal static bool HighlightWindow_InvertColor(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return true;

            IntPtr hDC; // The DC of the window.
            RECT rt = new RECT(); // Rectangle area of the window.

            float scale_factor = 1;

            // Get the window DC of the window.
            if ((hDC = (IntPtr) GetWindowDC(hWnd)) == IntPtr.Zero)
                return false;

            IntPtr thread_oldctx = IntPtr.Zero; // only useful for Win10.1607

            if (IsAboveWin10_1607())
            {
                // For Win10.1607, We need to switch to correct perspectives during the course of
                // * calling GetWindowRect()
                // * calling FramRgn()

                int target_hwnd_dpi = 0; // to be filled
                IntPtr target_hwnd_ctx = DpiUtilities.GetWindowDpiAwarenessContext(hWnd);
                DpiUtilities.DPI_AWARENESS daw = DpiUtilities.GetAwarenessFromDpiAwarenessContext(target_hwnd_ctx);

                if (daw == DpiUtilities.DPI_AWARENESS.DPI_AWARENESS_PER_MONITOR_AWARE)
                {
                    // Target HWND is Per-monitor mode, so we switch our thread to Per-monitor mode as well,
                    // so that GetWindowRect() and FrameRgn() all operates in physical(=pixel) coordinate.

                    thread_oldctx = DpiUtilities.SetThreadDpiAwarenessContext(DpiUtilities.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE);
                }
                else
                {
                    // Target HWND is NOT Per-monitor mode, so we switch our thread to DPI-unaware mode. So,
                    // GetWindowRect() gives up coordinates in 96DPI perspective.
                    // Then we scale the window width-and-height by a scaling factor.
                    // -- for example, target HWND is 144DPI, the scale_factor will be 1.5 .
                    // Finally, we call FrameRgn() with scaled-up width-and-height bcz the coordinates
                    // to FrameRgn() is in target-HWND's perspective, not in our thread's perspective.

                    thread_oldctx = DpiUtilities.SetThreadDpiAwarenessContext(DpiUtilities.DPI_AWARENESS_CONTEXT_UNAWARE);
                    target_hwnd_dpi = DpiUtilities.GetDpiForWindow(hWnd);

                    scale_factor = (float)target_hwnd_dpi / 96;
                }
            }

            // Get the screen coordinates of the rectangle of the window.
            GetWindowRect(hWnd, out rt);

            rt.right -= rt.left;
            rt.left = 0;
            rt.bottom -= rt.top;
            rt.top = 0;

            rt.right = (int)(rt.right * scale_factor);
            rt.bottom = (int)(rt.bottom * scale_factor);

            // Draw a border in the DC covering the entire window area of the window.
            IntPtr hRgn = (IntPtr) CreateRectRgnIndirect(ref rt);

            SetROP2(hDC, R2_NOT);
            FrameRgn(hDC, hRgn, (IntPtr) GetStockObject(WHITE_BRUSH), 3, 3);
            DeleteObject(hRgn);

            // Finally release the DC.
            ReleaseDC(hWnd, hDC);

            if (IsAboveWin10_1607())
            {
                // Restore our thread's original perspective.
                DpiUtilities.SetThreadDpiAwarenessContext(thread_oldctx);
            }

            return true;
        }

        /// <summary>
        /// Determines whether the two windows are related.
        /// </summary>
        internal static bool IsRelativeWindow(IntPtr hWnd, IntPtr hRelativeWindow, bool bProcessAncestor)
        {
            int dwProcess = new int(), dwProcessOwner = new int();
            int dwThread = new int(), dwThreadOwner = new int();
            ;

            // Failsafe
            if (hWnd == IntPtr.Zero)
                return false;
            if (hRelativeWindow == IntPtr.Zero)
                return false;
            if (hWnd == hRelativeWindow)
                return true;

            // Get processes and threads
            dwThread = GetWindowThreadProcessId(hWnd, ref dwProcess);
            dwThreadOwner = GetWindowThreadProcessId(hRelativeWindow, ref dwProcessOwner);

            // Get relative info
            if (bProcessAncestor)
                return (dwProcess == dwProcessOwner);
            return (dwThread == dwThreadOwner);
        }

        private static int s_isAboveWin10_1607 = -1;

        //
        public static bool IsAboveWin10_1607()
        {
            if (s_isAboveWin10_1607 == -1)
            {
                try
                {
                    int dpi = DpiUtilities.GetDpiForSystem();
                    s_isAboveWin10_1607 = 1;
                }
                catch
                {
                    s_isAboveWin10_1607 = 0;
                }
            }

            return s_isAboveWin10_1607 == 1 ? true : false;
        }


        [DllImport("user32", EntryPoint = "IsWindow", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int IsWindow(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "IsWindowUnicode", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int IsWindowUnicode(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "SetCapture", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int SetCapture(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        // keep?
        [DllImport("user32", EntryPoint = "MapWindowPoints", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref RECT lprt, int cPoints);

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref System.Drawing.Point pt,
            [MarshalAs(UnmanagedType.U4)] int cPoints);

        //[DllImport("user32", EntryPoint = "ChildWindowFromPoint", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        //public static extern int ChildWindowFromPoint(IntPtr hWnd, int xPoint, int yPoint);
        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, System.Drawing.Point pt);

        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPointEx(IntPtr hWndParent, Point pt,
            ChildWindowFromPointFlags uFlags);

        //
        [Flags]
        public enum ChildWindowFromPointFlags : uint
        {
            CWP_ALL = 0x0000,
            CWP_SKIPINVISIBLE = 0x0001,
            CWP_SKIPDISABLED = 0x0002,
            CWP_SKIPTRANSPARENT = 0x0004
        }

        [DllImport("user32", EntryPoint = "GetParent", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);
        //
        public enum GetAncestorFlags
        {
            /// <summary>
            /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
            /// </summary>
            GetParent = 1,
            /// <summary>
            /// Retrieves the root window by walking the chain of parent windows.
            /// </summary>
            GetRoot = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
            /// </summary>
            GetRootOwner = 3
        }

        [DllImport("user32", EntryPoint = "GetWindowTextLengthA", SetLastError = true, CharSet = CharSet.Ansi,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextLengthA(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextLengthW", SetLastError = true, CharSet = CharSet.Unicode,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextLengthW(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextA", SetLastError = true, CharSet = CharSet.Ansi,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextA(IntPtr hWnd, IntPtr lpString, int cch);

        [DllImport("user32", EntryPoint = "GetWindowTextW", SetLastError = true, CharSet = CharSet.Unicode,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextW(IntPtr hWnd, IntPtr lpString, int cch);

        [DllImport("user32", EntryPoint = "GetClassNameA", SetLastError = true, CharSet = CharSet.Ansi,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetClassNameA(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);

        [DllImport("user32", EntryPoint = "GetClassNameW", SetLastError = true, CharSet = CharSet.Unicode,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetClassNameW(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);

        [DllImport("user32", EntryPoint = "GetWindowDC", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("gdi32", EntryPoint = "CreateRectRgnIndirect", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int CreateRectRgnIndirect(ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(System.Drawing.Point p);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32", EntryPoint = "GetWindowRgn", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        //  Region Flags - The return value specifies the type of the region that the function obtains. It can be one of the following values.
        const int ERROR = 0;
        const int NULLREGION = 1;
        const int SIMPLEREGION = 2;
        const int COMPLEXREGION = 3;


        [DllImport("gdi32", EntryPoint = "SetROP2", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.Winapi)]
        public static extern int SetROP2(IntPtr hdc, int nDrawMode);

        [DllImport("gdi32", EntryPoint = "FrameRgn", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.Winapi)]
        public static extern int FrameRgn(IntPtr hdc, IntPtr hRgn, IntPtr hBrush, int nWidth, int nHeight);

        [DllImport("gdi32", EntryPoint = "GetStockObject", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetStockObject(int nIndex);

        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);

        [DllImport("gdi32", EntryPoint = "DeleteObject", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int DeleteObject(IntPtr hObject);

        [DllImport("user32", EntryPoint = "ReleaseDC", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        static extern bool Ellipse(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32.dll")]
        static extern int FrameRect(IntPtr hdc, [In] ref RECT lprc, IntPtr hbr);

        // Binary raster ops
        public const int R2_NOT = unchecked((int) (6)); //  Dn

        // Stock Logical Objects
        public const int WHITE_BRUSH = unchecked((int) (0));

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, DeviceCap nIndex);

        //
        public enum DeviceCap
        {
            /// <summary>
            /// Horizontal width in pixels
            /// </summary>
            HORZRES = 8,

            /// <summary>
            /// Vertical height in pixels
            /// </summary>
            VERTRES = 10,

            /// <summary>
            /// Vertical height of entire desktop in pixels
            /// </summary>
            DESKTOPVERTRES = 117,

            /// <summary>
            /// Horizontal width of entire desktop in pixels
            /// </summary>
            DESKTOPHORZRES = 118,
        }
    }

    public static class DpiUtilities
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetThreadDpiAwarenessContext(IntPtr DpiAwarenessContext); // Win10.1607
        // -- return old DPI_AWARENESS_CONTEXT enum for this thread

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDpiAwarenessContext(IntPtr hwnd);
        // -- return DPI_AWARENESS_CONTEXT enum for this hwnd

        // DPI_AWARENESS_CONTEXT enum values:
        public static IntPtr DPI_AWARENESS_CONTEXT_UNAWARE = (IntPtr)(-1);
        public static IntPtr DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = (IntPtr)(-2);
        public static IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = (IntPtr)(-3); // we only use this here
        public static IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = (IntPtr)(-4);

        [DllImport("user32.dll")]
        public static extern DPI_AWARENESS GetAwarenessFromDpiAwarenessContext(IntPtr dpi_awareness_context);
        //
        public enum DPI_AWARENESS : int
        {
            DPI_AWARENESS_INVALID = -1,
            DPI_AWARENESS_UNAWARE = 0,
            DPI_AWARENESS_SYSTEM_AWARE = 1,
            DPI_AWARENESS_PER_MONITOR_AWARE = 2
        }


        [DllImport("user32.dll")]
        public static extern int GetDpiForWindow(IntPtr hwnd); // Win10.1607

        [DllImport("user32.dll")]
        public static extern int GetDpiForSystem(); // Win10.1607

    }
}
