using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using WindowFinder;

namespace TestControl
{
    /// <summary>
    /// Represents the test form.
    /// </summary>
    public sealed partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            this.radiobtnSnapFrame.Checked = true;
            lblScreenMousePos.Text = "";

            ckbMyThread.CheckFromCode(true);
            ckbMyProcess.CheckFromCode(true);

            ckbScreenshot.CheckFromCode(true);
        }

        #region Event Handler Methods

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the WindowHandleChanged event of the windowFinder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void windowFinder_WindowHandleChanged(object sender, System.EventArgs e)
        {
            txtWindowHandle.Text = windowFinder.WindowHandleText;
            txtWindowClass.Text = windowFinder.WindowClass;
            txtWindowText.Text = windowFinder.WindowText;
            txtWindowCharset.Text = windowFinder.WindowCharset;

            Rectangle rt = windowFinder.WindowRect;
            if (rt == Rectangle.Empty)
                txtWindowRect.Text = "";
            else
                txtWindowRect.Text = $"[{rt.Width} x {rt.Height}] LT({rt.Left}, {rt.Top}) RB({rt.Right}, {rt.Bottom})";
        }

        private void windowFinder_MouseDraggingChanged(object sender, MouseDraggingEventArgs e)
        {
            if (e.isStop)
            {
                lblFinderTool.Text = "Finder Tool:";
                lblScreenMousePos.Text = "";
            }
            else
            {
                lblFinderTool.Text = "Mouse at screen:";
                lblScreenMousePos.Text = $"X:{e.X} , Y:{e.Y}";
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtWindowHandle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtWindowHandle_TextChanged(object sender, System.EventArgs e)
        {
            IntPtr handle = IntPtr.Zero;
            try
            {
                if (txtWindowHandle.Text != "")
                {
                    handle = (IntPtr)Convert.ToInt32(txtWindowHandle.Text, 16);
                }
            }
            catch
            {
            }

            try
            {
                windowFinder.SetWindowHandle(handle);
            }
            finally
            {
            }
        }
        
        #endregion

        private void radiobtnInvertColor_CheckedChanged(object sender, EventArgs e)
        {
            // memo: this will be triggered when either of the radio button is clicked.

            this.windowFinder.tgwHighlightMethod = radiobtnInvertColor.Checked
                ? WindowFinder.WindowFinder.HighlightMethod.InvertColor
                : WindowFinder.WindowFinder.HighlightMethod.SnapFrame;

            RefreshUIByCfg();
        }

        private static int verTestApp = 10;

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Text += $" ver:{WindowFinder.WindowFinder.verMajor}.{WindowFinder.WindowFinder.verMinor}.{verTestApp}";

            DpiUtilities.PROCESS_DPI_AWARENESS procdaw = DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_DPI_Unset;

            try
            {
                // only Win81+
                IntPtr hCurrentProcess = Process.GetCurrentProcess().Handle;
                DpiUtilities.GetProcessDpiAwareness(hCurrentProcess, out procdaw); 
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            if(procdaw == DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_DPI_Unset)
            {
                // For Win7/XP
                bool win7daw = WindowFinder.DpiUtilities.IsSelfProcessDPIAware();
                if (win7daw)
                    procdaw = DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE;
                else
                    procdaw = DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNAWARE;
            }

            string hint = "";
            if (procdaw == DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNAWARE)
                hint += "DPI unaware";
            else if (procdaw == DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE)
                hint += "System-DPI aware";
            else if (procdaw == DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE)
                hint += "Per-monitor-DPI aware";

            lblDpiAwareness.Text = hint;

            if (DpiUtilities.IsWin81())
            {
                // Warn program limitation on Win81 and Win10.1607- . (not for Win7 & Win10.1607+)

                if (procdaw != DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE
                    && Screen.AllScreens.Length > 1)
                {
                    string info = @"Since you are running this program on Win81 or pre-Win10.1607 with more than one monitor, and this program is NOT run with Per-monitor aware mode, this results in two issues: 

First. The ""traditional"" Invert-color highlighting method does not always work correctly in non-100% DPI-scaling environment.

Second. The Snap-frame method has issues in mixmode DPI-scaling environment: Although it visually frames the target window correctly, it does NOT screenshot the window correctly, due to unreliable physical window width detection. 

So, on Win81 and pre-Win10.1607, to make this program run correctly, you must pass parameter ""2"" to it, to make it run in Per-monitor-aware mode.
";
                    MessageBox.Show(info, "Alert",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Program.ShowParameterHelp();
        }

        private void RefreshUIByCfg()
        {
            if (this.windowFinder.isIncludeMyProcess)
            {
                ckbMyProcess.CheckFromCode(true);

                ckbMyThread.Enabled = true;
                ckbMyThread.CheckFromCode(this.windowFinder.isIncludeMyThread);
            }
            else
            {
                ckbMyProcess.CheckFromCode(false);

                ckbMyThread.Enabled = false;
                ckbMyThread.CheckFromCode(false);
            }

            //

            if (this.windowFinder.tgwHighlightMethod == WindowFinder.WindowFinder.HighlightMethod.SnapFrame)
            {
                this.radiobtnSnapFrame.Checked = true;

                ckbScreenshot.Enabled = true;

                if (this.windowFinder.isDoScreenshot)
                    ckbScreenshot.CheckFromCode(true);
                else
                    ckbScreenshot.CheckFromCode(false);
            }
            else
            {
                this.radiobtnInvertColor.Checked = true;

                ckbScreenshot.Enabled = false;

                ckbScreenshot.CheckFromCode(false);
            }
        }

        private void ckbMyThread_CheckedChanged_ByHuman(object sender, EventArgs e)
        {
            this.windowFinder.isIncludeMyThread = ckbMyThread.Checked;

            RefreshUIByCfg();
        }

        private void ckbMyProcess_CheckedChanged_ByHuman(object sender, EventArgs e)
        {
            this.windowFinder.isIncludeMyProcess = ckbMyProcess.Checked;

            RefreshUIByCfg();
        }

        private void ckbScreenshot_CheckedChanged_ByHuman(object sender, EventArgs e)
        {
            this.windowFinder.isDoScreenshot = ckbScreenshot.Checked;

            RefreshUIByCfg();
        }
    }

    public class MyCheckBox : CheckBox
    {
        public MyCheckBox()
        {
            this.CheckedChanged += MyCheckedChanged;
        }

        private bool _isCheckFromCode = false;

        public event EventHandler CheckedChanged_ByHuman;

        public void CheckFromCode(bool isChecked)
        {
            _isCheckFromCode = true;

            this.Checked = isChecked;

            _isCheckFromCode = false;
        }

        public bool IsCheckingFromCode()
        {
            if (_isCheckFromCode)
                return true;
            else
                return false;
        }

        private void MyCheckedChanged(object sender, EventArgs e)
        {
            if (_isCheckFromCode)
                return;

            if(CheckedChanged_ByHuman!=null)
                CheckedChanged_ByHuman(sender, e);
        }
    }
}