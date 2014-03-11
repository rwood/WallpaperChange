using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace WallpaperChange
{
    public enum WallpaperStyle : int
    {
        Tiled,
        Centered,
        Stretched,
        Fill
    }

    public static class Win32Wallpaper
    {
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_SENDWININICHANGE = 0x02;
        private const int SPIF_UPDATEINIFILE = 0x01;
        public static void Set(FileInfo file, WallpaperStyle style)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            using (System.IO.Stream s = file.OpenRead())
            {
                string tempPath = file.FullName;
                if (!file.Name.Equals("wallpaper.bmp"))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(s);
                    tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
                    img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);
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

                SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0,
                    tempPath,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}