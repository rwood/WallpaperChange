using Microsoft.Win32;
using System;
using System.Reflection;
//using IWshRuntimeLibrary;
//using Shell32;
using System.Windows.Forms;
using System.IO;

namespace WallpaperChange
{
    public static class Startup
    {
        [Obsolete]
        private static readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        [Obsolete]
        private static readonly string TITLE = "WallpaperChanger";

        [Obsolete]
        public static bool StartupEnabled { get { return rkApp.GetValue(TITLE) != null; } }

        [Obsolete]
        public static void AddStartup()
        {
            Uri executingAsm = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            rkApp.SetValue(TITLE, "\"" + executingAsm.LocalPath + "\"");
        }

        [Obsolete]
        public static void RemoveStartup()
        {
            rkApp.DeleteValue(TITLE, false);
        }

        //public static void CreateStartupFolderShortcut()
        //{
        //    WshShellClass wshShell = new WshShellClass();
        //    IWshRuntimeLibrary.IWshShortcut shortcut;
        //    string startUpFolderPath =
        //      Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        //    // Create the shortcut
        //    shortcut =
        //      (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
        //        startUpFolderPath + "\\" +
        //        Application.ProductName + ".lnk");

        //    shortcut.TargetPath = Application.ExecutablePath;
        //    shortcut.WorkingDirectory = Application.StartupPath;
        //    shortcut.Description = "BitDay Wallpaper Changer";
        //    shortcut.IconLocation = Application.StartupPath + @"\favicon.ico";
        //    shortcut.Save();
        //}

        //public static string GetShortcutTargetFile(string shortcutFilename)
        //{
        //    string pathOnly = Path.GetDirectoryName(shortcutFilename);
        //    string filenameOnly = Path.GetFileName(shortcutFilename);

        //    Shell32.Shell shell = new Shell32.ShellClass();
        //    Shell32.Folder folder = shell.NameSpace(pathOnly);
        //    Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
        //    if (folderItem != null)
        //    {
        //        Shell32.ShellLinkObject link =
        //          (Shell32.ShellLinkObject)folderItem.GetLink;
        //        return link.Path;
        //    }

        //    return String.Empty; // Not found
        //}

        //public static void DeleteStartupFolderShortcuts()
        //{
        //    string startUpFolderPath =
        //      Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        //    DirectoryInfo di = new DirectoryInfo(startUpFolderPath);
        //    FileInfo[] files = di.GetFiles("*.lnk");

        //    foreach (FileInfo fi in files)
        //    {
        //        string shortcutTargetFile = GetShortcutTargetFile(fi.FullName);

        //        if (shortcutTargetFile.EndsWith(Application.ExecutablePath,
        //              StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            System.IO.File.Delete(fi.FullName);
        //        }
        //    }
        //}

        //public static bool StartupFolderShortcutExists()
        //{
        //    string startUpFolderPath =
        //      Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        //    DirectoryInfo di = new DirectoryInfo(startUpFolderPath);
        //    FileInfo[] files = di.GetFiles("*.lnk");

        //    foreach (FileInfo fi in files)
        //    {
        //        string shortcutTargetFile = GetShortcutTargetFile(fi.FullName);

        //        if (shortcutTargetFile.EndsWith(Application.ExecutablePath,
        //              StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

    }
}