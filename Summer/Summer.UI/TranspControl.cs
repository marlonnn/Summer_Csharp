using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Summer.UI
{
    [Designer(typeof(TranspControlDesigner))]
    public class TranspControl: UserControl
    {
        public enum LineShape
        {
            Top,
            Left,
            Left_up,
            Left_down,
            Right,
            Bottom
        }

        public bool drag = false;
        private Image backImage = null;
        private Color fillColor = Color.White;
        private Color backColor = Color.Transparent;
        private Color transpKey = Color.White;
		private int opacity = 100;
        private int lineWidth = 2;
        private int alpha;
        private bool glassMode = true;

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

        public TranspControl()
		{
            //Set style for double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint, true);
            _lineColor = Color.Black;
            _penWidth = 2f;
            _rotationAngle = 0;
            _hasCap = true;
        }


        [Browsable(false)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        public Image BackImage
        {
            get { return this.backImage; }
            set
            {
                this.backImage = value;
                this.Invalidate();
            }
        }

        public Color TranspKey
        {
            get { return this.transpKey; }
            set
            {
                this.transpKey = value;
                this.Invalidate();
            }
        }
		
        public Color GlassColor
        {
            get { return this.backColor; }
            set
            {
                this.backColor = value; 
                this.Invalidate();
		    }
		}

        public bool GlassMode
        {
            get { return this.glassMode; }
            set
            {
                this.glassMode = value;
                this.Invalidate();
            }
        }

		public Color FillColor
		{
			get { return this.fillColor; }
			set
			{
				this.fillColor = value;
                this.Invalidate();
			}
		}

        public int LineWidth
        {
            get { return this.lineWidth; }
            set
            {
                this.lineWidth = value;
                this.Invalidate();
            }
        }

		public int Opacity
		{
			get
			{
				if (opacity > 100) {opacity = 100;}
				else if (opacity < 1) {opacity = 0;}
				return this.opacity;
			}
			set
			{
				this.opacity = value;
                this.Invalidate();
			}
		}
      
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;

            if (Parent != null && !drag)
            {
                BackColor = Color.Transparent;
                int index = Parent.Controls.GetChildIndex(this);

                for (int i = Parent.Controls.Count - 1; i > index; i--)
                {
                    Control c = Parent.Controls[i];
                    if (c.Bounds.IntersectsWith(Bounds) && c.Visible)
                    {
                        Bitmap bmp = new Bitmap(c.Width, c.Height, g);
                        c.DrawToBitmap(bmp, c.ClientRectangle);

                        g.TranslateTransform(c.Left - Left, c.Top - Top);
                        g.DrawImageUnscaled(bmp, Point.Empty);
                        g.TranslateTransform(Left - c.Left, Top - c.Top);
                        bmp.Dispose();
                    }
                }
            }
            else
            {
                g.Clear(Parent.BackColor);
                g.FillRectangle(new SolidBrush(Color.FromArgb(opacity * 255 / 100, GlassColor)),
                                               this.ClientRectangle);
            }

            if (BackImage != null && GlassMode)
            {
                Bitmap image = new Bitmap(BackImage);
                image.MakeTransparent(TranspKey);

                float a = (float)opacity / 100.0f;

                float[][] mtxItens = {
                new float[] {1,0,0,0,0},
                new float[] {0,1,0,0,0},
                new float[] {0,0,1,0,0},
                new float[] {0,0,0,a,0},
                new float[] {0,0,0,0,1}};
                ColorMatrix colorMatrix = new ColorMatrix(mtxItens);

                ImageAttributes imgAtb = new ImageAttributes();
                imgAtb.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

                g.DrawImage(
                        image,
                        ClientRectangle,
                        0.0f,
                        0.0f,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel,
                        imgAtb);
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            Rectangle pBounds = this.Bounds;
            pBounds.Inflate(pBounds.Width/2, pBounds.Height/2);
            this.Invalidate();
            if (this.Parent != null) this.Parent.Invalidate(pBounds, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Rectangle pBounds = this.Bounds;
            pBounds.Inflate(pBounds.Width/2, pBounds.Height/2);
            this.Invalidate();
            if (this.Parent != null) this.Parent.Invalidate(pBounds, true);
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
        }
		
		protected override void OnPaint(PaintEventArgs e)
		{
            base.OnPaint(e);
        
            ///////////////////////////////
            //         SETTINGS          //
            ///////////////////////////////

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.GammaCorrected;

            RectangleF bounds = this.ClientRectangle;
            alpha = (opacity * 255) / 100;
         
            float penWidth = (float)LineWidth;
            Pen pen = new Pen(Color.FromArgb(alpha, ForeColor), penWidth);
            pen.Alignment = PenAlignment.Center;

            Brush brushColor = new SolidBrush(Color.FromArgb(alpha, FillColor));
            Brush bckColor = new SolidBrush(Color.FromArgb(alpha, GlassColor));


            ///////////////////////////////
            //    DRAW YOUR SHAPE HERE   //
            ///////////////////////////////

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
                case LineShape.Left_up:
                    _p2 = new Point(0, 0);
                    _p1 = new Point(this.Width - 2, this.Height - 2);
                    break;
                case LineShape.Left_down:
                    _p2 = new Point(0, this.Height - 2);
                    _p1 = new Point(this.Width, 0);
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

            ///////////////////////////////
            //       FREES MEMORY        //
            ///////////////////////////////

            brushColor.Dispose();
            bckColor.Dispose();
			pen.Dispose();

		}
    }

    internal class TranspControlDesigner : ControlDesigner
    {
        private TranspControl control;


        protected override void OnMouseDragBegin(int x, int y)
        {
            base.OnMouseDragBegin(x, y);
            control = (TranspControl)(this.Control);
            control.drag = true;
           
        }
      
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            control = (TranspControl)(this.Control);
            control.drag = false;
           
      }
   
   }

}
