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
    public partial class PMT : UserControl
    {
        private enum States
        {
            Normal,
            MouseOver,
            Clicked
        }

        private Rectangle _fillRect, _innerRect;
        private Pen _pen, _vPen, _bPen, _rPen, _clickPen;
        private SolidBrush _solidBrush, _vSolidBrush, _bSolidBrush, _rSolidBrush, _lightSolidBrush;
        private Rectangle _vRectangle, _bRectangle, _rRectangle;

        // default values
        private bool _Active = true;

        [Description("Enable the button?"), Category("PMT")]
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

        private string _pmtText;

        [Description("PMT Display Text"), Category("Appearance")]
        public string PMTText
        {
            get
            {
                return _pmtText;
            }
            set
            {
                if (value != _pmtText)
                {
                    _pmtText = value;
                    this.Invalidate();
                    InvokeInvalidate(value);
                }
            }
        }

        private void InvokeInvalidate(string value)
        {
            if (!IsHandleCreated)
                return;
            try
            {
                this.Invoke((MethodInvoker)delegate { this.orientedTextLabel1.Text = value; });
            }
            catch { }
        }

        private States _State = States.Normal;
        public PMT()
        {
            InitializeComponent();

            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
            _innerRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            _pen = new Pen(Color.Black, 1);

            _vPen = new Pen(Color.Violet, 1);
            _bPen = new Pen(Color.Blue, 1);
            _rPen = new Pen(Color.Red, 1);
            _clickPen = new Pen(Color.Black, 3);

            _solidBrush = new SolidBrush(Color.LightSkyBlue);
            _lightSolidBrush = new SolidBrush(Color.LightCyan);

            _vSolidBrush = new SolidBrush(Color.Violet);
            _bSolidBrush = new SolidBrush(Color.Blue);
            _rSolidBrush = new SolidBrush(Color.Red);

            _vRectangle = new Rectangle(this.Width / 2 - 20, 5, 5, 5);
            _bRectangle = new Rectangle(this.Width / 2 - 5, 5, 5, 5);
            _rRectangle = new Rectangle(this.Width / 2 + 10, 5, 5, 5);

        }

        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
            //PMTText = this.Name;
            this.orientedTextLabel1.Text = PMTText;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (String.IsNullOrEmpty(PMTText))
            {
                PMTText = this.Name;
            }
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

            graphics.FillRectangle(_bSolidBrush, _bRectangle);
            graphics.DrawRectangle(_bPen, _bRectangle);

            graphics.FillRectangle(_vSolidBrush, _vRectangle);
            graphics.DrawRectangle(_vPen, _vRectangle);

            graphics.FillRectangle(_rSolidBrush, _rRectangle);
            graphics.DrawRectangle(_rPen, _rRectangle);
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


        protected override void OnResize(EventArgs e)
        {
            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _vSolidBrush.Dispose();
                _bSolidBrush.Dispose();
                _rSolidBrush.Dispose();

                _vPen.Dispose();
                _bPen.Dispose();
                _rPen.Dispose();
                _clickPen.Dispose();

                _solidBrush.Dispose();
                _lightSolidBrush.Dispose();

                _pen.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
