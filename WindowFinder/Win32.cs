// Win32.cs
// By Joe Esposito

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
        public const int ANYSIZE_ARRAY = unchecked((int)(1));
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
            if(IsWindow(hWnd) == 0)
                return "";

            if(IsWindowUnicode(hWnd) != 0)
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
            if(IsWindow(hWnd) == 0)
                return "";

            if(IsWindowUnicode(hWnd) != 0)
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
            POINTAPI pt;

            pt.x = xPoint;
            pt.y = yPoint;
            ClientToScreen(hClientWnd, ref pt);

            return (IntPtr)WindowFromPoint(pt.x, pt.y);
        }

        /// <summary>
        /// Highlights the specified window.
        /// </summary>
        internal static bool HighlightWindow(IntPtr hWnd)
        {
            IntPtr hDC;                   // The DC of the window.
            RECT rt = new RECT();         // Rectangle area of the window.

            // Get the window DC of the window.
            if((hDC = (IntPtr)GetWindowDC(hWnd)) == IntPtr.Zero)
                return false;

            // Get the screen coordinates of the rectangle of the window.
            GetWindowRect(hWnd, ref rt);
            rt.right -= rt.left;
            rt.left = 0;
            rt.bottom -= rt.top;
            rt.top = 0;

            // Draw a border in the DC covering the entire window area of the window.
            IntPtr hRgn = (IntPtr)CreateRectRgnIndirect(ref rt);
            GetWindowRgn(hWnd, hRgn);
            SetROP2(hDC, R2_NOT);
            FrameRgn(hDC, hRgn, (IntPtr)GetStockObject(WHITE_BRUSH), 3, 3);
            DeleteObject(hRgn);

            // Finally release the DC.
            ReleaseDC(hWnd, hDC);

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
            if(hWnd == IntPtr.Zero)
                return false;
            if(hRelativeWindow == IntPtr.Zero)
                return false;
            if(hWnd == hRelativeWindow)
                return true;

            // Get processes and threads
            dwThread = GetWindowThreadProcessId(hWnd, ref dwProcess);
            dwThreadOwner = GetWindowThreadProcessId(hRelativeWindow, ref dwProcessOwner);

            // Get relative info
            if(bProcessAncestor)
                return (dwProcess == dwProcessOwner);
            return (dwThread == dwThreadOwner);
        }

        [DllImport("user32", EntryPoint = "IsWindow", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int IsWindow(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "IsWindowUnicode", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int IsWindowUnicode(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "SetCapture", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int SetCapture(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "ClientToScreen", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int ClientToScreen(IntPtr hWnd, ref POINTAPI lpPoint);

        [DllImport("user32", EntryPoint = "MapWindowPoints", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref RECT lprt, int cPoints);

        [DllImport("user32", EntryPoint = "MapWindowPoints", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref POINTAPI lppt, int cPoints);

        [DllImport("user32", EntryPoint = "ChildWindowFromPoint", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int ChildWindowFromPoint(IntPtr hWnd, int xPoint, int yPoint);

        [DllImport("user32", EntryPoint = "GetParent", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextLengthA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextLengthA(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextLengthW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextLengthW(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextA(IntPtr hWnd, IntPtr lpString, int cch);

        [DllImport("user32", EntryPoint = "GetWindowTextW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextW(IntPtr hWnd, IntPtr lpString, int cch);

        [DllImport("user32", EntryPoint = "GetClassNameA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetClassNameA(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);

        [DllImport("user32", EntryPoint = "GetClassNameW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetClassNameW(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);

        [DllImport("user32", EntryPoint = "GetWindowDC", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowDC(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowRect", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("gdi32", EntryPoint = "CreateRectRgnIndirect", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int CreateRectRgnIndirect(ref RECT lpRect);

        [DllImport("user32", EntryPoint = "WindowFromPoint", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32", EntryPoint = "GetWindowRgn", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("gdi32", EntryPoint = "SetROP2", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int SetROP2(IntPtr hdc, int nDrawMode);

        [DllImport("gdi32", EntryPoint = "FrameRgn", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int FrameRgn(IntPtr hdc, IntPtr hRgn, IntPtr hBrush, int nWidth, int nHeight);

        [DllImport("gdi32", EntryPoint = "GetStockObject", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetStockObject(int nIndex);

        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);

        [DllImport("gdi32", EntryPoint = "DeleteObject", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int DeleteObject(IntPtr hObject);

        [DllImport("user32", EntryPoint = "ReleaseDC", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hdc);

        // Binary raster ops
        public const int R2_NOT = unchecked((int)(6));//  Dn

        // Stock Logical Objects
        public const int WHITE_BRUSH = unchecked((int)(0));
    }
}
