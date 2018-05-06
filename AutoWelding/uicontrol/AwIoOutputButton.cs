using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AutoWelding.uicontrol
{
    class AwIoOutputButton : Panel
    {
        private Button awButton;
        private Label title;        
        private Color onColor;
        private Color offColor;
        private bool status;
        private string text;

        public Button AwButton
        {
            get { return awButton; }
        }

        public bool Status
        {
            get { return status; }
            set { 
                status = value;
                awButton.BackColor = status ? onColor : offColor;
            }
        }

        public AwIoOutputButton(string name)
        {
            awButton = new Button();

            onColor = Color.LightGreen;
            offColor = Color.Blue;//Color.Red; 

            status = false;
            text = name;
            
            title = new Label();

            awButton.Width = 25;
            awButton.Height = 25;

            Point point = new Point(0, 0);

            title.Text = text;
            point.X = awButton.Width > title.Width ? (awButton.Width - title.Width) / 2 : point.X;
            title.Location = point;            
            title.Height = title.Font.Height;
            title.Parent = this;

            point.X = 0;
            point.Y = title.Height;
            awButton.Location = point;
            awButton.BackColor = status ? onColor : offColor;
            awButton.Parent = this;

        }
    }
}
