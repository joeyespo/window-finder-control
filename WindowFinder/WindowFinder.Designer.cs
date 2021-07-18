using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowFinder
{
    partial class WindowFinder
    {
        /// <summary>
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picTarget = new PictureBox();
            this.SuspendLayout();
            // 
            // picTarget
            // 
            this.picTarget.BackColor = Color.White;
            this.picTarget.Name = "picTarget";
            this.picTarget.Size = new Size(92, 104);
            this.picTarget.TabIndex = 0;
            this.picTarget.TabStop = false;
            this.picTarget.MouseUp += new MouseEventHandler(this.picTarget_MouseUp);
            this.picTarget.MouseMove += new MouseEventHandler(this.picTarget_MouseMove);
            this.picTarget.MouseDown += new MouseEventHandler(this.picTarget_MouseDown);
            this.picTarget.KeyDown += picTarget_KeyDown;
            this.picTarget.KeyUp += picTarget_KeyUp;
            // 
            // WindowFinder
            // 
            this.Controls.AddRange(new Control[] {
                                                                  this.picTarget});
            this.Name = "WindowFinder";
            this.Size = new Size(272, 216);
            this.Load += new EventHandler(this.WindowFinder_Load);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components;
        private PictureBox picTarget;
    }
}
