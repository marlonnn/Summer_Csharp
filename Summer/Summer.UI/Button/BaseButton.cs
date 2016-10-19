using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Summer.UI;
using System.Drawing.Drawing2D;

namespace Summer.UI.Button
{
    /// <summary>
    /// Custom base button
    /// </summary>
    public partial class BaseButton : UserControl
    {
        /// <summary>
        /// button change states when mouse enter, leave, up or click.
        /// </summary>
        protected Summer.UI.Attribute.States _State = Summer.UI.Attribute.States.Normal;

        /// <summary>
        /// invalidate region area when states changed
        /// </summary>
        protected Region _region;

        /// <summary>
        /// custom draw fill and inner button area
        /// </summary>
        protected Rectangle _fillRect, _innerRect;

        protected SolidBrush _solidBrush, _lightSolidBrush;

        protected Pen _pen, _clickPen;

        /// <summary>
        /// draw button text brush
        /// </summary>
        protected Brush _textBrush;

        protected double _rotationAngle;
        [Description("Button text rotation angle"), Category("Appearance"), DefaultValue(0D)]
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

        /// <summary>
        /// button text
        /// </summary>
        protected string _btnText;

        [Description("Button Display Text"), Category("Appearance")]
        public string ButtonText
        {
            get
            {
                return _btnText;
            }
            set
            {
                if (value != _btnText)
                {
                    _btnText = value;
                    this.Invalidate();
                    InvokeInvalidate(value);
                }
            }
        }

        /// <summary>
        /// invoke change button text
        /// </summary>
        /// <param name="value"></param>
        private void InvokeInvalidate(string value)
        {
            if (!IsHandleCreated)
                return;
            try
            {
                this.Invoke((MethodInvoker)delegate { ButtonText = value; });
            }
            catch { }
        }

        protected bool _Active = true;

        [Description("Enable the button?"), Category("Spectroscope")]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
            }
        }


        public BaseButton()
        {
            InitializeComponent();

            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
            _innerRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            _pen = new Pen(Color.Black, 1);
            _clickPen = new Pen(Color.Black, 3);

            _lightSolidBrush = new SolidBrush(Color.LightCyan);

            _textBrush = new SolidBrush(this.ForeColor);
        }

        /// <summary>
        /// subclass show override this method to draw custom button shape
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            //if (String.IsNullOrEmpty(ButtonText))
            //{
            //    ButtonText = this.Name;
            //}
            float width = graphics.MeasureString(ButtonText, this.Font).Width;
            float height = graphics.MeasureString(ButtonText, this.Font).Height;

            //For rotation, who about rotation?
            double angle = (_rotationAngle / 180) * Math.PI;
            graphics.TranslateTransform(
                (ClientRectangle.Width + (float)(height * Math.Sin(angle)) - (float)(width * Math.Cos(angle))) / 2,
                (ClientRectangle.Height - (float)(height * Math.Cos(angle)) - (float)(width * Math.Sin(angle))) / 2);
            graphics.RotateTransform((float)_rotationAngle);
            graphics.DrawString(ButtonText, this.Font, _textBrush, 0, 0);
            graphics.ResetTransform();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_Active)
            {
                _State = Summer.UI.Attribute.States.Normal;
                RegionInvalidate();
                base.OnMouseLeave(e);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (_Active)
            {
                _State = Summer.UI.Attribute.States.MouseOver;
                RegionInvalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_Active)
            {
                _State = Summer.UI.Attribute.States.MouseOver;
                RegionInvalidate();
                base.OnMouseUp(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_Active)
            {
                _State = Summer.UI.Attribute.States.Clicked;
                RegionInvalidate();
                base.OnMouseDown(e);

            }
        }

        protected override void OnClick(EventArgs e)
        {
            // prevent click when button is inactive
            if (_Active)
            {
                if (_State == Summer.UI.Attribute.States.Clicked)
                {
                    base.OnClick(e);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
        }

        /// <summary>
        /// invalidate to change button specified region background color when states changed.
        /// </summary>
        protected void RegionInvalidate()
        {
            if (_region == null)
            {
                this.Invalidate();
            }
            else
            {
                this.Invalidate(_region);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _clickPen.Dispose();

                _solidBrush.Dispose();
                _lightSolidBrush.Dispose();

                _pen.Dispose();
                _textBrush.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
