using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoWelding.test
{
    public partial class agilentTest : Form
    {
        public agilentTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AutoWelding.mcAgilent.config();
        }

        private void button2_Click(object sender, EventArgs e)
        {
          listBox1.Items.Add( AutoWelding.mcAgilent.Measure());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AutoWelding.mcAgilent.adjust();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool IsOk=AutoWelding.mcAgilent.InitIO(textBox1.Text);
            button1.Enabled = IsOk;
            button2.Enabled = IsOk;
            button3.Enabled = IsOk;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AutoWelding.mcAgilent.closeIO();
        }

        private void agilentTest_Load(object sender, EventArgs e)
        {

        }
    }
}
