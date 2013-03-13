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
using System.Net;
using System.Reflection;

namespace SubBuddy
{
    public partial class Main : Form
    {
        delegate void SetTextCallback(string text);

        public static Buddy buddy = new Buddy();
        int internalCheckChange = 0;
        String body;

        public Main()
        {
            InitializeComponent();
        }

        private void DoEet()
        {
            if (body.Equals("ready"))
            {
                buddy.watch(Username.Text, Password.Text, "", this, 1);
            }
            else
            {
                buddy.watch(Username.Text, Password.Text, "", this, 0);
            }
        }

        private void readyBody()
        {
            Thread t = new Thread(DoEet);
            t.IsBackground = true;
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            body = "";
            readyBody();
        }

        public void newThread()
        {
            body = "ready";
            this.Invoke(new MethodInvoker(delegate { readyBody(); }));
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            options.ShowDialog();
        }

        private void openDownloadsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Settings.Default.Path + "/");
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
            if (internalCheckChange == 0)
            {
                if (Remember.Checked == true)
                {
                    Settings.Default.Username = Username.Text;
                    Settings.Default.Password = Password.Text;
                    Settings.Default.Remember = true;
                    Settings.Default.Save();
                }
                if (Remember.Checked == false)
                {
                    Settings.Default.Remember = false;
                    Settings.Default.Username = "";
                    Settings.Default.Password = "";
                    Settings.Default.Save();
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            updateCheck(false);

            if (Settings.Default.Path == "")
            {
                MessageBox.Show("It seems as though this is SubBuddy's first launch. Please configure all enabled options before using the program.");
                Options options = new Options();
                options.ShowDialog();
            }

            if (Settings.Default.Remember == true)
            {
                // I should really learn a bit more about C# before I implement terrible solutions like this.
                internalCheckChange = 1;
                Remember.Checked = true;
                internalCheckChange = 0;
            }

            Username.Text = Settings.Default.Username;
            Password.Text = Settings.Default.Password;

            if (!Directory.Exists(Settings.Default.Path))
            {
                Directory.CreateDirectory(Settings.Default.Path);
            }

            if (!File.Exists(Settings.Default.Path + "/downloaded"))
            {
                FileStream myStream = File.Create(Settings.Default.Path + "/downloaded");
                myStream.Flush();
                myStream.Close();
            }
            if (!File.Exists(Settings.Default.Path + "/blacklist"))
            {
                FileStream myStream = File.Create(Settings.Default.Path + "/blacklist");
                myStream.Flush();
                myStream.Close();
            }
            if (!File.Exists(Settings.Default.Path + "/localsubs"))
            {
                FileStream myStream = File.Create(Settings.Default.Path + "/localsubs");
                myStream.Flush();
                myStream.Close();
            }
            if (!File.Exists(Settings.Default.Path + "/synonyms"))
            {
                FileStream myStream = File.Create(Settings.Default.Path + "/synonyms");
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
            About about = new About();
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

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateCheck(true);
        }

        private void updateCheck(bool verbose)
        {
            WebClient wc = new WebClient();
            String mostRecentUpdate = "";
            
            try
            {
                mostRecentUpdate = wc.DownloadString("http://onlinebargainshrimptoyourdoor.com/current");
            }
            catch (WebException e)
            {
                MessageBox.Show("Subbuddy failed to check for updates.");
            }
            
            wc.Dispose();

            if (mostRecentUpdate != "")
            {
                String[] mostRecentUpdateArray = mostRecentUpdate.Split('|');
                if (mostRecentUpdateArray[0] != Assembly.GetExecutingAssembly().GetName().Version.ToString())
                {
                    MessageBox.Show("A new version of SubBuddy is available. Please check http://onlinebargainshrimptoyourdoor.com/ for details.");
                }
                else
                {
                    if (verbose) { MessageBox.Show("You are already using the most recent version of SubBuddy."); }
                }
            }
        }
    }
}
