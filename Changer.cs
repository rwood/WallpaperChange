using System;
using System.Windows.Forms;

namespace WallpaperChange
{
    public partial class Changer : Form
    {
        private WallpaperChanger _Changer = new WallpaperChanger();

        public Changer()
        {
            Startup.RemoveStartup();
            InitializeComponent();
            _Changer.Start();
            btnStartAutomatically.Checked = Startup.StartupFolderShortcutExists();
        }

        private void btnStartAutomatically_Click(object sender, EventArgs e)
        {
            bool exists = Startup.StartupFolderShortcutExists();
            if (btnStartAutomatically.Checked && !exists)
                Startup.CreateStartupFolderShortcut();
            else if (!btnStartAutomatically.Checked && exists)
                Startup.DeleteStartupFolderShortcuts();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _Changer.Stop();
            Application.Exit();
        }
    }
}