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
    //[Designer(typeof(ShapeControlDesigner))]
    public partial class BaseLine : UserControl
    {
        public enum LineShape
        {
            Top,
            Left,
            Right,
            Bottom
        }

        private Point _p1, _p2;

        private Color _lineColor;

        private float _penWidth;

        private LineShape _shape;

        [Category("Appearance"), Description("shape of this line."), DefaultValue(typeof(LineShape), "Top")]
        public LineShape Shape
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
                    //this.Invalidate();
                }
            }
        }

        private void InvokeInvalidate(LineShape value)
        {
            if (!IsHandleCreated)
                return;

            try
            {
                this.Invoke((MethodInvoker)delegate { this._shape = value; });
            }
            catch { }
        }

        private bool _hasCap;

        [Category("Appearance"), Description("wether this line has cap."), DefaultValue(true)]
        public bool HasCap
        {
            get
            {
                return this._hasCap;
            }
            set
            {
                if (value != this._hasCap)
                {
                    this._hasCap = value;
                    InvokeInvalidate(this._hasCap, value);
                }
            }
        }

        private int _rotationAngle;

        public event PropertyChangedEventHandler PropertyChanged;

        [Category("Appearance"), Description("rotation angle of this line."), DefaultValue(0d)]
        public int RotationAngle
        {
            get
            {
                return this._rotationAngle;
            }
            set
            {
                if (value != this._rotationAngle)
                {
                    this._rotationAngle = value;
                    InvokeInvalidate(this._rotationAngle, value);
                }
            }
        }

        private void InvokeInvalidate()
        {
            if (!IsHandleCreated)
                return;

            Rectangle rect = new Rectangle(Location, Size);

            try
            {
                this.Invoke((MethodInvoker)delegate { Parent.Invalidate(rect, true); });
            }
            catch { }
        }

        private void InvokeInvalidate(object obj, bool value)
        {
            if (!IsHandleCreated)
                return;

            try
            {
                this.Invoke((MethodInvoker)delegate { obj = value; });
            }
            catch { }
        }

        private void InvokeInvalidate(object obj, double value)
        {
            if (!IsHandleCreated)
                return;

            try
            {
                this.Invoke((MethodInvoker)delegate { obj = value; });
            }
            catch { }
        }

        public BaseLine()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            _lineColor = Color.Black;
            _penWidth = 2f;
            _rotationAngle = 0;
            _hasCap = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            double angle = (_rotationAngle / 180) * Math.PI;
            graphics.TranslateTransform(
                (ClientRectangle.Width + (float)(this.Height * Math.Sin(angle)) - (float)(this.Width * Math.Cos(angle))) / 2,
                (ClientRectangle.Height - (float)(this.Height * Math.Cos(angle)) - (float)(this.Width * Math.Sin(angle))) / 2);
            graphics.RotateTransform((float)_rotationAngle);
            switch (_shape)
            {
                case LineShape.Top:
                    _p2 = new Point(this.Width / 2, 0);
                    _p1 = new Point(this.Width / 2, this.Height);
                    break;
                case LineShape.Left:
                    _p2 = new Point(0, this.Height / 2);
                    _p1 = new Point(this.Width, this.Height / 2);
                    break;
                case LineShape.Right:
                    _p1 = new Point(0, this.Height / 2);
                    _p2 = new Point(this.Width, this.Height / 2);
                    break;
                case LineShape.Bottom:
                    _p1 = new Point(this.Width / 2, 0);
                    _p2 = new Point(this.Width / 2, this.Height);
                    break;
            }

            using (Pen p = new Pen(_lineColor, _penWidth))
            using (AdjustableArrowCap lineCap = new AdjustableArrowCap(4, 4, true))
            {
                if (HasCap)
                {
                    p.CustomEndCap = lineCap;
                }
                graphics.DrawLine(p, _p1, _p2);
            }
            graphics.ResetTransform();

        }

        protected override void OnResize(EventArgs e)
        {
            switch (_shape)
            {
                case LineShape.Top:
                    _p2 = new Point(this.Width / 2, 0);
                    _p1 = new Point(this.Width / 2, this.Height);
                    break;
                case LineShape.Left:
                    _p2 = new Point(0, this.Height / 2);
                    _p1 = new Point(this.Width, this.Height / 2);
                    break;
                case LineShape.Right:
                    _p1 = new Point(0, this.Height / 2);
                    _p2 = new Point(this.Width, this.Height / 2);
                    break;
                case LineShape.Bottom:
                    _p1 = new Point(this.Width / 2, 0);
                    _p2 = new Point(this.Width / 2, this.Height);
                    break;
            }
            base.OnResize(e);
        }

    }
}
