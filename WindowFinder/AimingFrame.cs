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
        static Color MyTransParentColor = Color.Black;

        public AimingFrame()
        {
            this.Text = "Aiming Frame";

            this.FormBorderStyle = FormBorderStyle.None;

            // Enable this to make a "tool window", so it does NOT appear in taskbar
            this.ShowInTaskbar = false; 

            this.TransparencyKey = MyTransParentColor;

            // Add a secret sizing grip at bottom right corner(optional). Effective?
            //this.SizeGripStyle = SizeGripStyle.Show;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen BorderPen = new Pen(Color.Red, 6))
            {
                e.Graphics.DrawRectangle(BorderPen, this.ClientRectangle);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (Brush BgBrush = new SolidBrush(MyTransParentColor))
            {
                e.Graphics.FillRectangle(BgBrush, this.ClientRectangle);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

    }
}
