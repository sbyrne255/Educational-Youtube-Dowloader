using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Educational_Youtube_Saver
{
    public partial class WaitingLoading : Form
    {
        public WaitingLoading(string title)
        {
            InitializeComponent();
            x = title;
        }
        public string x;

        private void Loading_Load(object sender, EventArgs e)
        {

            this.Text = x;
            //textBox1.Text = "We are loading your video information, \n this may take a few seconds...";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.ActiveControl = label1;
        }
    }
}
