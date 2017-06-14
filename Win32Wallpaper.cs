using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
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

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}