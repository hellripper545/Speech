using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BunnyBot.Properties;

namespace BunnyBot
{
    public partial class help : Form
    {
        //string sal="Sir";
                 
        public help()
        {
            InitializeComponent();
            //salutation.Text = str;
        }

       private void help_Load(object sender, EventArgs e)
        {
            
            pictureBox2.ImageLocation = "Resources\\BunnyBot2.gif";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            /*StringBuilder items = new StringBuilder();
            items.Append(Environment.NewLine + "sai teja");
            richTextBox1.Text = items.ToString();*/
            richTextBox1.Enabled = false;
         }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //pictureBox1.ImageLocation = "C:\\Users\\RAMYA\\Downloads\\Speech-master\\BunnyBot\\Resources\\BunnyBot2.gif";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
