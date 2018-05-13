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
    public partial class err : Form
    {
        //string strAg;
        public err()
        {
            InitializeComponent();
        }

        private void err_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
        }
        int iAgilent = 0, iComBoard = 0, iTC6200P = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        bool isAgOk, isComOk, isTCok;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            switch(iAgilent)
            {
                case 0:
                    AutoWelding.mcAgilent = new agilent.Agilent();
                    label1.Text = "Agilent初始化中...";
                    timer1.Enabled = true;
                    break;
                case 1:
                    isAgOk = AutoWelding.mcAgilent.InitIO(AutoWelding.strAgilent);
                    timer1.Enabled = true;
                    break;
                case 2:
                    if(isAgOk)
                    {
                        label1.Text = "Agilent初始化成功";
                        label1.ForeColor = Color.Blue;
                    }
                    else
                    {
                        label1.Text = "Agilent初始化未成功";
                        label1.ForeColor = Color.Red;
                    }
                    break;
                case 3:
                    timer1.Enabled = false;
                    break;
            }
            iAgilent++;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            switch (iComBoard)
            {
                case 0:
                    AutoWelding.mcComBoard = new ComBoard(AutoWelding.strComBoard);
                    label2.Text = "串口设备连接中...";
                    break;
                case 1:
                    AutoWelding.mcComBoard.Handshake();
                    break;
                case 2:
                    if (AutoWelding.mcComBoard.IsHandshaked)
                    {
                        label2.Text = "串口设备连接成功";
                        label2.ForeColor = Color.Blue;
                    }
                    else
                    {
                        label2.Text = "串口设备连接未成功";
                        label2.ForeColor = Color.Red;
                    }
                    break;
                case 3:
                    timer2.Enabled = false;
                    break;
            }
            iComBoard++;
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            switch (iTC6200P)
            {
                case 0:
                    AutoWelding.mcTC6200P = new TC6200P(AutoWelding.strTCCom);
                    label3.Text = "高压设备连接中...";
                    break;
                case 1:
                    AutoWelding.mcTC6200P.Handshake();
                    break;
                case 2:
                    if (AutoWelding.mcTC6200P.IsHandshaked)
                    {
                        label3.Text = "高压设备连接成功";
                        label3.ForeColor = Color.Blue;
                    }
                    else
                    {
                        label3.Text = "高压设备连接未成功";
                        label3.ForeColor = Color.Red;
                    }
                    break;
                case 3:
                    timer3.Enabled = false;
                    break;
            }
            iTC6200P++;
        }
    }
}
