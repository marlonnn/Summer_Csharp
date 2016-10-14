using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Summer.UI.Button;

namespace Demo.UI
{
    [Designer(typeof(SpectroDesigner))]
    public class Spectro : BaseButton
    {
        public bool drag = false;
        private Image backImage = null;
        private Color fillColor = Color.White;
        private Color backColor = Color.Transparent;
        private Color transpKey = Color.White;
        private int opacity = 100;
        private int lineWidth = 2;
        private int alpha;
        private bool glassMode = true;

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

        public Spectro()
        {
            InitializeComponent();
            //Set style for double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint, true);

            _fillRect = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
            _solidBrush = new SolidBrush(Color.Pink);
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
                if (opacity > 100) { opacity = 100; }
                else if (opacity < 1) { opacity = 0; }
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
            pBounds.Inflate(pBounds.Width / 2, pBounds.Height / 2);
            this.Invalidate();

            if (this.Parent != null) this.Parent.Invalidate(pBounds, true);

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Width = this.Height;
            //if (this.label1 != null)
            //{
            //    this.label1.Location = new Point(this.Width / 2 + 8, this.Width / 2 - 20);
            //}
            Rectangle pBounds = this.Bounds;
            pBounds.Inflate(pBounds.Width / 2, pBounds.Height / 2);
            this.Invalidate();
            if (this.Parent != null) this.Parent.Invalidate(pBounds, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ///////////////////////////////
            //         SETTINGS          //
            ///////////////////////////////

            Graphics graphics = e.Graphics;
            //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //graphics.CompositingQuality = CompositingQuality.GammaCorrected;

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

            DrawTrapezoid(graphics);

            float width = graphics.MeasureString(ButtonText, this.Font).Width;
            float height = graphics.MeasureString(ButtonText, this.Font).Height;
            graphics.DrawString(ButtonText, this.Font, _textBrush, this.Width / 2 + 8, this.Width / 2 - 20);
            ///////////////////////////////
            //       FREES MEMORY        //
            ///////////////////////////////

            brushColor.Dispose();
            bckColor.Dispose();
            pen.Dispose();
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

        private PointF[] CurvePoint(Shape shape)
        {
            PointF point1 = new PointF();
            PointF point2 = new PointF();
            PointF point3 = new PointF();
            PointF point4 = new PointF();
            switch (shape)
            {
                case Shape.TOP:
                    point1 = new PointF(28, 28);
                    point2 = new PointF(this.Width - 28, this.Width - 28);
                    point3 = new PointF(this.Width - 28, this.Width * 7 / 8 - 28);
                    point4 = new PointF(this.Width / 8 + 28, 28);
                    break;
                case Shape.BOTTOM:
                    point1 = new PointF(28, 28);
                    point2 = new PointF(28, this.Width / 8 + 28);
                    point3 = new PointF(this.Width * 7 / 8 - 28, this.Width - 28);
                    point4 = new PointF(this.Width - 28, this.Width - 28);
                    break;
                default:
                    point1 = new PointF(28, 28);
                    point2 = new PointF(this.Width - 28, this.Width - 28);
                    point3 = new PointF(this.Width - 28, this.Width * 7 / 8 - 28);
                    point4 = new PointF(this.Width / 8 + 28, 28);
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
            if (Program.MainForm.SpectroscopeHandler != null)
            {
                Program.MainForm.SpectroscopeHandler(this, e);
            }
            base.OnClick(e);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 
            // Spectroscope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ButtonText = "Spectro";
            this.Name = "Spectro";
            this.Size = new System.Drawing.Size(85, 85);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }


    internal class SpectroDesigner : ControlDesigner
    {
        private Spectro control;


        protected override void OnMouseDragBegin(int x, int y)
        {
            base.OnMouseDragBegin(x, y);
            control = (Spectro)(this.Control);
            control.drag = true;

        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            control = (Spectro)(this.Control);
            control.drag = false;

        }

    }

}
