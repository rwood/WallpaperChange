using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WallpaperChange
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Downloader.GetProperResPictures();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Changer());
        }
    }
}
