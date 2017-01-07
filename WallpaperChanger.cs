using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using WallpaperChange.Settings;

namespace WallpaperChange
{
    public class WallpaperChanger
    {
        private readonly UserSettings _userSettings;

        public WallpaperChanger()
        {
            _userSettings = UserSettings.Load();
        }

        private static void MatrixBlend(string baseImagePath, string overlayImagePath, string resultpath, float alpha)
        {
            using (var image1 = (Bitmap) Image.FromFile(baseImagePath))
            {
                using (var image2 = (Bitmap) Image.FromFile(overlayImagePath))
                {
                    // just change the alpha
                    var matrix = new ColorMatrix(new[]
                    {
                        new[] {1F, 0, 0, 0, 0},
                        new[] {0, 1F, 0, 0, 0},
                        new[] {0, 0, 1F, 0, 0},
                        new[] {0, 0, 0, alpha, 0},
                        new[] {0, 0, 0, 0, 1F}
                    });

                    var imageAttributes = new ImageAttributes();
                    imageAttributes.SetColorMatrix(matrix);

                    using (var g = Graphics.FromImage(image1))
                    {
                        g.CompositingMode = CompositingMode.SourceOver;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(image2,
                            new Rectangle(0, 0, image1.Width, image1.Height),
                            0,
                            0,
                            image2.Width,
                            image2.Height,
                            GraphicsUnit.Pixel,
                            imageAttributes);

                        image1.Save(resultpath, ImageFormat.Bmp);
                    }
                }
            }
        }

        public FileAtTime Start(FileAtTime currentFileAtTime)
        {
            var now = DateTime.Now;
            _userSettings.FileTimes.Sort();
            var shouldBeFileAtTime = _userSettings.FileTimes.LastOrDefault(f => f.GetTimeOfDayDateTime() <= now);
            if (shouldBeFileAtTime != null && currentFileAtTime == null)
            {
                Win32Wallpaper.Set(shouldBeFileAtTime.GetWallpaperFileInfo(), _userSettings.WallpaperStyle);
                return shouldBeFileAtTime;
            }
            if (shouldBeFileAtTime == null && currentFileAtTime != null)
            {
                Win32Wallpaper.Set(currentFileAtTime.GetWallpaperFileInfo(), _userSettings.WallpaperStyle);
                return currentFileAtTime;
            }
            if (shouldBeFileAtTime == null ||
                (currentFileAtTime.TimeOfDay == shouldBeFileAtTime.TimeOfDay &&
                 currentFileAtTime.WallpaperPath == shouldBeFileAtTime.WallpaperPath))
            {
                return shouldBeFileAtTime;
            }
            DoTransition(shouldBeFileAtTime.GetWallpaperFileInfo(), currentFileAtTime.GetWallpaperFileInfo());
            return shouldBeFileAtTime;
        }

        private void DoTransition(FileInfo newWallpaper, FileInfo currentWallpaper)
        {
            if (!newWallpaper.Exists || !currentWallpaper.Exists)
            {
                return;
            }
            var tranformWallpaper = new FileInfo(Path.Combine(Path.GetTempPath(), "transform.bmp"));
            try
            {
                if (tranformWallpaper.Exists)
                {
                    tranformWallpaper.Delete();
                    tranformWallpaper.Refresh();
                }
                var alphaIncrement = 1.0f/_userSettings.GetTransitionSlices();
                var timeIncrement =
                    Convert.ToInt32(_userSettings.GetTransitionTimeMilliseconds()/_userSettings.GetTransitionSlices());

                for (var i = .0f; i <= 1f; i += alphaIncrement)
                {
                    MatrixBlend(currentWallpaper.FullName, newWallpaper.FullName, tranformWallpaper.FullName, i);
                    tranformWallpaper.Refresh();
                    Win32Wallpaper.Set(tranformWallpaper, _userSettings.WallpaperStyle);
                    tranformWallpaper.Delete();
                    tranformWallpaper.Refresh();
                    Thread.Sleep(timeIncrement);
                }
                Win32Wallpaper.Set(newWallpaper, _userSettings.WallpaperStyle);
            }
            finally
            {
                if (tranformWallpaper.Exists) tranformWallpaper.Delete();
                Win32Wallpaper.Set(newWallpaper, _userSettings.WallpaperStyle);
            }
        }
    }
}