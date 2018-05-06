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
    public partial class FormIoControl : Form
    {
        private SystemParam sysParam;
        private AwPanel inputIoPanel;
        private AwPanel outputIoPanel;
        List<AwIoStatus> inputIoList;
        List<AwIoOutputButton> outputIoList;


        public FormIoControl(ref SystemParam paramSys)
        {
            sysParam = paramSys;
            inputIoList = new List<AwIoStatus>();
            outputIoList = new List<AwIoOutputButton>();
            InitializeComponent();
        }

        private void FormIoControl_Load(object sender, EventArgs e)
        {
            this.Size = new Size(560, 428);
            InitUiControls();
        }

        private void InitUiControls()
        {
            InitInputControls();
            InitOutputControls();
            InitBottomControls();
        }

        private void InitInputControls()
        {
            Point point = new Point(20, 20);
            
            inputIoPanel = new AwPanel();
            inputIoPanel.Location = point;
            inputIoPanel.BorderColor = sysParam.ControlFrameColor;
            inputIoPanel.BorderWidth = 3;
            inputIoPanel.Width = this.Width - 40;
            inputIoPanel.Height = 160;
            inputIoPanel.Parent = this;
            inputIoPanel.TitleBackground.Height = 30;
            inputIoPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;
            inputIoPanel.Title.Text = "IO ‰»Î";

            point.X = 40;
            point.Y = inputIoPanel.Title.Height + 20;

            for( int i = 1; i < 19; i++)
            {
                AwIoStatus ioInput = new AwIoStatus( i.ToString() );
                ioInput.Parent = inputIoPanel;
                ioInput.Width = 30;
                ioInput.Height = 50;
                ioInput.Location = point;
                inputIoList.Add(ioInput);

                point.X += ioInput.Width + 20;

                //if ( ( point.X + ioInput.Width + 20 ) > inputIoPanel.Width)
                //{
                //    point.X = 10;
                //    point.Y += ioInput.Height + 20;
                //}

                if (i == 9)
                {
                    point.X = 40;
                    point.Y += ioInput.Height + 10;
                }
            }


        }

        private void InitOutputControls()
        {
            Point point = inputIoPanel.Location;
            point.Y += inputIoPanel.Height + 20;

            outputIoPanel = new AwPanel();
            outputIoPanel.Location = point;
            outputIoPanel.BorderColor = sysParam.ControlFrameColor;
            outputIoPanel.BorderWidth = 3;
            outputIoPanel.Width = this.Width - 40;
            outputIoPanel.Height = 160;
            outputIoPanel.Parent = this;
            outputIoPanel.TitleBackground.Height = 30;
            outputIoPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;
            outputIoPanel.Title.Text = "IO ‰≥ˆ";

            point.X = 20;
            point.Y = outputIoPanel.Title.Height + 20;

            for (int i = 1; i < 25; i++)
            {
                AwIoOutputButton ioOutput = new AwIoOutputButton(i.ToString());
                ioOutput.Parent = outputIoPanel;
                ioOutput.Width = 30;
                ioOutput.Height = 50;
                ioOutput.Location = point;
                ioOutput.AwButton.Click += button_Click;
                outputIoList.Add(ioOutput);

                point.X += ioOutput.Width + 10;

                //if ( ( point.X + ioOutput.Width + 20 ) > outputIoPanel.Width)
                //{
                //    point.X = 10;
                //    point.Y += ioOutput.Height + 20;
                //}

                if (i == 12)
                {
                    point.X = 20;
                    point.Y += ioOutput.Height + 10;
                }
            }

        }

        private void InitBottomControls()
        {
            Point point = new Point();
            point.X = outputIoPanel.Location.X;
            point.Y = outputIoPanel.Location.Y + outputIoPanel.Height + 10;

            labelCard.Location = point;

            point.X += labelCard.Width + 20;
            comboBoxCard.Items.Add("1");
            comboBoxCard.Items.Add("2");
            comboBoxCard.Location = point;

            point.X = outputIoPanel.Location.X + outputIoPanel.Width - buttonExit.Width;
            buttonExit.Location = point;
        }

        /**********************************************************************************************
         * discription: Io Out 
         * 
         * 
         ***********************************************************************************************/
        private void button_Click(object sender, EventArgs e)
        {
            ((AwIoOutputButton)((Button)sender).Parent).Status = !((AwIoOutputButton)((Button)sender).Parent).Status;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}