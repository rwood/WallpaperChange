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
            numTransitionSlices.Text = _userSettings.TransitionSlices;
            numTransitionTime.Text = _userSettings.TransitionTimeMilliseconds;
            cmbWallpaperStyle.DataSource = Enum.GetValues(typeof (WallpaperStyle));
            cmbWallpaperStyle.SelectedItem = _userSettings.WallpaperStyle;
            _userSettings.FileTimes.ForEach(u => pnlFileTimes.Controls.Add(new FileAndTimeControl(u, _userSettings)));
            chkStartWithWindows.Checked = _userSettings.StartApplicationWithWindows;
        }

        public static SettingsForm GetInstance()
        {
            if (_instance != null) return _instance;
            _instance = new SettingsForm();
            _instance.FormClosed += (sender, args) => _instance = null;
            return _instance;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var control in pnlFileTimes.Controls.OfType<FileAndTimeControl>())
            {
                (control).SaveValues();
            }
            _userSettings.FileTimes.Sort();
            _userSettings.TransitionSlices = numTransitionSlices.Text;
            _userSettings.TransitionTimeMilliseconds = numTransitionTime.Text;
            _userSettings.WallpaperStyle = (WallpaperStyle) cmbWallpaperStyle.SelectedItem;
            _userSettings.StartApplicationWithWindows = chkStartWithWindows.Checked;
            _userSettings.HandleStartupShortcut();
            _userSettings.Save();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddFileAndTime_Click(object sender, EventArgs e)
        {
            var fat = new FileAtTime();
            _userSettings.FileTimes.Add(fat);
            pnlFileTimes.Controls.Add(new FileAndTimeControl(fat, _userSettings));
        }
    }
}