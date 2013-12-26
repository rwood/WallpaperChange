using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperChange
{
    class Program
    {
        private static readonly Dictionary<int, string> times = new Dictionary<int, string>() {
            { 0, "08-Late-Night.png"},
            { 5, "01-Morning.png"},
            { 10, "02-Late-Morning.png"},
            { 12, "03-Afternoon.png"},
            { 16, "04-Late-Afternoon.png"},
            { 18, "05-Evening.png"},
            { 20, "06-Late-Evening.png"},
            { 22, "07-Night.png"}};

        static void Main(string[] args)
        {
            DateTime now = DateTime.Now;
            int timeSlot = 0;
            foreach (int key in times.Keys)
            {
                if (key < now.Hour)
                    timeSlot = key;
                else
                    break;
            }
            FileInfo file = new FileInfo(times[timeSlot]);
            if(file.Exists)
                Wallpaper.Set(file, Wallpaper.Style.Centered);
        }
    }


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
            System.IO.Stream s = new System.Net.WebClient().OpenRead(file.FullName);

            System.Drawing.Image img = System.Drawing.Image.FromStream(s);
            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
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
