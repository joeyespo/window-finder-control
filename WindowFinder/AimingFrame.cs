using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowFinder
{
    class AimingFrame : Form
    {
        private static Color MyTransparentColor = Color.Black;
        private static Color FrameColor = Color.Red;
        private static int FrameWidth = 3;


        public AimingFrame()
        {
            this.Text = "Aiming Frame";

            this.FormBorderStyle = FormBorderStyle.None;

            // Enable this to make a "tool window", so it does NOT appear in taskbar
            this.ShowInTaskbar = false; 

            this.TransparencyKey = MyTransparentColor;

            // Create a hint label to show aimed area width info.

            this.lblHint = new System.Windows.Forms.Label();
            this.Controls.Add(this.lblHint);
            this.lblHint.BackColor = Color.White;
            this.lblHint.ForeColor = FrameColor;
            this.lblHint.Location = new System.Drawing.Point(FrameWidth+1, FrameWidth+1);
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new Font("Tahoma", 8);
            this.lblHint.Text = "ABC"; // set later
        }

        public void SetHint(string text)
        {
            lblHint.Text = text;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen BorderPen = new Pen(FrameColor, FrameWidth*2))
            {
                e.Graphics.DrawRectangle(BorderPen, this.ClientRectangle);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (Brush BgBrush = new SolidBrush(MyTransparentColor))
            {
                e.Graphics.FillRectangle(BgBrush, this.ClientRectangle);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        private System.Windows.Forms.Label lblHint;
    }
}
