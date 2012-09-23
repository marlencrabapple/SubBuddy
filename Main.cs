/*
Copyright (C) 2012 Ian Bradley

This file is part of SubBuddy.

SubBuddy is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

SubBuddy is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with SubBuddy. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace SubBuddy
{
    public partial class Main : Form
    {
        delegate void SetTextCallback(string text);

        public static Buddy buddy = new Buddy();
        //public static String versionNumber = "0.2.3.3";

        public Main()
        {
            InitializeComponent();
        }

        private void DoEet()
        {
            buddy.watch(Username.Text, Password.Text, "", this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(DoEet));
            t.IsBackground = true;
            t.Start();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            options.ShowDialog();
        }

        private void openDownloadsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(CompatSettings.Default.Path + "/");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void EnableButton(bool enable)
        {
            this.Invoke(new MethodInvoker(delegate { this.WatchButton.Enabled = enable; }));
        }

        public void set_button1_text(String text)
        {
            this.Invoke(new MethodInvoker(delegate { this.WatchButton.Text = text; }));
        }

        public void set_list_text(List<string> list)
        {
            if (list == null)
            {
                CurrentVids.Text = "";
            }
            else
            {
                CurrentVids.DataSource = list;
            }
        }

        public void set_statusbar_text(String text)
        {
            // No idea how this stayed in the code for so long.
            // toolStripStatusLabel1.Text = text;
            this.Invoke(new MethodInvoker(delegate { this.toolStripStatusLabel1.Text = text; }));
        }

        private void setStatusBarTextPartTwoElectricBoogaloo(String text)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Remember.Checked == true)
            {
                CompatSettings.Default.Username = Username.Text;
                CompatSettings.Default.Password = Password.Text;
                CompatSettings.Default.Remember = true;
                CompatSettings.Default.Save();
            }
            if (Remember.Checked == false)
            {
                CompatSettings.Default.Remember = false;
                CompatSettings.Default.Username = "";
                CompatSettings.Default.Password = "";
                CompatSettings.Default.Save();
            }
        }
        private void Main_Load(object sender, EventArgs e)
        {
            if (CompatSettings.Default.Path == "")
            {
                MessageBox.Show("It seems as though this is SubBuddy's first launch. Please configure all enabled options before using the program.");
                Options options = new Options();
                options.ShowDialog();
            }

            Username.Text = CompatSettings.Default.Username;
            Password.Text = CompatSettings.Default.Password;

            if (!Directory.Exists(CompatSettings.Default.Path))
            {
                Directory.CreateDirectory(CompatSettings.Default.Path);
            }

            if (!File.Exists(CompatSettings.Default.Path + "/downloaded"))
            {
                FileStream myStream = File.Create(CompatSettings.Default.Path + "/downloaded");
                myStream.Flush();
                myStream.Close();
            }
            if (!File.Exists(CompatSettings.Default.Path + "/blacklist"))
            {
                FileStream myStream = File.Create(CompatSettings.Default.Path + "/blacklist");
                myStream.Flush();
                myStream.Close();
            }
            if (!File.Exists(CompatSettings.Default.Path + "/localsubs"))
            {
                FileStream myStream = File.Create(CompatSettings.Default.Path + "/localsubs");
                myStream.Flush();
                myStream.Close();
            }
            if (!File.Exists(CompatSettings.Default.Path + "/synonyms"))
            {
                FileStream myStream = File.Create(CompatSettings.Default.Path + "/synonyms");
                myStream.Flush();
                myStream.Close();
            }
        }

        private void ignoreListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Blacklist blacklist = new Blacklist();
            blacklist.ShowDialog();
        }

        private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buddy.SubSync(Username.Text, this);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalSubs localsubs = new LocalSubs();
            localsubs.ShowDialog();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://onlinebargainshrimptoyourdoor.com/subbuddy/");
        }

        private void accountSynonymsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountSynonyms synonyms = new AccountSynonyms();
            synonyms.ShowDialog();
        }

        public String getUsernameText()
        {
            return this.Username.Text;
        }

        private void skipCurrentQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buddy.setQueueDownloaded(Username.Text, Password.Text, "", this);
        }
    }
}
