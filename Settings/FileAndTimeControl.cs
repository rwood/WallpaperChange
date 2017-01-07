using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WallpaperChange.Settings
{
    public partial class FileAndTimeControl : UserControl
    {
        private readonly FileAtTime _fileAtTime;
        private readonly UserSettings _userSettings;
        public FileAndTimeControl(FileAtTime fat, UserSettings userSettings)
        {
            InitializeComponent();
            _fileAtTime = fat;
            _userSettings = userSettings;
            dtpTimeOfDay.Text = fat.TimeOfDay;
            txtFilePath.Text = fat.WallpaperPath;
            if (!string.IsNullOrEmpty(txtFilePath.Text))
            {
                var directoryInfo = new FileInfo(txtFilePath.Text).Directory;
                if (directoryInfo != null)
                {
                    ofdBrowse.InitialDirectory = directoryInfo.FullName;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var result = ofdBrowse.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFilePath.Text = ofdBrowse.FileName;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _userSettings.FileTimes.Remove(_fileAtTime);
            Parent.Controls.Remove(this);
        }

        public void SaveValues()
        {
            if (!File.Exists(txtFilePath.Text))
            {
                _userSettings.FileTimes.Remove(_fileAtTime);
            }
            else
            {
                _fileAtTime.TimeOfDay = dtpTimeOfDay.Text;
                _fileAtTime.WallpaperPath = txtFilePath.Text;
            }
        }
    }
}
