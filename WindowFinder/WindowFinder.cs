using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowFinder
{
    [DefaultEvent("WindowHandleChanged")]
    [ToolboxBitmap(typeof(ResourceFinder), "WindowFinder.Resources.WindowFinder.bmp")]
    [Designer(typeof(WindowFinderDesigner))]
    public sealed partial class WindowFinder : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowFinder"/> class.
        /// </summary>
        public WindowFinder()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            SetStyle(ControlStyles.FixedWidth, true);
            SetStyle(ControlStyles.FixedHeight, true);
            SetStyle(ControlStyles.StandardClick, false);
            SetStyle(ControlStyles.StandardDoubleClick, false);
            SetStyle(ControlStyles.Selectable, false);

            picTarget.Size = new Size(31, 28);
            Size = picTarget.Size;

            _snapframe = new SnapFrame();

            _timerCheckKey = new Timer();
            _timerCheckKey.Interval = 100;
            _timerCheckKey.Tick += new EventHandler(TimerCheckKey);
        }

        void TimerCheckKey(object obj, EventArgs ea)
        {
            bool isCtrlKeyDown = Control.ModifierKeys == Keys.Control;

            if (isCtrlKeyDown != _wasCtrlKeyDown)
            {
                _wasCtrlKeyDown = isCtrlKeyDown;
                RetargetMyHwnd();
            }
        }

        #region Public Properties

        /// <summary>
        /// Called when the WindowHandle property is changed.
        /// </summary>
        public event EventHandler WindowHandleChanged;

        void OnWindowHandleChanged()
        {
            if (WindowHandleChanged != null)
                WindowHandleChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// When user is dragging the finder, this event fires.
        /// When user stops dragging the finder, this event fires with isStop=true.
        /// </summary>
        public event EventHandler<MouseDraggingEventArgs> MouseDraggingChanged;

        void OnMouseDraggingChanged(MouseDraggingEventArgs e)
        {
            if (MouseDraggingChanged != null)
                MouseDraggingChanged(this, e);
        }

        /// <summary>
        /// Handle of the window found.
        /// </summary>
        [Browsable(false)]
        public IntPtr WindowHandle
        {
            get
            {
                return windowHandle;
            }
        }

        /// <summary>
        /// Handle text of the window found.
        /// </summary>
        [Browsable(false)]
        public string WindowHandleText
        {
            get
            {
                return windowHandleText;
            }
        }

        /// <summary>
        /// Class of the window found.
        /// </summary>
        [Browsable(false)]
        public string WindowClass
        {
            get
            {
                return windowClass;
            }
        }

        /// <summary>
        /// Text of the window found.
        /// </summary>
        [Browsable(false)]
        public string WindowText
        {
            get
            {
                return windowText;
            }
        }

        /// <summary>
        /// Whether or not the found window is unicode.
        /// </summary>
        [Browsable(false)]
        public bool IsWindowUnicode
        {
            get
            {
                return isWindowUnicode;
            }
        }

        /// <summary>
        /// Whether or not the found window is unicode, via text.
        /// </summary>
        [Browsable(false)]
        public string WindowCharset
        {
            get
            {
                return windowCharset;
            }
        }

        public Rectangle WindowRect
        {
            get { return _tgWinRect;  }
        }

        public Point ScreenMousePos { get; private set; }


        /// <summary>
        /// If true, only top-level window can be highlighted.
        /// </summary>
        [Browsable(true)]
        public bool isFindOnlyTopLevel { get; set; } = false;

        public enum HighlightMethod
        {
            InvertColor = 0,
            SnapFrame = 1,
        }

        /// <summary>
        /// Select from one of two window-highlighting method.
        /// InvertColor is the traditional one, which loses visual effect on Win7+ DWM top-level window.
        /// SnapFrame uses a layered window(since Win2000) to demarcate a window's border, always works.
        /// </summary>
        [Browsable(true)]
        public HighlightMethod tgwHighlightMethod { get; set; } = HighlightMethod.SnapFrame;

        /// <summary>
        /// This refers to a special layered-window that is visually a red frame.
        /// This red frame will tell human user which window he is currently aiming.
        /// </summary>
        private SnapFrame _snapframe;

        /// <summary>
        /// If true, when user release the mouse button, the screen image on target window location is
        /// copied to clipboard.
        /// Note: This feature work only in SnapFrame method.
        /// For InvertColor method, it is much harder or impractical to know target window location
        /// in various situations.
        /// </summary>
        [Browsable(true)]
        public bool isDoScreenshot { get; set; } = true;

        [Browsable(true)]
        public bool isIncludeMyProcess { get; set; } = true;

        /// <summary>
        /// If isIncludeMyProcess=false, then isIncludeMyThread is effectively *false* -- even if
        /// isIncludeMyThread's value is still true.
        /// </summary>
        [Browsable(true)]
        public bool isIncludeMyThread { get; set; } = true;

        #endregion

        #region Event Handler Methods

        /// <summary>
        /// Handles the Load event of the WindowFinder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void WindowFinder_Load(object sender, EventArgs e)
        {
            this.Size = picTarget.Size;
            try
            {
                // Load cursors
                Assembly assembly = Assembly.GetExecutingAssembly();
                cursorTarget = new Cursor(assembly.GetManifestResourceStream("WindowFinder.curTarget.cur"));
                bitmapFind = new Bitmap(assembly.GetManifestResourceStream("WindowFinder.bmpFind.bmp"));
                bitmapFinda = new Bitmap(assembly.GetManifestResourceStream("WindowFinder.bmpFinda.bmp"));
            }
            catch(Exception x)
            {
                // Show error
                MessageBox.Show(this, "Failed to load resources.\n\n" + x.ToString(), "WindowFinder");

                // Attempt to use backup cursor
                if(cursorTarget == null)
                    cursorTarget = Cursors.Cross;
            }

            // Set default values
            picTarget.Image = bitmapFind;
        }

        private Point _RelaMousePos;

        /// <summary>
        /// Handles the MouseDown event of the picTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void picTarget_MouseDown(object sender, MouseEventArgs e)
        {
            // Set capture image and cursor
            picTarget.Image = bitmapFinda;
            picTarget.Cursor = cursorTarget;

            // Set capture
            Win32.SetCapture(picTarget.Handle);

            // Begin targeting
            isTargeting = true;
            targetWindow = IntPtr.Zero;

            _RelaMousePos = new Point(e.X, e.Y);

            // Show info
            SetWindowHandle(picTarget.Handle);

            _wasCtrlKeyDown = false;
            _timerCheckKey.Start();
        }

        private Timer _timerCheckKey = null;
        private bool _wasCtrlKeyDown = false;


        private Win32.RECT _target_hwnd_screen_rect; // in physical(pixel) coordinates
        private bool _is_target_rect_accurate;

        /// <summary>
        /// Handles the MouseMove event of the picTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void picTarget_MouseMove(object sender, MouseEventArgs e)
        {
            // Make sure targeting before highlighting windows
            if(!isTargeting)
                return;

            _RelaMousePos = new Point(e.X, e.Y);

            RetargetMyHwnd();
        }

        /// <summary>
        /// Handles the MouseUp event of the picTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void picTarget_MouseUp(object sender, MouseEventArgs e)
        {
            // End targeting
            isTargeting = false;

            // Unhighlight window
            if (targetWindow != IntPtr.Zero)
            {
                ToggleWindowHighlight(targetWindow);
            }

            // Reset capture image and cursor
            picTarget.Cursor = Cursors.Default;
            picTarget.Image = bitmapFind;

            // Release capture
            Win32.SetCapture(IntPtr.Zero);

            _timerCheckKey.Stop();

            OnMouseDraggingChanged(new MouseDraggingEventArgs(ScreenMousePos.X, ScreenMousePos.Y, true));

            if (isDoScreenshot && tgwHighlightMethod==HighlightMethod.SnapFrame)
            {
                MyCaptureToClipboard();
            }
        }

        private bool IsFromSnapFrame(IntPtr hwnd)
        {
            IntPtr hwndToplevel = Win32.GetMyToplevelWnd(hwnd);
            if (hwndToplevel == _snapframe.Handle)
                return true;
            else
                return false;
        }

        private void RetargetMyHwnd()
        {
            Point pt = _RelaMousePos;
            Win32.ClientToScreen(picTarget.Handle, ref pt);

            ScreenMousePos = pt;
            OnMouseDraggingChanged(new MouseDraggingEventArgs(ScreenMousePos.X, ScreenMousePos.Y, false));

			//Debug.WriteLine($"### RetargetMyHwnd(): {_RelaMousePos.X},{_RelaMousePos.Y} -> {pt.X},{pt.Y}");

            // Get screen coords from client coords and window handle
            IntPtr hChild1 = Win32.WindowFromPoint(IntPtr.Zero, pt.X, pt.Y);
            // -- We name it "child" bcz it must be a child-or-grand-child of the Desktop window.

            if (IsFromSnapFrame(hChild1))
                return;

            // Get real window
            if (hChild1 != IntPtr.Zero)
            {
                // MSDN undoc: 
                // Case 1: Under normal situation, WindowFromPoint() gives us most visible child-window HWND.
                // Case 2: If top-level window X brings up a modal dialog(About box etc), then a child window
                //         of X will not be reported by WindowFromPoint() but X is reported instead.
                // To cope with Case 2, we have to call ChildWindowFromPointEx() recursively until we find
                // the most visible window.

                IntPtr hParent = IntPtr.Zero;

                while (true)
                {
                    Win32.MapWindowPoints(hParent, hChild1, ref pt, 1);

                    IntPtr hChild2 = (IntPtr)Win32.ChildWindowFromPointEx(hChild1, pt,
                        Win32.ChildWindowFromPointFlags.CWP_SKIPINVISIBLE);

                    if (hChild2 == IntPtr.Zero)
                        break;
                    if (hChild1 == hChild2)
                        break;

                    hParent = hChild1;
                    hChild1 = hChild2;
                }
            }

            if (isFindOnlyTopLevel || _wasCtrlKeyDown)
            {
                hChild1 = Win32.GetMyToplevelWnd(hChild1);
            }

            // Show info
            SetWindowHandle(hChild1);

            // Highlight valid window
            HighlightValidWindow(hChild1, this.Handle);
        }

        private void MyCaptureToClipboard()
        {
            Win32.RECT rt = _target_hwnd_screen_rect;
            Bitmap bm = new Bitmap(rt.Width, rt.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.CopyFromScreen(rt.left, rt.top, 0, 0, new Size(rt.Width, rt.Height));

            Clipboard.SetImage(bm);
        }

        private void picTarget_KeyDown(object sender, KeyEventArgs e)
        {
            // Chj: This cannot be triggered! Why?

            Debug.WriteLine($"picTarget_KeyDown({e.KeyCode})...");
            if (e.KeyCode == Keys.Control)
            {
                RetargetMyHwnd();
            }
        }

        private void picTarget_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"picTarget_KeyUp({e.KeyCode})...");
            if (e.KeyCode == Keys.Control)
            {
                RetargetMyHwnd();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the window handle if handle is a valid window.
        /// </summary>
        /// <param name="handle">The handle to set to.</param>
        public void SetWindowHandle(IntPtr handle)
        {
            IntPtr oldhwnd = windowHandle;

            if((Win32.IsWindow(handle) == 0) || !IsIncludeWindow(handle, this.Handle))
            {
                // Clear window information
                windowHandle = IntPtr.Zero;
                windowHandleText = string.Empty;
                windowClass = string.Empty;
                windowText = string.Empty;
                isWindowUnicode = false;
                windowCharset = string.Empty;
                _tgWinRect = Rectangle.Empty;
            }
            else
            {
                // Set window information
                windowHandle = handle;
                windowHandleText = Convert.ToString(handle.ToInt32(), 16).ToUpper().PadLeft(8, '0');
                windowClass = Win32.GetClassName(handle);
                windowText = Win32.GetWindowText(handle);
                isWindowUnicode = Win32.IsWindowUnicode(handle) != 0;
                windowCharset = ((isWindowUnicode) ? ("Unicode") : ("Ansi"));

                Win32.RECT rt = new Win32.RECT();
                Win32.GetWindowRect(handle, out rt);
                _tgWinRect = new Rectangle(rt.left, rt.top, rt.right-rt.left, rt.bottom-rt.top);
            }

            if(oldhwnd != windowHandle)
                OnWindowHandleChanged();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Determine whether hwndOther should be *included* as a finding target.
        /// Implicit input: this.isIncludeMyThread, this.isIncludeMyProcess .
        /// </summary>
        /// <param name="hwndOther">the HWND currently under mouse pointer.</param>
        /// <param name="hwndMy">the HWND from the calling thread.</param>
        /// <returns></returns>
        private bool IsIncludeWindow(IntPtr hwndOther, IntPtr hwndMy)
        {
            uint dwProcess, dwProcessOther;
            uint dwThread, dwThreadOther;

            // Failsafe
            if (hwndMy == IntPtr.Zero)
                return false;
            if (hwndMy == IntPtr.Zero)
                return false;
            if (hwndMy == hwndOther)
                return false;

            bool effective_include_my_thread = isIncludeMyThread;
            //
            if (isIncludeMyProcess == false)
                effective_include_my_thread = false; // gets overridden

            if (effective_include_my_thread)
                return true;

            // Get process-id and thread-id
            dwThread = Win32.GetWindowThreadProcessId(hwndMy, out dwProcess);
            dwThreadOther = Win32.GetWindowThreadProcessId(hwndOther, out dwProcessOther);

            if (isIncludeMyProcess)
                return (dwThread == dwThreadOther) ? false : true;
            else 
                return (dwProcess == dwProcessOther) ? false : true;
        }

        /// <summary>
        /// Highlights the target window, and the API user can decide whether
        /// [a window/control from the calling thread/process] is considered a valid target.
        /// Previous highlight(of an old window) is turned off at the same time.
        /// </summary>
        private void HighlightValidWindow(IntPtr hWnd, IntPtr hOwner)
        {
            // Check for valid highlight
            if(targetWindow == hWnd)
                return;

            // Check for inclusion
            if(!IsIncludeWindow(hWnd, hOwner))
            {
                // Unhighlight last window
                if(targetWindow != IntPtr.Zero)
                {
                    ToggleWindowHighlight(targetWindow);
                }

                return;
            }

            // Unhighlight previous window
            ToggleWindowHighlight(targetWindow);

            // Highlight new window
            ToggleWindowHighlight(hWnd);
        }

//      private int s_DwmRenderingEnabled = -1;

        /// <summary>
        /// If hWnd was highlight-on, turn it off;
        /// If hWnd was highlight-off, turn it on.
        ///
        /// Implicit input/output: this.targetWindow
        /// 
        /// </summary>
        /// <param name="hWnd">the HWND to toggle</param>
        private void ToggleWindowHighlight(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return;

            Debug.Assert(targetWindow==IntPtr.Zero || targetWindow==hWnd);

            if (tgwHighlightMethod == HighlightMethod.InvertColor)
            {
                /*
                if (targetWindow == IntPtr.Zero)
                {
                    // highlight on for new window, new window DWM NcRendering should be turned OFF.

                    if (Win32.IsToplevelWindow(hWnd))
                    {
                        s_DwmRenderingEnabled = Win32.DWM_SetNonclientRendering(hWnd, Win32.DWMNCRP_DISABLED);
                    }
                }
                else
                {
                    if (Win32.IsToplevelWindow(targetWindow))
                    {
                        // highlight off for current highlighting window.
                        if (s_DwmRenderingEnabled == 0 || s_DwmRenderingEnabled == 1)
                        {
                            Win32.DWM_SetNonclientRendering(hWnd, s_DwmRenderingEnabled==1 ? Win32.DWMNCRP_ENABLED : Win32.DWMNCRP_DISABLED);
                        }
                    }
                }
                */

                Win32.HighlightWindow_InvertColor(hWnd);
            }
            else
            {
                if (targetWindow == IntPtr.Zero)
                {
                    // highlight on for new window
                    // Note: If I place this _snapframe.Show() after HighlightWindow_Overlaying(),
                    // the Snap-frame will be brought to front, which is not desired.
                    _snapframe.Show(); 

                    Win32.RECT rtp;
                    bool accurate = Win32.HighlightWindow_Overlaying(hWnd, _snapframe.Handle, out rtp);

                    string qm = accurate ? "" : "(?) ";

                    _snapframe.SetHint($"{qm}{rtp.Width} x {rtp.Height}");

                    _target_hwnd_screen_rect = rtp;
                    _is_target_rect_accurate = accurate;
                }
                else
                {
                    _snapframe.Hide(); // highlight off for old window
                }
            }

            if (targetWindow == IntPtr.Zero)
                targetWindow = hWnd; // highlight ON
            else
                targetWindow = IntPtr.Zero; // highlight OFF
        }



        #endregion

        private bool isTargeting = false;
        private Cursor cursorTarget = null;
        private Bitmap bitmapFind = null;
        private Bitmap bitmapFinda = null;
        private IntPtr targetWindow = IntPtr.Zero;
        private IntPtr windowHandle = IntPtr.Zero;
        private string windowHandleText = string.Empty;
        private string windowClass = string.Empty;
        private string windowText = string.Empty;
        private bool isWindowUnicode = false;
        private string windowCharset = string.Empty;
        private Rectangle _tgWinRect = Rectangle.Empty;
    }


    public class MouseDraggingEventArgs : System.EventArgs
    {
        public readonly int X;
        public readonly int Y;
        public readonly bool isStop;

        public MouseDraggingEventArgs(int X, int Y, bool isStop=false)
        {
            this.X = X;
            this.Y = Y;
            this.isStop = isStop;
        }
    }
}
