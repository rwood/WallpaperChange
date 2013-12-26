using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace WallpaperChange
{
    public class WallpaperChanger
    {
        private readonly Dictionary<int, string> _TimeSlots = new Dictionary<int, string>() {
            { 0, "08-Late-Night.png"},
            { 5, "01-Morning.png"},
            { 10, "02-Late-Morning.png"},
            { 12, "03-Afternoon.png"},
            { 16, "04-Late-Afternoon.png"},
            { 18, "05-Evening.png"},
            { 20, "06-Late-Evening.png"},
            { 22, "07-Night.png"}};
        Thread _Thread;
        bool _Running = false;
        int _LastTimeSlot = -1;

        public void Start()
        {
            _Running = true;
            ThreadStart starter = new ThreadStart(MainLoop);
            _Thread = new Thread(starter);
            _Thread.IsBackground = true;
            _Thread.Name = "Main Loop";
            _Thread.Priority = ThreadPriority.Lowest;
            _Thread.Start();
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
            int timeSlot = 0;
            foreach (int key in _TimeSlots.Keys)
            {
                if (key <= now.Hour)
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
