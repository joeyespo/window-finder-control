using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using WindowFinder;

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
            MySetInitDpi();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using(MainForm dialog = new MainForm())
                Application.Run(dialog);
        }

        static string mbtitle = "WindowFinder Tester";

        static void MySetInitDpi()
        {
            const int S_OK = 0;
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length == 1) // No argument
                return;

            string input = args[1];

            if (input == "0") // PROCESS_DPI_UNAWARE
            {
                int hr = -1;
                try
                {
                    hr = DpiUtilities.SetProcessDpiAwareness(DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_DPI_UNAWARE);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    MessageBox.Show("You pass in parameter 0.\r\n" +
                                    "\r\n" +
                                    "SetProcessDpiAwareness(PROCESS_DPI_UNAWARE) call fails, bcz it requires Win81+.",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                    return;
                }

                if (hr == 0)
                {
                    MessageBox.Show("You pass in parameter 0.\r\n" +
                                    "\r\n" +
                                    "SetProcessDpiAwareness(PROCESS_DPI_UNAWARE) returns success.",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show($"You pass in parameter 0.\r\n" +
                                    $"\r\n" +
                                    $"SetProcessDpiAwareness(PROCESS_DPI_UNAWARE) call fails. (0x{hr:X8})",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );
                }
            }
            else if (input == "1" && DpiUtilities.IsWin81_or_above())
            {
                int hr = -1;
                try
                {
                    hr = DpiUtilities.SetProcessDpiAwareness(DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    MessageBox.Show("You pass in parameter 1.\r\n" +
                                    "\r\n" +
                                    "SetProcessDpiAwareness(PROCESS_SYSTEM_DPI_AWARE) call fails. Program BUG!",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                    return;
                }

                if (hr == S_OK)
                {
                    MessageBox.Show("You pass in parameter 1.\r\n" +
                                    "\r\n" +
                                    "SetProcessDpiAwareness(PROCESS_SYSTEM_DPI_AWARE) returns success.",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show($"You pass in parameter 1.\r\n" +
                                    $"\r\n" +
                                    $"SetProcessDpiAwareness(PROCESS_SYSTEM_DPI_AWARE) call fails. (0x{hr:X8})",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );
                }
            }
            else if (input == "1" ) // Win7 case
            {
                bool succ = false;
                try
                {
                    succ = DpiUtilities.SetProcessDPIAware();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (succ)
                {
                    MessageBox.Show("You pass in parameter 1.\r\n" +
                                    "\r\n" +
                                    "SetProcessDPIAware() returns success.",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show("You pass in parameter 1.\r\n" +
                                    "\r\n" +
                                    "SetProcessDPIAware() fails. (Requires Win7+)",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );
                }
            }
            else if (input == "2")
            {
                int hr = -1;
                try
                {
                    hr = DpiUtilities.SetProcessDpiAwareness(DpiUtilities.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    MessageBox.Show("You pass in parameter 2.\r\n" +
                                    "\r\n" +
                                    "SetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE) call fails, bcz it requires Win81+.",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                    return;
                }

                if (hr == S_OK)
                {
                    MessageBox.Show("You pass in parameter 2.\r\n" +
                                    "\r\n" +
                                    "SetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE) returns success.",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show($"You pass in parameter 2.\r\n" +
                                    $"\r\n" +
                                    $"SetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE) call fails. (0x{hr:X8})",
                        mbtitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );
                }
            }
            else
            {
                ShowParameterHelp();
                Environment.Exit(1);
            }
        }

        public static void ShowParameterHelp()
        {
            string info = @"Pass a parameter to set DPI awareness for this process.

0 : DPI unware (this action is redundant)
1 : System-DPI aware
2 : Per-monitor-DPI aware

Note: If DPI awareness has been set via manifest, setting it again on Win81+ will fail with 0x80070005.

On Win81+, this program produces best results when run with Per-monitor-DPI aware(option 2).

Win7 does not implement Per-monitor-DPI aware, so you have to go with option 0 or 1.
";
            MessageBox.Show(info, mbtitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }
    }
}
