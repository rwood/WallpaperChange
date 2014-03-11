using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace WallpaperChange
{
    public class WallpaperChanger
    {
        private TimeSpan _LastTimeSlot = TimeSpan.MinValue;
        private bool _Running = false;
        private Thread _Thread;
        private SortedDictionary<TimeSpan, string> _TimeSlots = new SortedDictionary<TimeSpan, string>();
        private int _TransitionSlices = 5;
        private int _TransitionTime = 10;
        private WallpaperStyle _WallpaperStyle = WallpaperStyle.Centered;

        public void Start()
        {
            LoadConfig();
            _Running = true;
            ThreadStart starter = new ThreadStart(MainLoop);
            _Thread = new Thread(starter);
            _Thread.IsBackground = true;
            _Thread.Name = "Main Loop";
            _Thread.Priority = ThreadPriority.Lowest;
            _Thread.Start();
        }

        public void Stop()
        {
            _Running = false;
            _Thread.Join();
        }

        private static void MatrixBlend(string baseImagePath, string overlayImagePath, string resultpath, float alpha)
        {
            using (Bitmap image1 = (Bitmap)Bitmap.FromFile(baseImagePath))
            {
                using (Bitmap image2 = (Bitmap)Bitmap.FromFile(overlayImagePath))
                {
                    // just change the alpha
                    ColorMatrix matrix = new ColorMatrix(new float[][]{
                new float[] {1F, 0, 0, 0, 0},
                new float[] {0, 1F, 0, 0, 0},
                new float[] {0, 0, 1F, 0, 0},
                new float[] {0, 0, 0, alpha, 0},
                new float[] {0, 0, 0, 0, 1F}});

                    ImageAttributes imageAttributes = new ImageAttributes();
                    imageAttributes.SetColorMatrix(matrix);

                    using (Graphics g = Graphics.FromImage(image1))
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

        private void ChangeWallpaper()
        {
            LoadConfig();
            DateTime now = DateTime.Now;
            TimeSpan n = now - new DateTime(now.Year, now.Month, now.Day);
            TimeSpan timeSlot = TimeSpan.MinValue;
            foreach (TimeSpan key in _TimeSlots.Keys)
            {
                if (key <= n)
                    timeSlot = key;
                else
                    break;
            }
            if (timeSlot == TimeSpan.MinValue)
                return;
            if (_LastTimeSlot != timeSlot)
            {
                _LastTimeSlot = timeSlot;
                FileInfo newWp = new FileInfo(_TimeSlots[timeSlot]);
                FileInfo currentWp = new FileInfo(Path.Combine(Path.GetTempPath(), "wallpaper.bmp"));
                /*if (!newWp.Exists)
                    return;*/
                if (currentWp.Exists)
                {
                    DoTransition(newWp, currentWp);
                }
                else
                    Win32Wallpaper.Set(newWp, _WallpaperStyle);
            }

        }

        private void DoTransition(FileInfo newWallpaper, FileInfo currentWp)
        {
            if (currentWp.Exists)
            {
                FileInfo resultWp = new FileInfo(Path.Combine(Path.GetTempPath(), "transform.bmp"));
                currentWp = currentWp.CopyTo(Path.Combine(Path.GetTempPath(), "original.bmp"), true);
                currentWp.Refresh();
                float alphaIncrement = 1.0f / _TransitionSlices;
                int timeIncrement = Convert.ToInt32(_TransitionTime / _TransitionSlices);

                for (float i = .0f; i <= 1f; i += alphaIncrement)
                {
                    if (!_Running)
                        break;
                    MatrixBlend(currentWp.FullName, newWallpaper.FullName, resultWp.FullName, i);
                    resultWp.Refresh();
                    Win32Wallpaper.Set(resultWp, _WallpaperStyle);
                    resultWp.Delete();
                    resultWp.Refresh();
                    Thread.Sleep(timeIncrement);
                }
                resultWp = newWallpaper.CopyTo(Path.Combine(Path.GetTempPath(), "wallpaper.bmp"), true);
                resultWp.Refresh();
                Win32Wallpaper.Set(resultWp, _WallpaperStyle);
                resultWp.Refresh();
            }
            //if (newWp.Exists)
            //    Wallpaper.Set(newWp, Wallpaper.Style.Centered);
        }

        private void LoadConfig()
        {
            if (_TimeSlots == null)
                _TimeSlots = new SortedDictionary<TimeSpan, string>();
            else
                _TimeSlots.Clear();
            Regex time = new Regex("(?<hour>\\d?\\d):(?<min>\\d\\d)");
            foreach (string item in ConfigurationManager.AppSettings.Keys)
            {
                Match m = time.Match(item);
                string value = ConfigurationManager.AppSettings[item];
                if (string.IsNullOrEmpty(value))
                    continue;
                if (m.Success)
                {
                    int hour = Convert.ToInt32(m.Groups["hour"].Value);
                    int min = Convert.ToInt32(m.Groups["min"].Value);
                    TimeSpan ts = new TimeSpan(hour, min, 0);
                    _TimeSlots.Add(ts, value);
                }
                else if (item == "transition_slices")
                {
                    _TransitionSlices = Convert.ToInt32(value);
                }
                else if (item == "transition_time_ms")
                {
                    _TransitionTime = Convert.ToInt32(value);
                }
                else if (item == "style")
                {
                    try
                    {
                        _WallpaperStyle = (WallpaperStyle)Enum.Parse(typeof(WallpaperStyle), value);
                    }
                    catch
                    {
                        _WallpaperStyle = WallpaperStyle.Centered;
                    }
                }
            }
        }

        private void MainLoop()
        {
            DateTime lastRan = DateTime.MinValue;
            while (_Running)
            {
                if (DateTime.Now - lastRan > TimeSpan.FromMinutes(1))
                {
                    lastRan = DateTime.Now;
                    ChangeWallpaper();
                }
                Thread.Sleep(1000);
            }
        }
    }
}