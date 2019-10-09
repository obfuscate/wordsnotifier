namespace WordsNotifier
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerToShowWindow = new System.Windows.Forms.Timer(this.components);
            this.Settings = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnOpenFie = new System.Windows.Forms.Button();
            this.lblTimeToShowWindow = new System.Windows.Forms.Label();
            this.lblTimeToHideWindow = new System.Windows.Forms.Label();
            this.txtTimeShow = new System.Windows.Forms.TextBox();
            this.txtTimeHide = new System.Windows.Forms.TextBox();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 158);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.StartToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.ReloadToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // timerToShowWindow
            // 
            this.timerToShowWindow.Enabled = true;
            this.timerToShowWindow.Interval = 10000;
            this.timerToShowWindow.Tick += new System.EventHandler(this.TimerToShowWindow_Tick);
            // 
            // Settings
            // 
            this.Settings.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Settings.ContextMenuStrip = this.contextMenuStrip1;
            this.Settings.Icon = ((System.Drawing.Icon)(resources.GetObject("Settings.Icon")));
            this.Settings.Text = "WordsNotifier";
            this.Settings.Visible = true;
            this.Settings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseDoubleClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(542, 242);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(461, 242);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(13, 13);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(35, 13);
            this.lblFile.TabIndex = 3;
            this.lblFile.Text = "label1";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(174, 10);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(412, 20);
            this.txtFile.TabIndex = 4;
            // 
            // btnOpenFie
            // 
            this.btnOpenFie.Location = new System.Drawing.Point(592, 9);
            this.btnOpenFie.Name = "btnOpenFie";
            this.btnOpenFie.Size = new System.Drawing.Size(25, 23);
            this.btnOpenFie.TabIndex = 5;
            this.btnOpenFie.Text = "...";
            this.btnOpenFie.UseVisualStyleBackColor = true;
            this.btnOpenFie.Click += new System.EventHandler(this.BtnOpenFie_Click);
            // 
            // lblTimeToShowWindow
            // 
            this.lblTimeToShowWindow.AutoSize = true;
            this.lblTimeToShowWindow.Location = new System.Drawing.Point(12, 47);
            this.lblTimeToShowWindow.Name = "lblTimeToShowWindow";
            this.lblTimeToShowWindow.Size = new System.Drawing.Size(120, 13);
            this.lblTimeToShowWindow.TabIndex = 6;
            this.lblTimeToShowWindow.Text = "Timeout to show a word";
            // 
            // lblTimeToHideWindow
            // 
            this.lblTimeToHideWindow.AutoSize = true;
            this.lblTimeToHideWindow.Location = new System.Drawing.Point(12, 79);
            this.lblTimeToHideWindow.Name = "lblTimeToHideWindow";
            this.lblTimeToHideWindow.Size = new System.Drawing.Size(137, 13);
            this.lblTimeToHideWindow.TabIndex = 7;
            this.lblTimeToHideWindow.Text = "Time to hide popup window";
            // 
            // txtTimeShow
            // 
            this.txtTimeShow.Location = new System.Drawing.Point(174, 44);
            this.txtTimeShow.Name = "txtTimeShow";
            this.txtTimeShow.Size = new System.Drawing.Size(412, 20);
            this.txtTimeShow.TabIndex = 8;
            // 
            // txtTimeHide
            // 
            this.txtTimeHide.Location = new System.Drawing.Point(174, 76);
            this.txtTimeHide.Name = "txtTimeHide";
            this.txtTimeHide.Size = new System.Drawing.Size(412, 20);
            this.txtTimeHide.TabIndex = 9;
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sortToolStripMenuItem.Text = "Sort";
            this.sortToolStripMenuItem.Click += new System.EventHandler(this.SortToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 277);
            this.Controls.Add(this.txtTimeHide);
            this.Controls.Add(this.txtTimeShow);
            this.Controls.Add(this.lblTimeToHideWindow);
            this.Controls.Add(this.lblTimeToShowWindow);
            this.Controls.Add(this.btnOpenFie);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer timerToShowWindow;
        private System.Windows.Forms.NotifyIcon Settings;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnOpenFie;
        private System.Windows.Forms.Label lblTimeToShowWindow;
        private System.Windows.Forms.Label lblTimeToHideWindow;
        private System.Windows.Forms.TextBox txtTimeShow;
        private System.Windows.Forms.TextBox txtTimeHide;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
    }
}

