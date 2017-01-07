namespace WallpaperChange.Settings
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numTransitionSlices = new System.Windows.Forms.NumericUpDown();
            this.numTransitionTime = new System.Windows.Forms.NumericUpDown();
            this.cmbWallpaperStyle = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlFileTimes = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddFileAndTime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numTransitionSlices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransitionTime)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Transition Slices:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Transition Time (milliseconds):";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(455, 406);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(536, 406);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numTransitionSlices
            // 
            this.numTransitionSlices.Location = new System.Drawing.Point(165, 33);
            this.numTransitionSlices.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTransitionSlices.Name = "numTransitionSlices";
            this.numTransitionSlices.Size = new System.Drawing.Size(120, 20);
            this.numTransitionSlices.TabIndex = 5;
            this.numTransitionSlices.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numTransitionTime
            // 
            this.numTransitionTime.Location = new System.Drawing.Point(165, 7);
            this.numTransitionTime.Maximum = new decimal(new int[] {
            120000,
            0,
            0,
            0});
            this.numTransitionTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTransitionTime.Name = "numTransitionTime";
            this.numTransitionTime.Size = new System.Drawing.Size(120, 20);
            this.numTransitionTime.TabIndex = 6;
            this.numTransitionTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbWallpaperStyle
            // 
            this.cmbWallpaperStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWallpaperStyle.FormattingEnabled = true;
            this.cmbWallpaperStyle.Location = new System.Drawing.Point(165, 59);
            this.cmbWallpaperStyle.Name = "cmbWallpaperStyle";
            this.cmbWallpaperStyle.Size = new System.Drawing.Size(121, 21);
            this.cmbWallpaperStyle.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Wallpaper Style:";
            // 
            // pnlFileTimes
            // 
            this.pnlFileTimes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFileTimes.AutoScroll = true;
            this.pnlFileTimes.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFileTimes.Location = new System.Drawing.Point(12, 86);
            this.pnlFileTimes.Name = "pnlFileTimes";
            this.pnlFileTimes.Size = new System.Drawing.Size(599, 314);
            this.pnlFileTimes.TabIndex = 10;
            this.pnlFileTimes.WrapContents = false;
            // 
            // btnAddFileAndTime
            // 
            this.btnAddFileAndTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddFileAndTime.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFileAndTime.Image")));
            this.btnAddFileAndTime.Location = new System.Drawing.Point(12, 406);
            this.btnAddFileAndTime.Name = "btnAddFileAndTime";
            this.btnAddFileAndTime.Size = new System.Drawing.Size(23, 23);
            this.btnAddFileAndTime.TabIndex = 11;
            this.btnAddFileAndTime.UseVisualStyleBackColor = true;
            this.btnAddFileAndTime.Click += new System.EventHandler(this.btnAddFileAndTime_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(623, 441);
            this.Controls.Add(this.btnAddFileAndTime);
            this.Controls.Add(this.pnlFileTimes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbWallpaperStyle);
            this.Controls.Add(this.numTransitionTime);
            this.Controls.Add(this.numTransitionSlices);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "WallpaperChange Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numTransitionSlices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransitionTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numTransitionSlices;
        private System.Windows.Forms.NumericUpDown numTransitionTime;
        private System.Windows.Forms.ComboBox cmbWallpaperStyle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel pnlFileTimes;
        private System.Windows.Forms.Button btnAddFileAndTime;
    }
}