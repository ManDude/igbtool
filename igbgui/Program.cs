using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace igbgui
{
  static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            FreeConsole();
        }
    }
}
