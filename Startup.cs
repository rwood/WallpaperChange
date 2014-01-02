using Microsoft.Win32;
using System;
using System.Reflection;

namespace WallpaperChange
{
    public static class Startup
    {
        private static readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private static readonly string TITLE = "WallpaperChanger";

        public static bool StartupEnabled { get { return rkApp.GetValue(TITLE) != null; } }

        public static void AddStartup()
        {
            Uri executingAsm = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            rkApp.SetValue(TITLE, "\"" + executingAsm.LocalPath + "\"");
        }

        public static void RemoveStartup()
        {
            rkApp.DeleteValue(TITLE, false);
        }
    }
}