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
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            bool IsOk=AutoWelding.mcAgilent.InitIO(textBox1.Text);
            button1.Enabled = IsOk;
            button2.Enabled = IsOk;
            button3.Enabled = IsOk;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
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
                comboBox2.Items.Add(AutoWelding.mcTC6200P.comBoard.PortName);
                comboBox2.SelectedItem = comboBox2.Items[0];
                btmHV.Text = "连接成功";
                groupBox4.Enabled = true;
                groupBox5.Enabled = true;
                groupBox6.Enabled = true;
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
                    }
                    catch (Exception)
                    {

                    }
                }
                if(comboBox2.Items.Count>0)
                comboBox2.SelectedItem = comboBox2.Items[0];
            }
            if (AutoWelding.mcComBoard.IsHandshaked)
            {
                comboBox1.Enabled = false;
                comboBox1.Items.Add(AutoWelding.mcComBoard.comBoard.PortName);
                comboBox1.SelectedItem = comboBox1.Items[0];
                btmCom.Text = "连接成功";
                groupBox7.Enabled = true;
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
                    }
                    catch (Exception)
                    {

                    }
                }
                if(comboBox1.Items.Count>1)
                {
                    comboBox1.SelectedItem = comboBox1.Items[comboBox1.Items.Count - 1];
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool IsShaked = false;
            try
            {
                AutoWelding.mcTC6200P.changeCom(comboBox2.Text);
                 IsShaked = AutoWelding.mcTC6200P.Handshake();
            }
            catch (Exception)
            {

            }

            btmHV.Text = IsShaked ? "连接成功" : "连接失败";
            groupBox4.Enabled = IsShaked;
            groupBox5.Enabled = IsShaked;
            groupBox6.Enabled = IsShaked;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            bool IsShaked = false; ;
            try
            {
                AutoWelding.mcComBoard.changeCom(comboBox1.Text)  ;
                 IsShaked = AutoWelding.mcComBoard.Handshake();
            }
            catch (Exception)
            {

            }
            btmCom.Text = IsShaked ? "连接成功" : "连接失败";
            groupBox7.Enabled = IsShaked;
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

                //throw;
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
                AutoWelding.mcTC6200P.H_Volot = vMax;
                AutoWelding.mcTC6200P.L_Volot = vMin;
            }
            catch (Exception)
            {

                //throw;
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
                AutoWelding.mcTC6200P.H_Curr = vMax;
                AutoWelding.mcTC6200P.L_Curr = vMin;
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AutoWelding.mcComBoard.SelectType(emType.Cgd);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AutoWelding.mcComBoard.SelectType(emType.Cds);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AutoWelding.mcComBoard.SelectType(emType.Cgs);
        }
        void mcComBoardShaked()
        {
            
        }
        public void saveSys()
        {
            openSave.RegistryOp.SaveValue("com_TC6200P", AutoWelding.mcTC6200P.comBoard.PortName);
            openSave.RegistryOp.SaveValue("com_ComBoard", AutoWelding.mcTC6200P.comBoard.PortName);
        }
        private void button6_Click_1(object sender, EventArgs e)
        {

        }
    }
 }

