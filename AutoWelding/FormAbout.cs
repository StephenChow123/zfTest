using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AutoWelding.uicontrol;
using AutoWelding.system;

namespace AutoWelding
{
    public partial class FormAbout : Form
    {
        private AwPanel versionPanel;
        private SystemParam sysParam;
        public FormAbout( ref SystemParam paramSys )
        {
            sysParam = paramSys;
            versionPanel = new AwPanel();
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            this.Size = new Size(460, 290);
            InitControls();
        }

        void InitControls()
        {
            Point point = new Point(20, 20);
            versionPanel.BorderColor = sysParam.ControlFrameColor;
            versionPanel.BorderWidth = 3;
            versionPanel.TitleBackground.Height = 30;
            versionPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;
            versionPanel.Title.Text = "»Ìº˛–≈œ¢";

            versionPanel.Parent = this;
            versionPanel.Width = this.Width - point.X * 2;
            versionPanel.Height = this.Height * 2 / 3;
            versionPanel.Location = point;


            point.Y = (versionPanel.Height - versionPanel.TitleBackground.Height) / 2 - labelVersion.Height / 2;
            point.X = 20;
            labelVersion.Location = point;
            labelVersion.Text = sysParam.Version + " " + sysParam.ControlVersion;
            labelVersion.Parent = versionPanel;

            point.X = versionPanel.Location.X + versionPanel.Width - buttonExit.Width;
            point.Y = versionPanel.Location.Y + versionPanel.Height + 20;
            buttonExit.Location = point;

            labelCpyRight.Parent = versionPanel;
            labelCpyRight.Text = sysParam.CpyRight;
            point.Y = versionPanel.Height - 40;
            point.X = (versionPanel.Width - labelCpyRight.Width) / 2;
            labelCpyRight.Location = point;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}