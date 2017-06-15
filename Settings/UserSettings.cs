using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;
using File = System.IO.File;

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

        public int TransitionSlices { get; set; } = 10;
        public int TransitionTimeMilliseconds { get; set; } = 5000;
        public WallpaperStyle WallpaperStyle { get; set; } = WallpaperStyle.Fill;
        public List<FileAtTime> FileTimes { get; set; }

        public bool StartApplicationWithWindows { get; set; } = true;
        
        public void Save()
        {
            File.WriteAllText(GetSettingsFile().FullName, new JavaScriptSerializer().Serialize(this));
        }

        private static FileInfo GetSettingsFile()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WallpaperChange",
                SettingsFilename);
            var file = new FileInfo(filePath);
            if (file.Directory == null || !file.Directory.Exists)
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
                return new JavaScriptSerializer().Deserialize<UserSettings>(File.ReadAllText(settingsFile.FullName));
            }
            return GetDefaultSettings();
        }

        private static UserSettings GetDefaultSettings()
        {
            return new UserSettings
            {
                FileTimes = new List<FileAtTime>
                {
                    new FileAtTime {TimeOfDay = "0:01", WallpaperPath = "BitDay\\11-Mid-Night.png"},
                    new FileAtTime {TimeOfDay = "2:00", WallpaperPath = "BitDay\\12-Late-Night.png"},
                    new FileAtTime {TimeOfDay = "5:00", WallpaperPath = "BitDay\\01-Early-Morning.png"},
                    new FileAtTime {TimeOfDay = "10:00", WallpaperPath = "BitDay\\02-Mid-Morning.png"},
                    new FileAtTime {TimeOfDay = "11:00", WallpaperPath = "BitDay\\03-Late-Morning.png"},
                    new FileAtTime {TimeOfDay = "12:00", WallpaperPath = "BitDay\\04-Early-Afternoon.png"},
                    new FileAtTime {TimeOfDay = "13:00", WallpaperPath = "BitDay\\05-Mid-Afternoon.png"},
                    new FileAtTime {TimeOfDay = "15:00", WallpaperPath = "BitDay\\06-Late-Afternoon.png"},
                    new FileAtTime {TimeOfDay = "16:00", WallpaperPath = "BitDay\\07-Early-Evening.png"},
                    new FileAtTime {TimeOfDay = "18:00", WallpaperPath = "BitDay\\08-Mid-Evening.png"},
                    new FileAtTime {TimeOfDay = "20:00", WallpaperPath = "BitDay\\09-Late-Evening.png"},
                    new FileAtTime {TimeOfDay = "22:00", WallpaperPath = "BitDay\\10-Early-Night.png"},
                }
            };
        }

        public void HandleStartupShortcut()
        {
            var exeLocation = Assembly.GetEntryAssembly().Location;
            var startupMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "WallpaperChange.lnk");
            
            if (StartApplicationWithWindows)
            {
                Win32Wallpaper.CreateShortcut(startupMenuPath, exeLocation, description:"WallpaperChange Autostart");
            }
            else if(File.Exists(startupMenuPath))
            {
                File.Delete(startupMenuPath);
            }
        }
    }
}