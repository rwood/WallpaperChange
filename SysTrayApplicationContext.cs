using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Squirrel;
using WallpaperChange.Settings;
using Timer = System.Threading.Timer;

namespace WallpaperChange
{
    internal class SysTrayApplicationContext : ApplicationContext
    {
        private const string CheckForApplicationUpdates = "Check for Updates";
        private const string UpdateTheApplication = "Apply Updates";
        private readonly object _syncLock = new object();

        private readonly string[] _updateUrls =
        {
            "http://tamarau.com/WallpaperChange",
            "http://192.168.1.9/WallpaperChange"
        };

        private ToolStripMenuItem _btnAbout;
        private ToolStripMenuItem _btnSettings;
        private ToolStripMenuItem _btnStop;
        private ToolStripMenuItem _btnUpdates;

        private readonly TimeSpan _checkUpdatePeriod = TimeSpan.FromDays(1);
        private readonly TimeSpan _checkWallpaperPeriod = TimeSpan.FromSeconds(10);
        private ContextMenuStrip _contextMenuStrip1;
        private FileAtTime _currentFileAtTime;
        private NotifyIcon _notifyIcon1;
        private Timer _updateTimer;
        private WallpaperChanger _wallpaperChanger;
        private Timer _wallpaperTimer;

        private IContainer components;

        public SysTrayApplicationContext()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            _notifyIcon1 = new NotifyIcon(components);
            _contextMenuStrip1 = new ContextMenuStrip(components);
            _btnAbout = new ToolStripMenuItem();
            _btnStop = new ToolStripMenuItem();
            _btnSettings = new ToolStripMenuItem();
            _btnUpdates = new ToolStripMenuItem();
            _contextMenuStrip1.SuspendLayout();
            _notifyIcon1.ContextMenuStrip = _contextMenuStrip1;
            var imgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WallpaperChange.favicon.ico");
            if (imgStream != null)
            {
                var ico = new Icon(imgStream);
                _notifyIcon1.Icon = ico;
            }
            _notifyIcon1.Tag = "WallpaperChange";
            _notifyIcon1.Text = @"WallpaperChange";
            _notifyIcon1.Visible = true;
            _contextMenuStrip1.Items.AddRange(new ToolStripItem[] {_btnAbout, _btnUpdates, _btnSettings, _btnStop});
            _contextMenuStrip1.Name = "_contextMenuStrip1";
            _contextMenuStrip1.Size = new Size(176, 76);
            _btnAbout.Name = "_btnAbout";
            _btnAbout.AutoSize = true;
            _btnAbout.Text = @"About";
            _btnAbout.Click += About_OnClick;
            _btnStop.Name = "_btnStop";
            _btnStop.AutoSize = true;
            _btnStop.Text = @"Stop";
            _btnStop.Click += Stop_OnClick;
            _btnSettings.Name = "_btnSettings";
            _btnSettings.Text = @"Settings";
            _btnSettings.AutoSize = true;
            _btnSettings.Click += Settings_OnClick;
            _btnUpdates.Name = "_btnUpdates";
            _btnUpdates.Text = CheckForApplicationUpdates;
            _btnUpdates.AutoSize = true;
            _btnUpdates.Click += Updates_OnClick;
            _contextMenuStrip1.ResumeLayout(false);
            _wallpaperTimer = new Timer(WallpaperTimerElapsed, null, _checkWallpaperPeriod, Timeout.InfiniteTimeSpan);
            _updateTimer = new Timer(
                s =>
                {
                    var u = CheckForUpdates();
                    u.Wait();
                },
                null,
                TimeSpan.FromSeconds(30),
                Timeout.InfiniteTimeSpan);
        }

        private async void Updates_OnClick(object sender, EventArgs e)
        {
            var errors = 0;
            _btnUpdates.Enabled = false;
            foreach (var updateUrl in _updateUrls)
            {
                try
                {
                    var btn = (ToolStripMenuItem) sender;
                    if (btn.Text == CheckForApplicationUpdates)
                    {
                        var updates = await CheckForUpdates();
                        if (!updates) MessageBox.Show(@"Your app is up to date.");
                    }
                    if (btn.Text != UpdateTheApplication) return;
                    using (var updateManager = new UpdateManager(updateUrl))
                    {
                        await updateManager.UpdateApp();
                        MessageBox.Show(@"WallpaperChange just got updated. Restarting...", @"Huzzah!");
                    }
                    UpdateManager.RestartApp();
                    break;
                }
                catch (Exception ex)
                {
                    errors++;
                    if (errors > 1) MessageBox.Show($@"There was an error updating the app: {ex.Message}.");
                }
                finally
                {
                    _btnUpdates.Enabled = true;
                }
            }
        }

        private static void About_OnClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void WallpaperTimerElapsed(object sender)
        {
            try
            {
                lock (_syncLock)
                {
                    if (_wallpaperChanger == null)
                        _wallpaperChanger = new WallpaperChanger();
                    else
                        return;
                }
                _currentFileAtTime = _wallpaperChanger.Start(_currentFileAtTime);
                _wallpaperChanger = null;
            }
            finally
            {
                _wallpaperTimer.Change(_checkWallpaperPeriod, Timeout.InfiniteTimeSpan);
            }
        }

        private async Task<bool> CheckForUpdates()
        {
            var errors = 0;
            var updates = false;
            foreach (var url in _updateUrls)
            {
                try
                {
                    using (var updateManager = new UpdateManager(url))
                    {
                        var updateInfo = await updateManager.CheckForUpdate();
                        if (updateInfo == null || !updateInfo.ReleasesToApply.Any())
                        {
                            _btnUpdates.Text = CheckForApplicationUpdates;
                        }
                        else
                        {
                            _btnUpdates.Text = UpdateTheApplication;
                            updates = true;
                        }
                    }
                    UserSettings.Load().HandleStartupShortcut();
                    break;
                }
                catch (Exception ex)
                {
                    errors++;
                    if (errors > 1) MessageBox.Show($@"There was an error checking for updates: {ex.Message}.");
                }
                finally
                {
                    _updateTimer.Change(_checkUpdatePeriod, Timeout.InfiniteTimeSpan);
                }
            }
            return updates;
        }

        private void Stop_OnClick(object sender, EventArgs e)
        {
            _updateTimer.Dispose();
            lock (_syncLock)
            {
                _wallpaperTimer.Dispose();
            }
            Application.Exit();
        }

        private static void Settings_OnClick(object sender, EventArgs e)
        {
            var settingsForm = SettingsForm.GetInstance();
            settingsForm.Show();
            if (!settingsForm.Visible)
                settingsForm.BringToFront();
        }
    }
}