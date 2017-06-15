using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;

namespace WallpaperChange
{
    public static class Win32Wallpaper
    {
        private const string WallpaperstyleKey = "WallpaperStyle";
        private const string TileWallpaperKey = "TileWallpaper";
        private const int SpiSetdeskwallpaper = 20;
        private const int SpifSendwininichange = 0x02;
        private const int SpifUpdateinifile = 0x01;

        public static void Set(FileInfo file, WallpaperStyle style)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (key == null)
                return;
            
            var tempPath = file.FullName;
            if (!file.Extension.Equals(".bmp", StringComparison.InvariantCultureIgnoreCase))
            {
                tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
                var img = Image.FromFile(file.FullName);
                img.Save(tempPath, ImageFormat.Bmp);
            }

            switch (style)
            {
                case WallpaperStyle.Stretched:
                    key.SetValue(WallpaperstyleKey, 2.ToString());
                    key.SetValue(TileWallpaperKey, 0.ToString());
                    break;
                case WallpaperStyle.Centered:
                    key.SetValue(WallpaperstyleKey, 1.ToString());
                    key.SetValue(TileWallpaperKey, 0.ToString());
                    break;
                case WallpaperStyle.Tiled:
                    key.SetValue(WallpaperstyleKey, 1.ToString());
                    key.SetValue(TileWallpaperKey, 1.ToString());
                    break;
                case WallpaperStyle.Fill:
                    key.SetValue(WallpaperstyleKey, 10.ToString());
                    key.SetValue(TileWallpaperKey, 0.ToString());
                    break;
            }

            SystemParametersInfo(SpiSetdeskwallpaper,
                0,
                tempPath,
                SpifUpdateinifile | SpifSendwininichange);
        }

        /// <summary>
        /// Creates a shortcut at the specified path with the given target and
        /// arguments.
        /// </summary>
        /// <param name="path">The path where the shortcut will be created. This should
        ///     be a file with the LNK extension.</param>
        /// <param name="target">The target of the shortcut, e.g. the program or file
        ///     or folder which will be opened.</param>
        /// <param name="arguments">The additional command line arguments passed to the
        ///     target.</param>
        /// <param name="description"></param>
        public static void CreateShortcut(string path, string target, string arguments="", string description="")
        {
            var link = (IShellLink)new ShellLink();
            
            link.SetDescription(description);
            link.SetPath(target);
            link.SetArguments(arguments);

            //save it
            var file = (IPersistFile)link;
            file.Save(path, false);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
    }
}