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
using Summer.UI.Line;

namespace Demo.UI
{
    public partial class Spectroscope : UserControl
    {

        public enum Shape
        {
            TOP,
            BOTTOM,
        }

        public enum States
        {
            Normal,
            MouseOver,
            Clicked
        }

        private LinearGradientMode _mode;
        private Rectangle _fillRect;
        private Pen _pen;
        private LinearGradientBrush _brush;
        private Shape _shape;
        private States _state = States.Normal;
        //private GraphicsPath _graphicsPath;

        public Spectroscope()
        {
            InitializeComponent();

            _mode = LinearGradientMode.ForwardDiagonal;
            _fillRect = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
            _pen = new Pen(Color.Black, 1);
            _brush = new LinearGradientBrush(_fillRect, Color.Pink, Color.Pink, _mode);
            _shape = Shape.TOP;

            //_graphicsPath = new GraphicsPath();
            this.Click += Spectroscope_Click;
        }

        private void Spectroscope_Click(object sender, EventArgs e)
        {

        }

        public Shape ShapeType
        {
            set
            {
                this._shape = value;
            }
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
            PointF[] curvePoints = CurvePoint(_shape);
            switch (_state)
            {
                case States.Normal:
                    _brush = new LinearGradientBrush(_fillRect, Color.Pink, Color.Pink, _mode);
                    e.Graphics.FillPolygon(_brush, curvePoints);
                    e.Graphics.DrawPolygon(_pen, curvePoints);
                    break;
                case States.MouseOver:
                    _brush = new LinearGradientBrush(_fillRect, Color.MediumVioletRed, Color.MediumVioletRed, _mode);
                    e.Graphics.FillPolygon(_brush, curvePoints);
                    e.Graphics.DrawPolygon(_pen, curvePoints);
                    var v1 = this.Parent;
                    v1.Refresh();
                    break;
            }
        }

        private PointF[] CurvePoint(Shape shape)
        {
            PointF point1 = new PointF();
            PointF point2 = new PointF();
            PointF point3 = new PointF();
            PointF point4 = new PointF();
            switch (shape)
            {
                case Shape.TOP:
                    point1 = new PointF(20, 20);
                    point2 = new PointF(this.Width - 20, this.Width - 20);
                    point3 = new PointF(this.Width - 20, this.Width * 7 / 8 - 20);
                    point4 = new PointF(this.Width / 8 + 20, 20);
                    break;
                case Shape.BOTTOM:
                    point1 = new PointF(20, 20);
                    point2 = new PointF(20, this.Width / 8 + 20);
                    point3 = new PointF(this.Width * 7 / 8 - 20, this.Width - 20);
                    point4 = new PointF(this.Width - 20, this.Width - 20);
                    break;
                default:
                    point1 = new PointF(20, 20);
                    point2 = new PointF(this.Width - 20, this.Width - 20);
                    point3 = new PointF(this.Width - 20, this.Width * 7 / 8 - 20);
                    point4 = new PointF(this.Width / 8 + 20, 20);
                    break;
            }
            PointF[] curvePoints =
            {
                 point1,
                 point2,
                 point3,
                 point4,
             };
            return curvePoints;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Width = this.Height;
            this.label1.Location = new Point(this.Width / 2 + 8, this.Width / 2 - 20);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //if (_graphicsPath.IsVisible(e.Location))
            //{
            //    _state = States.MouseOver;
            //    this.Invalidate();
            //}
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //if (_graphicsPath.IsVisible(e.Location))
            //{
            //    _state = States.Normal;
            //    this.Invalidate();
            //}
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
