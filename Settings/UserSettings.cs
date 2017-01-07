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
            File.WriteAllText(SettingsFilename, (new JavaScriptSerializer()).Serialize(this));
        }

        public static UserSettings Load()
        {
            if (File.Exists(SettingsFilename))
            {
                return (new JavaScriptSerializer()).Deserialize<UserSettings>(File.ReadAllText(SettingsFilename));
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
    }
}