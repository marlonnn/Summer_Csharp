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
using Summer.UI.Button;

namespace Demo.UI
{
    public partial class Spectroscope : BaseButton
    {
        public enum Shape
        {
            TOP,
            BOTTOM,
        }

        private Shape _shape;
        [Description("Shape the Spectroscope?"), Category("Spectroscope"), DefaultValue(typeof(Shape), "TOP")]
        public Shape SpectrosShape
        {
            get
            {
                return this._shape;
            }

            set
            {
                if (value != this._shape)
                {
                    this._shape = value;
                    InvokeInvalidate(value);
                    this.Invalidate();
                }
            }
        }

        private string _spectrosText;

        [Description("Filter Display Text"), Category("Appearance")]
        public string SpectrosText
        {
            get
            {
                return _spectrosText;
            }
            set
            {
                if (value != _spectrosText)
                {
                    _spectrosText = value;
                    InvokeInvalidate(value);
                    this.Invalidate();
                }
            }
        }

        private void InvokeInvalidate(string value)
        {
            if (!IsHandleCreated)
                return;
            try
            {
                this.Invoke((MethodInvoker)delegate { this.label1.Text = value; });
            }
            catch { }
        }

        private void InvokeInvalidate(Shape value)
        {
            if (!IsHandleCreated)
                return;
            try
            {
                this.Invoke((MethodInvoker)delegate { this._shape = value; });
            }
            catch { }
        }

        public Spectroscope()
        {
            InitializeComponent();

            _fillRect = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
            _solidBrush = new SolidBrush(Color.Pink);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (String.IsNullOrEmpty(SpectrosText))
            {
                SpectrosText = this.Name;
            }
            else
            {
                this.label1.Text = SpectrosText;
            }

            DrawTrapezoid(graphics);
        }

        private void DrawTrapezoid(Graphics graphics)
        {
            if (_Active)
            {
                GraphicsPath path = new GraphicsPath();
                PointF[] _curvePoints = CurvePoint(SpectrosShape);
                path.AddPolygon(_curvePoints);
                _region = new Region(path);
                switch (_State)
                {
                    case Summer.UI.Attribute.States.Normal:
                        graphics.FillPolygon(_solidBrush, _curvePoints);
                        graphics.DrawPolygon(_pen, _curvePoints);
                        break;
                    case Summer.UI.Attribute.States.MouseOver:
                        graphics.FillPolygon(_lightSolidBrush, _curvePoints);
                        graphics.DrawPolygon(_pen, _curvePoints);
                        break;
                    case Summer.UI.Attribute.States.Clicked:
                        graphics.FillPolygon(_lightSolidBrush, _curvePoints);
                        graphics.DrawPolygon(_clickPen, _curvePoints);
                        break;
                }
            }

        }

        /// <summary>
        ///Create points that define polygon.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
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
            if (this.label1 != null)
            {
                this.label1.Location = new Point(this.Width / 2 + 8, this.Width / 2 - 20);
            }
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnClick(System.EventArgs e)
        {
            base.OnClick(e);
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
