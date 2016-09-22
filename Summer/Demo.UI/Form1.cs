using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Graphics graphics = e.Graphics;
            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //Pen p = new Pen(Color.Black, 2);
            //var v1 = spectroscope1.GetTopMidPoint();
            //var v2 = spectroscope2.GetTopMidPoint();

            //var v3 = spectroscope1.PointToScreen(spectroscope1.GetTopMidPoint());
            //var v4 = spectroscope2.PointToScreen(spectroscope2.GetTopMidPoint());
            //Point p1 = new Point(spectroscope1.Location.X + spectroscope1.GetTopMidPoint().X,
            //    spectroscope1.Location.Y + spectroscope1.GetTopMidPoint().Y);
            //Point p2 = new Point(spectroscope2.Location.X + spectroscope2.GetTopMidPoint().X,
            //    spectroscope2.Location.Y + spectroscope2.GetTopMidPoint().Y + 50);
            //e.Graphics.DrawLine(p, p1, p2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Invalidate(true);
        }
    }
}
