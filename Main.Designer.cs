/*
Copyright (C) 2012 Ian Bradley

This file is part of SubBuddy.

SubBuddy is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

SubBuddy is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with SubBuddy. If not, see http://www.gnu.org/licenses/.
*/

namespace SubBuddy
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Username = new System.Windows.Forms.TextBox();
            this.openDownloadsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.youTubeAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.syncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localSubscriptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.manageSubscriptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountSynonymsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.WatchButton = new System.Windows.Forms.Button();
            this.Password = new System.Windows.Forms.TextBox();
            this.CurrentVids = new System.Windows.Forms.ListBox();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Remember = new System.Windows.Forms.CheckBox();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.skipCurrentQueueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(12, 43);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(100, 20);
            this.Username.TabIndex = 13;
            // 
            // openDownloadsFolderToolStripMenuItem
            // 
            this.openDownloadsFolderToolStripMenuItem.Name = "openDownloadsFolderToolStripMenuItem";
            this.openDownloadsFolderToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.openDownloadsFolderToolStripMenuItem.Text = "Open Downloads Folder";
            this.openDownloadsFolderToolStripMenuItem.Click += new System.EventHandler(this.openDownloadsFolderToolStripMenuItem_Click);
            // 
            // ignoreListToolStripMenuItem
            // 
            this.ignoreListToolStripMenuItem.Name = "ignoreListToolStripMenuItem";
            this.ignoreListToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.ignoreListToolStripMenuItem.Text = "Blacklist";
            this.ignoreListToolStripMenuItem.Click += new System.EventHandler(this.ignoreListToolStripMenuItem_Click);
            // 
            // youTubeAccountToolStripMenuItem
            // 
            this.youTubeAccountToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ignoreListToolStripMenuItem});
            this.youTubeAccountToolStripMenuItem.Name = "youTubeAccountToolStripMenuItem";
            this.youTubeAccountToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.youTubeAccountToolStripMenuItem.Text = "YouTube Account";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1,
            this.homepageToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Enabled = false;
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(171, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // homepageToolStripMenuItem
            // 
            this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
            this.homepageToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.homepageToolStripMenuItem.Text = "Homepage";
            this.homepageToolStripMenuItem.Click += new System.EventHandler(this.homepageToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // syncToolStripMenuItem
            // 
            this.syncToolStripMenuItem.Name = "syncToolStripMenuItem";
            this.syncToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.syncToolStripMenuItem.Text = "Sync";
            this.syncToolStripMenuItem.Click += new System.EventHandler(this.syncToolStripMenuItem_Click);
            // 
            // localSubscriptionsToolStripMenuItem
            // 
            this.localSubscriptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syncToolStripMenuItem,
            this.editToolStripMenuItem});
            this.localSubscriptionsToolStripMenuItem.Name = "localSubscriptionsToolStripMenuItem";
            this.localSubscriptionsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.localSubscriptionsToolStripMenuItem.Text = "Local Subscriptions";
            // 
            // statusStrip1
            // 
            this.statusStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 166);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(288, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(26, 17);
            this.toolStripStatusLabel1.Text = "Idle";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Username";
            // 
            // manageSubscriptionsToolStripMenuItem
            // 
            this.manageSubscriptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localSubscriptionsToolStripMenuItem,
            this.youTubeAccountToolStripMenuItem,
            this.accountSynonymsToolStripMenuItem});
            this.manageSubscriptionsToolStripMenuItem.Name = "manageSubscriptionsToolStripMenuItem";
            this.manageSubscriptionsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.manageSubscriptionsToolStripMenuItem.Text = "Manage Subscriptions";
            // 
            // accountSynonymsToolStripMenuItem
            // 
            this.accountSynonymsToolStripMenuItem.Name = "accountSynonymsToolStripMenuItem";
            this.accountSynonymsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.accountSynonymsToolStripMenuItem.Text = "Account Synonyms";
            this.accountSynonymsToolStripMenuItem.Click += new System.EventHandler(this.accountSynonymsToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Password";
            // 
            // WatchButton
            // 
            this.WatchButton.Location = new System.Drawing.Point(12, 131);
            this.WatchButton.Name = "WatchButton";
            this.WatchButton.Size = new System.Drawing.Size(84, 23);
            this.WatchButton.TabIndex = 18;
            this.WatchButton.Text = "Watch";
            this.WatchButton.UseVisualStyleBackColor = true;
            this.WatchButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Password
            // 
            this.Password.Enabled = false;
            this.Password.Location = new System.Drawing.Point(12, 82);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(100, 20);
            this.Password.TabIndex = 15;
            // 
            // CurrentVids
            // 
            this.CurrentVids.FormattingEnabled = true;
            this.CurrentVids.Location = new System.Drawing.Point(156, 33);
            this.CurrentVids.Name = "CurrentVids";
            this.CurrentVids.Size = new System.Drawing.Size(120, 121);
            this.CurrentVids.TabIndex = 17;
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // Remember
            // 
            this.Remember.AutoSize = true;
            this.Remember.Location = new System.Drawing.Point(12, 108);
            this.Remember.Name = "Remember";
            this.Remember.Size = new System.Drawing.Size(112, 17);
            this.Remember.TabIndex = 21;
            this.Remember.Text = "Remember Details";
            this.Remember.UseVisualStyleBackColor = true;
            this.Remember.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.manageSubscriptionsToolStripMenuItem,
            this.openDownloadsFolderToolStripMenuItem,
            this.skipCurrentQueueToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // skipCurrentQueueToolStripMenuItem
            // 
            this.skipCurrentQueueToolStripMenuItem.Name = "skipCurrentQueueToolStripMenuItem";
            this.skipCurrentQueueToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.skipCurrentQueueToolStripMenuItem.Text = "Skip Current Queue";
            this.skipCurrentQueueToolStripMenuItem.Click += new System.EventHandler(this.skipCurrentQueueToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(288, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 188);
            this.Controls.Add(this.Username);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WatchButton);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.CurrentVids);
            this.Controls.Add(this.Remember);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "SubBuddy";
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.ToolStripMenuItem openDownloadsFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem youTubeAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem homepageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localSubscriptionsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem manageSubscriptionsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button WatchButton;
        public System.Windows.Forms.TextBox Password;
        public System.Windows.Forms.ListBox CurrentVids;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.CheckBox Remember;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem accountSynonymsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skipCurrentQueueToolStripMenuItem;
    }
}

