using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SubBuddy
{
    public partial class AccountSynonyms : Form
    {
        public AccountSynonyms()
        {
            InitializeComponent();
        }

        private void AccountSynonyms_Load(object sender, EventArgs e)
        {
            if (File.ReadAllText(CompatSettings.Default.Path + "synonyms") != "")
            {
                richTextBox1.Text = File.ReadAllText(CompatSettings.Default.Path + "synonyms");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(CompatSettings.Default.Path + "synonyms", richTextBox1.Text);
        }
    }
}
