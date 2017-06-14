using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WallpaperChange
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                Debugger.Launch();
                MessageBox.Show(((Exception)eventArgs.ExceptionObject).Message);
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SysTrayApplicationContext());
        }
    }
}