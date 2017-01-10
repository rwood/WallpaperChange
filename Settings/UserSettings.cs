using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace WallpaperChange.Settings
{
    public class FileAtTime : IComparable<FileAtTime>
    {
        public string TimeOfDay { get; set; }
        public string WallpaperPath { get; set; }

        public int CompareTo(FileAtTime obj)
        {
            return GetTimeOfDayDateTime().CompareTo(obj.GetTimeOfDayDateTime());
        }

        public DateTime GetTimeOfDayDateTime()
        {
            DateTime dt;
            return DateTime.TryParse(TimeOfDay, out dt) ? dt : DateTime.MinValue;
        }

        public FileInfo GetWallpaperFileInfo()
        {
            return new FileInfo(WallpaperPath);
        }
    }

    public class UserSettings
    {
        private const string SettingsFilename = "settings.json";

        public string TransitionSlices { get; set; }
        public string TransitionTimeMilliseconds { get; set; }
        public WallpaperStyle WallpaperStyle { get; set; }
        public List<FileAtTime> FileTimes { get; set; }

        public bool StartApplicationWithWindows { get; set; }

        public int GetTransitionSlices()
        {
            int val;
            return int.TryParse(TransitionSlices, out val) && val > 0 ? val : 10;
        }

        public int GetTransitionTimeMilliseconds()
        {
            int val;
            return int.TryParse(TransitionTimeMilliseconds, out val) && val > 0 ? val : 5000;
        }

        public void Save()
        {
            File.WriteAllText(GetSettingsFile().FullName, (new JavaScriptSerializer()).Serialize(this));
        }

        private static FileInfo GetSettingsFile()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"WallpaperChange",
                SettingsFilename);
            var file = new FileInfo(filePath);
            if (file.Directory == null)
            {
                return new FileInfo(SettingsFilename);
            }
            return file;
        }

        public static UserSettings Load()
        {
            var settingsFile = GetSettingsFile();
            if (settingsFile.Exists)
            {
                return (new JavaScriptSerializer()).Deserialize<UserSettings>(File.ReadAllText(settingsFile.FullName));
            }
            return GetDefaultSettings();
        }

        private static UserSettings GetDefaultSettings()
        {
            return new UserSettings
            {
                WallpaperStyle = WallpaperStyle.Fill,
                TransitionSlices = "10",
                TransitionTimeMilliseconds = "5000",
                StartApplicationWithWindows = true,
                FileTimes = new List<FileAtTime>
                {
                    new FileAtTime {TimeOfDay = "12:00 AM", WallpaperPath = "BitDay\\08-Late-Night.png"},
                    new FileAtTime {TimeOfDay = "05:00 AM", WallpaperPath = "BitDay\\01-Morning.png"},
                    new FileAtTime {TimeOfDay = "10:00 AM", WallpaperPath = "BitDay\\02-Late-Morning.png"},
                    new FileAtTime {TimeOfDay = "12:00 PM", WallpaperPath = "BitDay\\03-Afternoon.png"},
                    new FileAtTime {TimeOfDay = "04:00 PM", WallpaperPath = "BitDay\\04-Late-Afternoon.png"},
                    new FileAtTime {TimeOfDay = "06:00 PM", WallpaperPath = "BitDay\\05-Evening.png"},
                    new FileAtTime {TimeOfDay = "08:00 PM", WallpaperPath = "BitDay\\06-Late-Evening.png"},
                    new FileAtTime {TimeOfDay = "10:00 PM", WallpaperPath = "BitDay\\07-Night.png"}
                }
            };
        }

        public void HandleStartupShortcut()
        {
            var startMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs",
                "WallpaperChange", "WallpaperChange.lnk");
            var appShortcut = new FileInfo(startMenuPath);
            var startupMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "WallpaperChange.lnk");
            var appStartupShortcut = new FileInfo(startupMenuPath);
            if (StartApplicationWithWindows && appShortcut.Exists)
            {
                appShortcut.CopyTo(appStartupShortcut.FullName, true);
                return;
            }
            if (appStartupShortcut.Exists) appStartupShortcut.Delete();
        }
    }
}