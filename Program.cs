using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace WallpaperChange
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program p = new Program();
            p.Start();
            Application.Run();
            //Application.Run(new Changer());
        }

        private WallpaperChanger _Changer = new WallpaperChanger();
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnStop;
        //private System.Windows.Forms.ToolStripMenuItem btnStartAutomatically;
        private System.ComponentModel.Container components;

        public void Start()
        {
            Startup.RemoveStartup();
            InitializeComponent();
            _Changer.Start();
            //btnStartAutomatically.Checked = Startup.StartupFolderShortcutExists();
        }

        //private void btnStartAutomatically_Click(object sender, EventArgs e)
        //{
        //    bool exists = Startup.StartupFolderShortcutExists();
        //    if (btnStartAutomatically.Checked && !exists)
        //        Startup.CreateStartupFolderShortcut();
        //    else if (!btnStartAutomatically.Checked && exists)
        //        Startup.DeleteStartupFolderShortcuts();
        //}

        private void btnStop_Click(object sender, EventArgs e)
        {
            _Changer.Stop();
            Application.Exit();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Program));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnStop = new System.Windows.Forms.ToolStripMenuItem();
            //this.btnStartAutomatically = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            Stream imgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WallpaperChange.favicon.ico");
            Icon ico = new Icon(imgStream);
            this.notifyIcon1.Icon = ico;//((System.Drawing.Icon)(resources.GetObject("favicon.ico")));
            this.notifyIcon1.Tag = "WallpaperChanger";
            this.notifyIcon1.Text = "WallpaperChanger";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.btnStop });
            //this.btnStartAutomatically,
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(176, 76);
            // 
            // toolStripMenuItem1
            // 
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(175, 22);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStartAutomatically
            // 
            //this.btnStartAutomatically.CheckOnClick = true;
            //this.btnStartAutomatically.Name = "btnStartAutomatically";
            //this.btnStartAutomatically.Size = new System.Drawing.Size(175, 22);
            //this.btnStartAutomatically.Text = "Start Automatically";
            //this.btnStartAutomatically.Click += new System.EventHandler(this.btnStartAutomatically_Click);
            // 
            // Changer
            // 
            this.contextMenuStrip1.ResumeLayout(false);
        }
    }
}