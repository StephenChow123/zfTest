using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AutoWelding.uicontrol;
using AutoWelding.system;
using AutoWelding.engine;
using AutoWelding.awdatabase;

namespace AutoWelding
{
    public partial class FormNote : Form,MsgReceiverInterface
    {
        private AwPanel loginPanel;
        private SystemParam sysParam;
        private string note;
        public FormNote(string msg)
        {
            note = msg;
            loginPanel = new AwPanel();
            sysParam = SystemParam.GetInstance();
            InitializeComponent();
        }

        private void FormNote_Load(object sender, EventArgs e)
        {
            InitControl();

            textBoxNote.Text = note;
            //SystemParam.GetInstance().RegisterReceiver(this);
        }

        void InitControl()
        { 
        
            Point point = new Point(20, 20);
            loginPanel.BorderColor = sysParam.ControlFrameColor;
            loginPanel.BorderWidth = 3;
            loginPanel.TitleBackground.Height = 30;
            loginPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;
            loginPanel.Title.Text = "提示";

            loginPanel.Parent = this;
            loginPanel.Width = this.Width - point.X * 2;
            loginPanel.Height = this.Height * 3 / 5;
            loginPanel.Location = point;

            point.X = 20;
            point.Y += loginPanel.Title.Height + 30;
            textBoxNote.Location = point;
            textBoxNote.Parent = loginPanel;

           
        }

        private void FormNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SystemParam.GetInstance().UnregisterReceiver(this);
        }

        public void HandleMsg(ref Message msg)
        {
            UiData uiData = (UiData)Marshal.PtrToStructure(msg.LParam, typeof(UiData));

            if((ControlMsg)msg.WParam == ControlMsg.REPLACE_COLLOID_STATUS)
            {
                this.Close();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }
    }
}
