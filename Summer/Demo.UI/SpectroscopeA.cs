using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Demo.UI
{
    public partial class SpectroscopeA : UserControl
    {

        private LinearGradientMode _mode;
        private Rectangle _fillRect;
        private Pen _pen;
        private LinearGradientBrush _brush;


        public SpectroscopeA()
        {
            InitializeComponent();

            _mode = LinearGradientMode.ForwardDiagonal;
            _fillRect = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
            _pen = new Pen(Color.Black, 1);
            _brush = new LinearGradientBrush(_fillRect, Color.Pink, Color.Pink, _mode);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Width = this.Height;
            this.label1.Location = new Point(this.Width / 2 + 5, this.Width / 2 - 18);
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {

        }

        protected override void OnMouseLeave(System.EventArgs e)
        {

        }

        // Draw the new button. 
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            DrawTrapezoid(e);
        }

        private void DrawTrapezoid(PaintEventArgs e)
        {
            // Create points that define polygon.
            PointF point1 = new PointF(20, 20);
            PointF point2 = new PointF(20, this.Width / 8 + 20);
            PointF point3 = new PointF(this.Width * 7 / 8 - 20, this.Width  - 20);
            PointF point4 = new PointF(this.Width - 20, this.Width  - 20);
            PointF[] curvePoints =
                     {
                 point1,
                 point2,
                 point3,
                 point4,
             };

            // Draw polygon to screen.
            e.Graphics.FillPolygon(_brush, curvePoints);
            e.Graphics.DrawPolygon(_pen, curvePoints);

        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _brush.Dispose();
                _pen.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
