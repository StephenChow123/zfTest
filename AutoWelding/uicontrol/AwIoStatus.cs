using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AutoWelding.uicontrol
{
    class AwIoStatus : Panel
    {
        private AwPicBox picBox;
        private Label title;
        private Color backgroundColor;
        private Color onColor;
        private Color offColor;
        private bool status;
        private string text;

        public bool Status
        {
            get { return status; }
            set { 
                status = value;
                picBox.UpdateForeColor(status ? onColor : offColor);
            }
        }


        public AwIoStatus(string name)
        {
            backgroundColor = Color.FromArgb(100, 128, 128, 128);
            onColor = Color.LightGreen;
            offColor = Color.Blue;
            status = false;
            text = name;

            picBox = new AwPicBox(status ? onColor : offColor, 20, 20, 8);
            title = new Label();

            picBox.Width = 30;
            picBox.Height = 30;

            Point point = new Point(0, 0);

            title.Text = text;
            point.X = picBox.Width > title.Width ? (picBox.Width - title.Width) / 2 : point.X;
            title.Location = point;            
            title.Height = title.Font.Height;
            title.Parent = this;

            point.X = 0;
            point.Y = title.Height;
            picBox.Location = point;
            picBox.BackColor = backgroundColor;
            picBox.Parent = this;

        }

        /**********************************************************************************************
         * discription: ªÊ÷∆Õº–Œ
         * 
         * 
         ***********************************************************************************************/
        protected override void OnPaint(PaintEventArgs pe)
        {

        }
    }
}
