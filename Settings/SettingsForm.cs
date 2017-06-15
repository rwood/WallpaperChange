using System;
using System.Linq;
using System.Windows.Forms;

namespace WallpaperChange.Settings
{
    public partial class SettingsForm : Form
    {
        private static SettingsForm _instance;

        private readonly UserSettings _userSettings;

        public SettingsForm()
        {
            InitializeComponent();
            _userSettings = UserSettings.Load();
            numTransitionSlices.Text = _userSettings.TransitionSlices.ToString();
            numTransitionTime.Text = _userSettings.TransitionTimeMilliseconds.ToString();
            cmbWallpaperStyle.DataSource = Enum.GetValues(typeof (WallpaperStyle));
            cmbWallpaperStyle.SelectedItem = _userSettings.WallpaperStyle;
            foreach (var fileAtTime in _userSettings.FileTimes)
            {
                pnlFileTimes.Controls.Add(new FileAndTimeControl
                {
                    TimeOfDay = fileAtTime.TimeOfDay,
                    WallpaperPath = fileAtTime.WallpaperPath,
                });
            }
            chkStartWithWindows.Checked = _userSettings.StartApplicationWithWindows;
        }

        public static SettingsForm GetInstance()
        {
            if (_instance != null) return _instance;
            _instance = new SettingsForm();
            _instance.FormClosed += (sender, args) => _instance = null;
            return _instance;
        }

        private void Save_OnClick(object sender, EventArgs e)
        {
            _userSettings.FileTimes = pnlFileTimes.Controls.Cast<FileAndTimeControl>()
                                                  .Select(c => new FileAtTime{ TimeOfDay = c.TimeOfDay, WallpaperPath = c.WallpaperPath})
                                                  .OrderBy(f => f)
                                                  .ToList();

            int transitionSlices;
            if(int.TryParse(numTransitionSlices.Text, out transitionSlices))
                _userSettings.TransitionSlices = transitionSlices;
            int transitionTimeMilliseconds;
            if (int.TryParse(numTransitionTime.Text, out transitionTimeMilliseconds))
                _userSettings.TransitionTimeMilliseconds = transitionTimeMilliseconds;

            _userSettings.WallpaperStyle = (WallpaperStyle) cmbWallpaperStyle.SelectedItem;
            _userSettings.StartApplicationWithWindows = chkStartWithWindows.Checked;
            _userSettings.HandleStartupShortcut();
            _userSettings.Save();
            Close();
        }

        private void Cancel_OnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void AddFileAndTime_OnClick(object sender, EventArgs e)
        {
            pnlFileTimes.Controls.Add(new FileAndTimeControl());
        }
    }
}