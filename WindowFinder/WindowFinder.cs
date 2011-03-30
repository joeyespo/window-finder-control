using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace WindowFinder
{
    [DefaultEvent("WindowHandleChanged")]
    [ToolboxBitmap(typeof(ResourceFinder), "WindowFinder.Resources.WindowFinder.bmp")]
    [Designer(typeof(WindowFinderDesigner))]
    public sealed partial class WindowFinder : UserControl
    {
        public WindowFinder()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            SetStyle(ControlStyles.FixedWidth, true);
            SetStyle(ControlStyles.FixedHeight, true);
            SetStyle(ControlStyles.StandardClick, false);
            SetStyle(ControlStyles.StandardDoubleClick, false);

            picTarget.Size = new Size(31, 28);
            Size = picTarget.Size;
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

        /// <summary>
        /// Sets the window handle if handle is a valid window.
        /// </summary>
        /// <param name="handle">The handle to set to.</param>
        public void SetWindowHandle(IntPtr handle)
        {
            if((Win32.IsWindow(handle) == 0) || (Win32.IsRelativeWindow(handle, this.Handle, true)))
            {
                // Clear window information
                windowHandle = IntPtr.Zero;
                windowHandleText = "";
                windowClass = "";
                windowText = "";
                isWindowUnicode = false;
                windowCharset = "";
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
            }
            if(WindowHandleChanged != null)
                WindowHandleChanged(this, EventArgs.Empty);
        }

        private void WindowFinder_Load(object sender, System.EventArgs e)
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

        private void picTarget_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Set capture image and cursor
            picTarget.Image = bitmapFinda;
            picTarget.Cursor = cursorTarget;

            // Set capture
            Win32.SetCapture(picTarget.Handle);

            // Begin targeting
            bTargeting = true;
            hCurrentTarget = IntPtr.Zero;

            // Show info   TODO: Put into function for mousemove & mousedown
            SetWindowHandle(picTarget.Handle);
        }
        private void picTarget_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IntPtr hWnd;
            IntPtr hTemp;
            Win32.POINTAPI pt;

            // TODO: Draw border around EVERY window

            pt.x = e.X;
            pt.y = e.Y;
            Win32.ClientToScreen(picTarget.Handle, ref pt);

            // Make sure targeting before highlighting windows
            if(!bTargeting)
                return;

            // Get screen coords from client coords and window handle
            hWnd = Win32.WindowFromPoint(picTarget.Handle, e.X, e.Y);

            // Get real window
            if(hWnd != IntPtr.Zero)
            {
                hTemp = IntPtr.Zero;

                while(true)
                {
                    Win32.MapWindowPoints(hTemp, hWnd, ref pt, 1);
                    hTemp = (IntPtr)Win32.ChildWindowFromPoint(hWnd, pt.x, pt.y);
                    if(hTemp == IntPtr.Zero)
                        break;
                    if(hWnd == hTemp)
                        break;
                    hWnd = hTemp;
                }

                /* TODO: Work with ALL windows
                Win32.ScreenToClient(hWnd, ref pt);
                Win32.MapWindowPoints(IntPtr.Zero, hWnd, ref pt, 2);
                if ((hTemp = (IntPtr)Win32.ChildWindowFromPoint(hWnd, pt.x, pt.y)) != IntPtr.Zero) 
                {
                  hWnd = hTemp;
                }
                // */
            }

            // Get owner
            while((hTemp = Win32.GetParent(hWnd)) != IntPtr.Zero)
                hWnd = hTemp;

            // Show info
            SetWindowHandle(hWnd);

            // Highlight valid window
            highlightValidWindow(hWnd, this.Handle);
        }
        private void picTarget_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IntPtr hWnd;
            IntPtr hTemp;

            // End targeting
            bTargeting = false;

            // Unhighlight window
            if(hCurrentTarget != IntPtr.Zero)
                Win32.HighlightWindow(hCurrentTarget);
            hCurrentTarget = IntPtr.Zero;

            // Reset capture image and cursor
            picTarget.Cursor = Cursors.Default;
            picTarget.Image = bitmapFind;

            // Get screen coords from client coords and window handle
            hWnd = Win32.WindowFromPoint(picTarget.Handle, e.X, e.Y);

            // Get owner
            while((hTemp = Win32.GetParent(hWnd)) != IntPtr.Zero)
                hWnd = hTemp;

            SetWindowHandle(hWnd);

            // Release capture
            Win32.SetCapture(IntPtr.Zero);
        }

        void highlightValidWindow(IntPtr hWnd, IntPtr hOwner)
        {
            // Check for valid highlight
            if(hCurrentTarget == hWnd)
                return;

            // Check for relative
            if(Win32.IsRelativeWindow(hWnd, hOwner, true))
            {
                // Unhighlight last window
                if(hCurrentTarget != IntPtr.Zero)
                {
                    Win32.HighlightWindow(hCurrentTarget);
                    hCurrentTarget = IntPtr.Zero;
                }

                return;
            }

            // Unhighlight last window
            Win32.HighlightWindow(hCurrentTarget);

            // Set as current target
            hCurrentTarget = hWnd;

            // Highlight window
            Win32.HighlightWindow(hWnd);
        }

        /// <summary>
        /// Called when the WindowHandle property is changed.
        /// </summary>
        public event EventHandler WindowHandleChanged;

        private bool bTargeting = false;
        private Cursor cursorTarget = null;
        private Bitmap bitmapFind = null;
        private Bitmap bitmapFinda = null;
        private IntPtr hCurrentTarget = IntPtr.Zero;
        private IntPtr windowHandle = IntPtr.Zero;
        private string windowHandleText = "";
        private string windowClass = "";
        private string windowText = "";
        private bool isWindowUnicode = false;
        private string windowCharset = "";
    }
}
