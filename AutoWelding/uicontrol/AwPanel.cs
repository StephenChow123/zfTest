using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AutoWelding.uicontrol
{
    class AwPanel : Panel
    {
        private Color borderColor;
        private int borderWidth;
        private Panel titleBackground;
        private Label title;

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public int BorderWidth
        {
            get { return borderWidth; }
            set 
            { 
                borderWidth = value;
                titleBackground.Width = this.Width - (borderWidth-1) * 2 + 1;

                Point point = new Point(borderWidth - 1, borderWidth - 1);
                titleBackground.Location = point;
            }
        }

        public Panel TitleBackground
        {
            get { return titleBackground; }
        }

        public Label Title
        {
            get { return title; }
        }

        public AwPanel()
        {           
            borderColor = BackColor;
            borderWidth = 2;
            titleBackground = new Panel();
            titleBackground.Margin = new Padding(0, 0, 0, 0);
            titleBackground.Padding = new Padding(0, 0, 0, 0);
            titleBackground.Parent = this;            
            title = new Label();
            title.Parent = titleBackground;
            title.Location = new Point(5, 5);
            this.SizeChanged += ControlSizeChanged;
        }

        /**********************************************************************************************
         * discription: ªÊ÷∆Õº–Œ
         * 
         * 
         ***********************************************************************************************/
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = this.CreateGraphics();
            Pen pen = new Pen(borderColor, borderWidth);
            g.DrawRectangle(pen, this.ClientRectangle);            
        }

        /**********************************************************************************************
         * discription: SizeChange
         * 
         * 
         ***********************************************************************************************/
        void ControlSizeChanged(object sender, EventArgs e)
        {
            titleBackground.Width = this.Width - (borderWidth-1) * 2 + 1;
        }
    }
}
