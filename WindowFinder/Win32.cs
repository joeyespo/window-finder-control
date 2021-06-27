// Win32.cs
// By Joe Esposito
// [2021-06-21] Updated by Jimm Chen, now works with Win1.1607 mixed-mode DPI scaling.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;


namespace WindowFinder
{
    /// <summary>
    /// Win32 API.
    /// </summary>
    public static class Win32
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

            public int Height
            {
                get { return bottom - top; }
                set { bottom = value + top; }
            }

            public int Width
            {
                get { return right - left; }
                set { right = value + left; }
            }
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

                    thread_oldctx =
                        DpiUtilities.SetThreadDpiAwarenessContext(DpiUtilities.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE);
                }
                else
                {
                    // Target HWND is NOT Per-monitor mode, so we switch our thread to DPI-unaware mode. So,
                    // GetWindowRect() gives up coordinates in 96DPI perspective.
                    // Then we scale the window width-and-height by a scaling factor.
                    // -- for example, target HWND is 144DPI, the scale_factor will be 1.5 .
                    // Finally, we call FrameRgn() with scaled-up width-and-height bcz the coordinates
                    // to FrameRgn() is in target-HWND's perspective, not in our thread's perspective.

                    thread_oldctx =
                        DpiUtilities.SetThreadDpiAwarenessContext(DpiUtilities.DPI_AWARENESS_CONTEXT_UNAWARE);
                    target_hwnd_dpi = DpiUtilities.GetDpiForWindow(hWnd);

                    scale_factor = (float) target_hwnd_dpi / 96;
                }
            }
            else if (DpiUtilities.IsWin81_or_above())
            {
                // This section is only suitable to Win81

                int selfdpi = DpiUtilities.Win81_GetWindowDpi(IntPtr.Zero);
                int targetdpi = DpiUtilities.Win81_GetWindowDpi(hWnd);

                scale_factor = (float)targetdpi / selfdpi; // may =1 or <1

                // [2021-06-26] Messy on Win81: Still buggy when caller is Per-mon-aware.
            }

            // Get the screen coordinates of the rectangle of the window.
            GetWindowRect(hWnd, out rt);
            
            rt.right -= rt.left;
            rt.left = 0;
            rt.bottom -= rt.top;
            rt.top = 0;

            rt.right = (int) (rt.right * scale_factor);
            rt.bottom = (int) (rt.bottom * scale_factor);

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
        /// Highlight target window(hWnd) by overlaying it with an same-size window(hwndOverlay).
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hwndOverlay"></param>
        internal static void HighlightWindow_Overlaying(IntPtr hWnd, IntPtr hwndOverlay)
        {
            /* This is unnecessary for Win81 and Win10.1607+
            IntPtr thread_oldctx = IntPtr.Zero;

            if (IsAboveWin10_1607())
            {
                thread_oldctx =
                    DpiUtilities.SetThreadDpiAwarenessContext(DpiUtilities.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE);
            }
            */

            RECT rt = new RECT();
            GetWindowRect(hWnd, out rt);

            IntPtr hwndToplevel = Win32.GetAncestor(hWnd, Win32.GetAncestorFlags.GetRoot);

            if (DpiUtilities.IsWin7() && (DpiUtilities.Win7_SystemDpi() > 96))
            {
                Win7_SpecialAdjust(hWnd, ref rt);
            }

            Win32.SetWindowPos(hwndOverlay, Win32.HWND_TOP,
                rt.left, rt.top, (rt.right - rt.left), (rt.bottom - rt.top)
            );

            //
            // Now we also want to place the aiming-window JUST ABOVE the target hWnd (z-order),
            // not blindly make it TOP at z-order.
            //

            IntPtr hwndHigher = Win32.GetWindow(hwndToplevel, Win32.GetWindowType.GW_HWNDPREV);
            if (hwndHigher == IntPtr.Zero)
            {
                // hWnd is the highest at z-order, so hwndOverlay has been at top(HWND_TOP code above).
                // So do nothing here.
            }
            else
            {
                Win32.SetWindowPos(hwndOverlay, hwndHigher, 0, 0, 0, 0,
                    Win32.SetWindowPosFlags.NOMOVE | Win32.SetWindowPosFlags.NOSIZE);
                //Debug.WriteLine($"### {(uint)hwndHigher:X8} -> {(uint)hwndOverlay:X8} -> {(uint)hwndToplevel:X8}");
            }

            /* This is unnecessary for Win81 and Win10.1607+
            if (IsAboveWin10_1607())
            {
                DpiUtilities.SetThreadDpiAwarenessContext(thread_oldctx); // restore ctx
            }
            */
        }

        /// <summary>
        /// This function deals with a hard problem on a Win7 with System DPI set to non-100% (assume 150%).
        /// If we (window-finder code) gets a width of 400 from GetWindowRect(hwnd0),
        /// also assuming we are DPI-unaware program, then we are actually facing TWO cases:
        ///  1. The hwnd0 is also DPI-unaware, and it is actually occupying 400*1.5=600 physical pixels.
        ///  2. The hwnd0 is Sysdpi-aware, so it is occupying exactly 400 physical pixels.
        /// Then how do we distinguish between the TWO cases? Windows 7 does not give up out-of-box
        /// API call to distinguish them, so we need to circumvent it brilliantly.
        /// 
        /// This happens similarly as well if we are a System-DPI-awareness program.
        ///
        /// Luckily, there is a way. DwmGetWindowAttribute(DWMWA_EXTENDED_FRAME_BOUNDS) will tell us
        /// a top-level windows's phyiscal dimension(location, width), with the help of DWM-rect,
        /// we can finally distinguish the TWO cases.
        /// 
        /// </summary>
        /// <param name="hwnd0"></param>
        /// <param name="winrect"></param>
        private static void Win7_SpecialAdjust(IntPtr hwnd0, ref RECT rect0)
        {
            Debug.Assert(DpiUtilities.IsWin7());
            Debug.Assert(DpiUtilities.Win7_SystemDpi() > 96);

            // Note: Only top-level window can be queried for DWM window rect.

            IntPtr hwndtop = Win32.GetAncestor(hwnd0, Win32.GetAncestorFlags.GetRoot);
            RECT recttop;
            GetWindowRect(hwndtop, out recttop);

            //RECT rttop = GetWindowRect_DWM(hwndtop);

            //Debug.WriteLine($"DWM: width {rttop.Width} , height {rttop.Height}");

            bool isCallerStdDpi = DpiUtilities.Win7_IsCallerStdDpi();
            int sysdpi = DpiUtilities.Win7_SystemDpi();

            float scale_factor = (float)sysdpi / 96;

            int width_upscaled = recttop.Width * sysdpi / 96;

            int[] top_widths = new int[] { recttop.Width, width_upscaled };

            int match = FindMatchingWidth_byDWM(hwndtop, top_widths[0], top_widths[1]);

            if (isCallerStdDpi)
            {
                // Assume sysdpi=144 (1.5X), and hwnd0 virtual width is 400,
                // then hwnd0 may be in two cases:
                // 1. a DPI-unaware window that has physical width 400*1.5=600 .
                // 2. a Sys-DPI-aware window this has physical width 400 .
                // For case 1, the caller should SetWindowPos() with a width of 600/1.5=400 .
                // For case 2, the caller should SetWindowPos() with a width of 400/1.5=267 .

                if (match == 0) // case 2
                {
                    rect0.left = Convert.ToInt32(rect0.left / scale_factor);
                    rect0.top = Convert.ToInt32(rect0.top / scale_factor);
                    rect0.right = Convert.ToInt32(rect0.right / scale_factor);
                    rect0.bottom = Convert.ToInt32(rect0.bottom / scale_factor);
                }
            }
            else
            {
                // Assume sysdpi=144 (1.5X), and hwnd0 virtual width is 400,
                // then hwnd0 may be in two cases:
                // 1. a DPI-unaware window that has physical width 400*1.5=600 .
                // 2. a Sys-DPI-aware window this has physical width 400 .
                // For case 1, the caller should SetWindowPos() with a width of 600 .
                // For case 2, the caller should SetWindowPos() with a width of 400 .

                if (match == 1) // case 1
                {
                    rect0.left = Convert.ToInt32(rect0.left * scale_factor);
                    rect0.top = Convert.ToInt32(rect0.top * scale_factor);
                    rect0.right = Convert.ToInt32(rect0.right * scale_factor);
                    rect0.bottom = Convert.ToInt32(rect0.bottom * scale_factor);
                }
            }

        }

        private static int FindMatchingWidth_byDWM(IntPtr hwnd, int width0, int width1)
        {
            RECT rtdwm = GetWindowRect_DWM(hwnd);
            int diff0 = Math.Abs(rtdwm.Width - width0);
            int diff1 = Math.Abs(rtdwm.Width - width1);

            if (diff0 < diff1)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Determines whether the two windows are related.
        /// </summary>
        internal static bool IsRelativeWindow(IntPtr hWnd, IntPtr hRelativeWindow, bool bProcessAncestor)
        {
            uint dwProcess = new int(), dwProcessOwner = new int();
            uint dwThread = new int(), dwThreadOwner = new int();
            ;

            // Failsafe
            if (hWnd == IntPtr.Zero)
                return false;
            if (hRelativeWindow == IntPtr.Zero)
                return false;
            if (hWnd == hRelativeWindow)
                return true;

            // Get processes and threads
            dwThread = GetWindowThreadProcessId(hWnd, out dwProcess);
            dwThreadOwner = GetWindowThreadProcessId(hRelativeWindow, out dwProcessOwner);

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
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Point pt,
            [MarshalAs(UnmanagedType.U4)] int cPoints);

        //[DllImport("user32", EntryPoint = "ChildWindowFromPoint", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        //public static extern int ChildWindowFromPoint(IntPtr hWnd, int xPoint, int yPoint);
        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, Point pt);

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
        public static extern IntPtr WindowFromPoint(Point p);

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

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

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
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, DeviceCap nIndex);
        //
        public enum DeviceCap
        {
            HORZRES = 8,
            VERTRES = 10,
            LOGPIXELSX = 88,
            LOGPIXELSY = 90,
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118,
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            SetWindowPosFlags uFlags = 0);
        //
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        //
        [Flags]
        public enum SetWindowPosFlags : uint
        {
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000,
        }

        /// <summary>
        /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);
        //
        public enum GetWindowType : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        /// <summary>
        /// Since Win2000.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        public const int MONITOR_DEFAULTTONULL = 0;
        public const int MONITOR_DEFAULTTOPRIMARY = 1;
        public const int MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, Win32.DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);

        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, Win32.DWMWINDOWATTRIBUTE dwAttribute, out Win32.RECT pvAttribute, int cbAttribute);

        public enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds, // we use this
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }

        /// <summary>
        /// Win7+, only effective on a top-level window.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static RECT GetWindowRect_DWM(IntPtr hwnd)
        {
            RECT rt;
            DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.ExtendedFrameBounds, out rt,
                Marshal.SizeOf(typeof(RECT)));

            return rt;
        }


        /// <summary>
        /// Since Win81
        /// </summary>
        /// <param name="hmonitor"></param>
        /// <param name="dpitype"></param>
        /// <param name="dpiX"></param>
        /// <param name="dpiY"></param>
        /// <returns></returns>
        [DllImport("Shcore.dll")]
        public static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpitype, out int dpiX, out int dpiY);
        //
        public enum Monitor_DPI_Type : int
        {
            Effective = 0,
            Angular = 1,
            Raw = 2,
            Default = Effective
        }
    }



    public static class DpiUtilities
    {
        /// <summary>
        /// Win10.1607+ Set-ThreadDpiAwarenessContext
        /// </summary>
        /// <param name="DpiAwarenessContext">Calling thread wants to use this DPI-awareness-context.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetThreadDpiAwarenessContext(IntPtr DpiAwarenessContext); // Win10.1607
        // -- return old DPI_AWARENESS_CONTEXT enum for this thread

        /// <summary>
        /// Win10.1607+ Get-WindowDpiAwarenessContext.
        /// Return DPI_AWARENESS_CONTEXT id(abstract) for this hwnd
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDpiAwarenessContext(IntPtr hwnd);

        // DPI_AWARENESS_CONTEXT enum values:
        public static IntPtr DPI_AWARENESS_CONTEXT_UNAWARE = (IntPtr)(-1);
        public static IntPtr DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = (IntPtr)(-2);
        public static IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = (IntPtr)(-3); // we only use this here
        public static IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = (IntPtr)(-4);

        /// <summary>
        /// Win10.1607+ Convert fine-grained DPI-awareness-context value to coarse-grained DPI-awareness enum.
        /// </summary>
        /// <param name="dpi_awareness_context"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Win81 GetProcessDpiAwareness. Use it to know our own process DPI-awareness.
        /// </summary>
        /// <param name="hprocess">Process Handle.</param>
        /// <param name="value">Receive output.</param>
        /// <returns>HRESULT.</returns>
        [DllImport("Shcore.dll")]
        public static extern int GetProcessDpiAwareness(IntPtr hprocess, out PROCESS_DPI_AWARENESS value);
        //
        public enum PROCESS_DPI_AWARENESS : int
        {
            PROCESS_DPI_UNAWARE = 0,
            PROCESS_SYSTEM_DPI_AWARE = 1,
            PROCESS_PER_MONITOR_DPI_AWARE = 2,

            PROCESS_DPI_Unset = -4,
        };

        /// <summary>
        /// Win81 SetProcessDpiAwareness() sets DPI-awareness for current process.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>HRESULT</returns>
        [DllImport("Shcore.dll")]
        public static extern int SetProcessDpiAwareness(PROCESS_DPI_AWARENESS value);

        /// <summary>
        /// Win7 IsProcessDPIAware().
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsProcessDPIAware();

        /// <summary>
        /// Win7 SetProcessDPIAware.
        /// </summary>
        /// <returns>Whether set success.</returns>
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        /// <summary>
        /// Detect if we are running on Win81+ by checking if GetProcessDpiAwareness() exists.
        /// </summary>
        /// <returns></returns>
        public static bool IsWin81_or_above()
        {
            try
            {
                PROCESS_DPI_AWARENESS procdaw;
                GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out procdaw);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get DPI value(96, 120, 144) in eye of a target HWND.
        /// This function is tricky.
        /// Limitation? every monitor must be same DPI?
        /// </summary>
        /// <param name="hwnd">The target HWND. If Zero, get DPI for own process.</param>
        /// <returns>DPI value 96, 120, 144 etc.</returns>
        public static int Win81_GetWindowDpi(IntPtr hwnd)
        {
            Debug.Assert(IsWin81_or_above());

            if (hwnd == IntPtr.Zero)
            {
                IntPtr hdc = Win32.GetDC(IntPtr.Zero);
                int dpi = Win32.GetDeviceCaps(hdc, Win32.DeviceCap.LOGPIXELSX);
                return dpi;
            }

            uint pid = 0;
            Win32.GetWindowThreadProcessId(hwnd, out pid);
            Process process = Process.GetProcessById((int)pid);
            // process.MainModule.FileName.ToString();

            PROCESS_DPI_AWARENESS procdaw;
            GetProcessDpiAwareness(process.Handle, out procdaw);

            if (procdaw == DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNAWARE)
            {
                return 96;
            }
            else
            {
                IntPtr hmonitor = Win32.MonitorFromWindow(hwnd, Win32.MONITOR_DEFAULTTONULL);
                if (hmonitor == IntPtr.Zero)
                    return 96; // unexpected
                //
                int sysdpiX, sysdpiY;
                int hr = Win32.GetDpiForMonitor(hmonitor, Win32.Monitor_DPI_Type.Effective, 
                    out sysdpiX, out sysdpiY);
                return sysdpiX;
            }
        }

        /// <summary>
        /// Just for testing Win81 GetDpiForMonitor behavior.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="mdt"></param>
        /// <returns></returns>
        public static int Win81_GetMonitorDpi(IntPtr hwnd,
            Win32.Monitor_DPI_Type mdt = Win32.Monitor_DPI_Type.Effective)
        {
            IntPtr hmonitor = Win32.MonitorFromWindow(hwnd, Win32.MONITOR_DEFAULTTONULL);
            if (hmonitor == IntPtr.Zero)
                return 0;

            int sysdpiX, sysdpiY;
            int hr = Win32.GetDpiForMonitor(hmonitor, mdt, out sysdpiX, out sysdpiY);
            if (hr != 0)
                return hr;

            return sysdpiX;
        }

        public static bool IsWin7()
        {
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                return true;
            else
                return false;
        }

        private static int s_win7_system_dpi = 0; // 0 means unset

        /// <summary>
        /// Get Win7 system DPI, which is a system-wide value set by human user.
        /// It will not change until user logs off and on.
        /// 
        /// This code works both on "caller is 96dpi" and "caller is System-DPI".
        /// </summary>
        /// <returns></returns>
        public static int Win7_SystemDpi()
        {
            if (s_win7_system_dpi > 0)
                return s_win7_system_dpi;

            IntPtr hdc = Win32.GetDC(IntPtr.Zero);

            int selfdpi = Win32.GetDeviceCaps(hdc, Win32.DeviceCap.LOGPIXELSX);
            if (selfdpi > 96)
            {
                // We conclude that it is the very System-DPI, 120, 144 etc.
                s_win7_system_dpi = selfdpi;
            }
            else
            {
                // The system-dpi may be 96, or larger.

                float realres = Win32.GetDeviceCaps(hdc, Win32.DeviceCap.DESKTOPHORZRES);
                float logires = Win32.GetDeviceCaps(hdc, Win32.DeviceCap.HORZRES);

                s_win7_system_dpi = Convert.ToInt32(96 * realres / logires);
            }

            Win32.ReleaseDC(IntPtr.Zero, hdc);

            return s_win7_system_dpi;
        }

        public static bool Win7_IsCallerStdDpi()
        {
            IntPtr hdc = Win32.GetDC(IntPtr.Zero);
            int selfdpi = Win32.GetDeviceCaps(hdc, Win32.DeviceCap.LOGPIXELSX);
            Win32.ReleaseDC(IntPtr.Zero, hdc);

            if (selfdpi == 96)
                return true;
            else
                return false;
        }
    }
}