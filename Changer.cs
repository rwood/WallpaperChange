using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WallpaperChange
{
    public partial class Changer : Form
    {
        WallpaperChanger _Changer = new WallpaperChanger();
        public Changer()
        {
            InitializeComponent();
            btnStartAutomatically.Checked = Startup.StartupEnabled;
            _Changer.Start();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _Changer.Stop();
            Application.Exit();
        }

        private void btnStartAutomatically_Click(object sender, EventArgs e)
        {
            if (Startup.StartupEnabled)
                Startup.RemoveStartup();
            else
                Startup.AddStartup();
            btnStartAutomatically.Checked = Startup.StartupEnabled;
        }
    }
}
