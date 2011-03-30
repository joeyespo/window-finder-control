using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

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
            if(!windowHandleChanging)
                txtWindowHandle.Text = windowFinder.WindowHandleText;
            txtWindowClass.Text = windowFinder.WindowClass;
            txtWindowText.Text = windowFinder.WindowText;
            txtWindowCharset.Text = windowFinder.WindowCharset;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtWindowHandle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtWindowHandle_TextChanged(object sender, System.EventArgs e)
        {
            IntPtr handle;

            try
            {
                handle = (IntPtr)Convert.ToInt32(txtWindowHandle.Text, 16);
            }
            catch
            {
                handle = IntPtr.Zero;
            }

            windowHandleChanging = true;
            try
            {
                windowFinder.SetWindowHandle(handle);
            }
            finally
            {
                windowHandleChanging = false;
            }
        }

        #endregion

        private bool windowHandleChanging = false;
    }
}
