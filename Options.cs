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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SubBuddy
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            textBox1.Text = Settings.Default.Path;
            comboBox2.SelectedIndex = Settings.Default.SubscriptionType;
            comboBox1.SelectedIndex = Settings.Default.Thumbnails;
            textBox2.Text = Settings.Default.Naming;
            numericUpDown1.Value = Settings.Default.Delay;
            textBox2.Enabled = false;
            comboBox1.Enabled = false;
            numericUpDown2.Value = Settings.Default.DownloadQueue;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            button2.Click += new EventHandler(button2_Click);
            textBox2.MouseHover += new EventHandler(textBox2_MouseHover);
        }

        void textBox2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox2, "Options: %user %id %date %title %quality");
        }

        void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            textBox1.Text = folder.SelectedPath;
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.Default.Delay = numericUpDown1.Value;
            Settings.Default.DownloadQueue = numericUpDown2.Value;
            Settings.Default.SubscriptionType = comboBox2.SelectedIndex;
            Settings.Default.Thumbnails = comboBox1.SelectedIndex;
            Settings.Default.Naming = textBox2.Text;
            
            if (!Settings.Default.Path.EndsWith("/SubBuddy/"))
            {
                Settings.Default.Path = textBox1.Text + "/SubBuddy/";
            }
            else
            {
                Settings.Default.Path = textBox1.Text;
            }

            Settings.Default.Save();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
