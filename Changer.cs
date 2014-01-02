using System;
using System.Windows.Forms;

namespace WallpaperChange
{
    public partial class Changer : Form
    {
        private WallpaperChanger _Changer = new WallpaperChanger();

        public Changer()
        {
            InitializeComponent();
            btnStartAutomatically.Checked = Startup.StartupEnabled;
            _Changer.Start();
        }

        private void btnStartAutomatically_Click(object sender, EventArgs e)
        {
            if (Startup.StartupEnabled)
                Startup.RemoveStartup();
            else
                Startup.AddStartup();
            btnStartAutomatically.Checked = Startup.StartupEnabled;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _Changer.Stop();
            Application.Exit();
        }
    }
}