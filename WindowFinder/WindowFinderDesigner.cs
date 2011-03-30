using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WindowFinder
{
    public class WindowFinderDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return (SelectionRules.Moveable | SelectionRules.Visible);
            }
        }
    }
}
