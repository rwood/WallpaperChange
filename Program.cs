using System;
using System.Diagnostics;
using System.IO;
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
                var ex = (Exception) eventArgs.ExceptionObject;
                MessageBox.Show(ex.Message);
                File.WriteAllText($"Error_{Guid.NewGuid()}.txt", $"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SysTrayApplicationContext());
        }
    }
}