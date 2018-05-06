using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AutoWelding.uicontrol
{
    class AwPicBox : PictureBox
    {
        private Color foreColor;
        private int radius;

        public void UpdateForeColor(Color fColor)
        {
            foreColor = fColor;
            this.Refresh();
        }

        public AwPicBox( Color fColor, int with, int height, int radious )
        {
            foreColor = fColor;
            Width = with;
            Height = height;
            this.radius = radious;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            Pen pen = new Pen(foreColor,3);
           // g.DrawRectangle(pen, this.ClientRectangle);
            
            SolidBrush brush = new SolidBrush(foreColor);
            g.FillEllipse(brush, Width / 2 - radius, Height / 2 - radius, radius * 2, radius * 2);
            
        }
    }
}
