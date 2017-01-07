using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WallpaperChange
{
    public static class Win32Wallpaper
    {
        private const int SpiSetdeskwallpaper = 20;
        private const int SpifSendwininichange = 0x02;
        private const int SpifUpdateinifile = 0x01;

        public static void Set(FileInfo file, WallpaperStyle style)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (key == null)
            {
                return;
            }
            using (Stream s = file.OpenRead())
            {
                var tempPath = file.FullName;
                if (!file.Name.Equals("wallpaper.bmp"))
                {
                    var img = Image.FromStream(s);
                    tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
                    img.Save(tempPath, ImageFormat.Bmp);
                }

                if (style == WallpaperStyle.Stretched)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == WallpaperStyle.Centered)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == WallpaperStyle.Tiled)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }

                if (style == WallpaperStyle.Fill)
                {
                    key.SetValue(@"WallpaperStyle", 10.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                SystemParametersInfo(SpiSetdeskwallpaper,
                    0,
                    tempPath,
                    SpifUpdateinifile | SpifSendwininichange);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}