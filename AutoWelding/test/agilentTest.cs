using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

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
            string[] ports = SerialPort.GetPortNames();
            if (AutoWelding.mcTC6200P.IsHandshaked)
            {
                comboBox2.Enabled = false;
                comboBox2.Items.Add(AutoWelding.mcTC6200P.comBoard.PortName);
                comboBox2.SelectedItem = 0;
                button6.Text = "连接成功";
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    try
                    {
                        SerialPort sp = new SerialPort(ports[i]);
                        sp.Open();
                        sp.Close();
                        ///可否自动握手？
                        comboBox2.Items.Add(ports[i]);
                        comboBox2.SelectedItem = 0;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (AutoWelding.mcComBoard.IsHandshaked)
            {
                comboBox1.Enabled = false;
                comboBox1.Items.Add(AutoWelding.mcComBoard.comBoard.PortName);
                comboBox1.SelectedItem = 0;
                button9.Text = "连接成功";
            }
          else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    try
                    {
                        SerialPort sp = new SerialPort(ports[i]);
                        sp.Open();
                        sp.Close();
                        comboBox1.Items.Add(ports[i]);
                        comboBox2.SelectedItem = comboBox2.Items.Count-1;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AutoWelding.mcTC6200P = new TC6200P(comboBox2.Text);
            button6.Text = AutoWelding.mcTC6200P.Handshake() ? "连接成功" : "连接失败";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AutoWelding.mcComBoard = new ComBoard(comboBox1.Text);
            button9.Text =  AutoWelding.mcComBoard.Handshake() ? "连接成功" : "连接失败";
        }

        private void switch1_StateChanged(object sender, NationalInstruments.UI.ActionEventArgs e)
        {
            if(switch1.Value)
            {
                AutoWelding.mcTC6200P.SetOn();
            }
            else
            {
                AutoWelding.mcTC6200P.SetOff();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                float vOut = (float)Convert.ToDouble(textBox7.Text);
                AutoWelding.mcTC6200P.SetVolt(vOut);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                float vMax = (float)Convert.ToDouble(textBox2.Text);
                float vMin = (float)Convert.ToDouble(textBox3.Text);
                AutoWelding.mcTC6200P.SetVoltLimHight(vMax);
                AutoWelding.mcTC6200P.SetVoltLimLow(vMin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                float vMax = (float)Convert.ToDouble(textBox4.Text);
                float vMin = (float)Convert.ToDouble(textBox5.Text);
                AutoWelding.mcTC6200P.SetCurrLimH(vMax);
                AutoWelding.mcTC6200P.SetCurrLimL(vMin);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
 }

