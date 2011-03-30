using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace TestControl
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using(MainForm dialog = new MainForm())
                Application.Run(dialog);
        }
    }
}
