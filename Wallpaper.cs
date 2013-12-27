using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WallpaperChange
{
    public static class Wallpaper
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style : int
        {
            Tiled,
            Centered,
            Stretched
        }

        public static void Set(FileInfo file, Style style)
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

                if (style == Style.Stretched)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == Style.Centered)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == Style.Tiled)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }

                SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0,
                    tempPath,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }
    }
}
