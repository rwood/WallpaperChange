using System;
using System.IO;
using System.Windows.Forms;

namespace WallpaperChange.Settings
{
    public partial class FileAndTimeControl : UserControl
    {
        public string TimeOfDay
        {
            get { return dtpTimeOfDay.Text; }
            set { dtpTimeOfDay.Text = value; }
        }

        public string WallpaperPath
        {
            get { return txtFilePath.Text; }
            set
            {
                txtFilePath.Text = value;

                if (!string.IsNullOrEmpty(txtFilePath.Text))
                {
                    var directoryInfo = new FileInfo(txtFilePath.Text).Directory;
                    if (directoryInfo != null)
                    {
                        ofdBrowse.InitialDirectory = directoryInfo.FullName;
                    }
                }
            }
        }

        public FileAndTimeControl()
        {
            InitializeComponent();
        }

        private void Browse_OnClick(object sender, EventArgs e)
        {
            var result = ofdBrowse.ShowDialog();
            if (result == DialogResult.OK)
            {
                WallpaperPath = ofdBrowse.FileName;
            }
        }

        private void Remove_OnClick(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
        }
    }
}
