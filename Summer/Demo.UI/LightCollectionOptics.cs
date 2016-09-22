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
    public partial class LightCollectionOptics : UserControl
    {
        private Pen _pen;
        public LightCollectionOptics()
        {
            InitializeComponent();

            _pen = new Pen(Color.Black, 1);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Size = new Size(227, 75);
        }

        /// <summary>
        /// Calculate X coordinate on an ellipse
        /// </summary>
        /// <param name="width">Ellipse width</param>
        /// <param name="height">Ellipse height</param>
        /// <param name="y">Y ranging from 0 to height</param>
        /// <returns>X relative to the center of the ellipse</returns>
        /// 
        static float EllipseCalculateX(float width, float height, float y)
        {
            if (y < 0 || y > height)
            {
                return 0;
            }

            y = y - (height / 2f);
            var a = width / 2f;
            var b = height / 2f;

            var x = (a * Math.Sqrt((b * b) - (y * y))) / b;

            return (float)x;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;


            // length, width, and depth of the storage in pixels
            //
            var length = 80f;
            var height = 40f;
            var depth = 20f;


            var start = new PointF(10f, 5);

            var cylinder = new RectangleF(start.X, start.Y, length, height);
            var leftCap = new RectangleF(cylinder.Left - (depth / 2f), cylinder.Top, depth, height);
            var rightCap = new RectangleF(cylinder.Right - (depth / 2f), cylinder.Top, depth, height);

            //var outline = new Pen(Color.Black, 1f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel };

            // outline the top and bottom of the storage
            //
            g.DrawLine(_pen, cylinder.Left, cylinder.Top, cylinder.Right, cylinder.Top);
            g.DrawLine(_pen, cylinder.Left, cylinder.Bottom, cylinder.Right, cylinder.Bottom);


            // outline the right cap
            //
            g.DrawEllipse(_pen, rightCap);


            // outline the left cap
            //
            g.DrawEllipse(_pen, leftCap);
        }

        protected override void OnResize(EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _pen.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
