using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperChange
{
    public static class Startup
    {
        static readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        static readonly string TITLE = "WallpaperChanger";

        public static void AddStartup()
        {
            rkApp.SetValue(TITLE, Assembly.GetExecutingAssembly().CodeBase);
        }

        public static void RemoveStartup()
        {
            rkApp.DeleteValue(TITLE, false);
        }

        public static bool StartupEnabled
        {
            get
            {
                return rkApp.GetValue(TITLE) != null;
            }
        }
    }
}
