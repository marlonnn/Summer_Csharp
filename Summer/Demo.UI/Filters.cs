using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.UI
{
    public partial class Filters : UserControl
    {
        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        private enum States
        {
            Normal,
            MouseOver,
            Clicked
        }

        private Rectangle _fillRect;
        private Rectangle _innerRect;
        private Pen _pen, _clickPen;
        private SolidBrush _lightSolidBrush, _solidBrush;

        private double _rotationAngle;
        [Description("Rotation Angle"), Category("Appearance")]
        public double RotationAngle
        {
            get
            {
                return _rotationAngle;
            }
            set
            {

                _rotationAngle = value;
                this.Invalidate();
            }
        }

        private string _filterText;

        [Description("Filter Display Text"), Category("Appearance")]
        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                if (value != _filterText)
                {
                    _filterText = value;
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
                this.Invoke((MethodInvoker)delegate { this.FilterText = value; });
            }
            catch { }
        }

        private Orientation _orientation;

        [Description("Filter Orientation"), Category("Appearance"), DefaultValue(typeof(Orientation), "Horizontal")]
        public Orientation Orientations
        {
            get
            {
                return this._orientation;
            }
            set
            {
                this._orientation = value;
                InvokeInvalidate();
            }
        }

        private void InvokeInvalidate()
        {
            switch (_orientation)
            {
                case Orientation.Horizontal:
                    this.Size = new System.Drawing.Size(this.Height, this.Width);
                    RotationAngle = 0D;
                    break;
                case Orientation.Vertical:
                    this.Size = new System.Drawing.Size(this.Height, this.Width);
                    RotationAngle = -90D;
                    break;
            }

            this.Invalidate();
        }

        // default values
        private bool _Active = true;

        [Description("Enable the button?"), Category("Filters")]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
                this.Invalidate();
            }
        }

        private float _textWidth;

        private float _textHeight;

        public float TextWidth
        {
            get
            {
                return this._textWidth;
            }
            set
            {
                this._textWidth = value;
            }
        }

        public float TextHeight
        {
            get
            {
                return this._textHeight;
            }
            set
            {
                this._textHeight = value;
            }
        }

        private States _State = States.Normal;

        public Filters()
        {
            InitializeComponent();
            switch (_orientation)
            {
                case Orientation.Horizontal:
                    _rotationAngle = 0d;
                    this.Size = new System.Drawing.Size(Math.Max(this.Width, this.Height), 
                        Math.Min(this.Width, this.Height));
                    break;
                case Orientation.Vertical:
                    _rotationAngle = -90D;
                    this.Size = new System.Drawing.Size(Math.Min(this.Width, this.Height),
                        Math.Max(this.Width, this.Height));
                    break;
                default:
                    _rotationAngle = -0D;
                    this.Size = new System.Drawing.Size(Math.Max(this.Width, this.Height),
                        Math.Min(this.Width, this.Height));
                    break;
            }
            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
            _innerRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            _pen = new Pen(Color.Black, 1);
            _clickPen = new Pen(Color.Black, 3);
            _solidBrush = new SolidBrush(Color.Orange);
            _lightSolidBrush = new SolidBrush(Color.LightCyan);
        }

        protected override void OnLoad(EventArgs e)
        {
            //FilterText = this.Name;
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            if (_Active)
            {
                _State = States.Normal;
                this.Invalidate();
                base.OnMouseLeave(e);
            }
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            if (_Active)
            {
                _State = States.MouseOver;
                this.Invalidate();
                base.OnMouseEnter(e);
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (_Active)
            {
                _State = States.MouseOver;
                this.Invalidate();
                base.OnMouseUp(e);
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (_Active)
            {
                _State = States.Clicked;
                this.Invalidate();
                base.OnMouseDown(e);
            }
        }

        protected override void OnClick(System.EventArgs e)
        {
            // prevent click when button is inactive
            if (_Active)
            {
                if (_State == States.Clicked)
                {
                    base.OnClick(e);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Brush textBrush = new SolidBrush(this.ForeColor);

            //Getting the width and height of the text, which we are going to write
            if (String.IsNullOrEmpty(FilterText))
            {
                FilterText = this.Name;
            }
            float width = graphics.MeasureString(FilterText, this.Font).Width;
            float height = graphics.MeasureString(FilterText, this.Font).Height;

            if (_Active)
            {
                switch (_State)
                {
                    case States.Normal:
                        graphics.FillRectangle(_solidBrush, _fillRect);
                        graphics.DrawRectangle(_pen, _innerRect);
                        break;
                    case States.MouseOver:
                        graphics.FillRectangle(_lightSolidBrush, _fillRect);
                        graphics.DrawRectangle(_pen, _innerRect);
                        break;
                    case States.Clicked:
                        graphics.FillRectangle(_lightSolidBrush, _fillRect);
                        graphics.DrawRectangle(_clickPen, _innerRect);
                        break;
                }
            }

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    _textWidth = graphics.MeasureString(FilterText, this.Font).Width;
                    _textHeight = graphics.MeasureString(FilterText, this.Font).Height;
                    break;
                case Orientation.Vertical:
                    _textHeight = graphics.MeasureString(FilterText, this.Font).Width;
                    _textWidth = graphics.MeasureString(FilterText, this.Font).Height;
                    break;
                default:
                    _textHeight = graphics.MeasureString(FilterText, this.Font).Width;
                    _textWidth = graphics.MeasureString(FilterText, this.Font).Height;
                    break;
            }

            //For rotation, who about rotation?
            double angle = (_rotationAngle / 180) * Math.PI;
            graphics.TranslateTransform(
                (ClientRectangle.Width + (float)(height * Math.Sin(angle)) - (float)(width * Math.Cos(angle))) / 2,
                (ClientRectangle.Height - (float)(height * Math.Cos(angle)) - (float)(width * Math.Sin(angle))) / 2);
            graphics.RotateTransform((float)_rotationAngle);
            graphics.DrawString(FilterText, this.Font, textBrush, 0, 0);
            graphics.ResetTransform();
        }

        protected override void OnResize(EventArgs e)
        {
            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
            _innerRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
