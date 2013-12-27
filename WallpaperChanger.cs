using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace WallpaperChange
{
    public class WallpaperChanger
    {
        private SortedDictionary<TimeSpan, string> _TimeSlots = new SortedDictionary<TimeSpan, string>();
        Thread _Thread;
        bool _Running = false;
        TimeSpan _LastTimeSlot = TimeSpan.MinValue;

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

        private void LoadConfig()
        {
            Regex time = new Regex("(?<hour>\\d?\\d):(?<min>\\d\\d)");
            foreach (string item in ConfigurationManager.AppSettings.Keys)
            {
                Match m = time.Match(item);
                if (!m.Success || string.IsNullOrEmpty(ConfigurationManager.AppSettings[item]))
                    continue;
                int hour = Convert.ToInt32(m.Groups["hour"].Value);
                int min = Convert.ToInt32(m.Groups["min"].Value);
                TimeSpan ts = new TimeSpan(hour, min, 0);
                _TimeSlots.Add(ts, ConfigurationManager.AppSettings[item]);
            }
        }

        private void MainLoop()
        {
            DateTime lastRan = DateTime.MinValue;
            while (_Running)
            {
                if (DateTime.Now - lastRan > TimeSpan.FromMinutes(15))
                {
                    lastRan = DateTime.Now;
                    ChangeWallpaper();
                }
                Thread.Sleep(10000);
            }
        }

        void ChangeWallpaper()
        {
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
            if (_LastTimeSlot != timeSlot)
            {
                _LastTimeSlot = timeSlot;
                FileInfo file = new FileInfo(_TimeSlots[timeSlot]);
                if (file.Exists)
                    Wallpaper.Set(file, Wallpaper.Style.Centered);
            }
        }


        public void Stop()
        {
            _Running = false;
            _Thread.Join();
        }
    }

}
