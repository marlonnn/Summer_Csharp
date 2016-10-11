﻿using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TranspControl
{
    [Designer(typeof(LightDesigner))]
    public class Light : UserControl
    {
        public enum LightType
        {
            VL,
            BL,
            RL,
            YL,
        }

        private LightType _type;
        [Description("Light type of the Light?"), Category("Light"), DefaultValue(typeof(LightType), "VL")]
        public LightType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                if (value != this._type)
                {
                    this._type = value;
                    switch (value)
                    {
                        case LightType.VL:
                            _pen = new Pen(Color.Violet, 1);
                            _solidBrush = new SolidBrush(Color.Violet);
                            break;
                        case LightType.BL:
                            _pen = new Pen(Color.Blue, 1);
                            _solidBrush = new SolidBrush(Color.Blue);
                            break;
                        case LightType.RL:
                            _pen = new Pen(Color.Red, 1);
                            _solidBrush = new SolidBrush(Color.Red);
                            break;
                        case LightType.YL:
                            _pen = new Pen(Color.Yellow, 1);
                            _solidBrush = new SolidBrush(Color.Yellow);
                            break;
                    }
                    InvokeInvalidate(value);
                    Invalidate();
                }
            }
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

        private string _lightText;
        private Font _font;
        private Rectangle _rectangle;

        private Pen _pen;
        private SolidBrush _solidBrush;

        [Description("Light text of the Light?"), Category("Light"), DefaultValue("")]
        public string LightText
        {
            get
            {
                return this._lightText;
            }
            set
            {
                if (value != this._lightText)
                {
                    this._lightText = value;
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
                this.Invoke((MethodInvoker)delegate { this._lightText = value; });
            }
            catch { }
        }

        private void InvokeInvalidate(LightType value)
        {
            if (!IsHandleCreated)
                return;
            try
            {
                this.Invoke((MethodInvoker)delegate { this._type = value; });
            }
            catch { }
        }

        protected Brush _textBrush;

        public Light()
        {
            //Set style for double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint, true);
            this.Size = new Size(18, 30);
            _textBrush = new SolidBrush(this.ForeColor);
            _font = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Regular);
            _rectangle = new Rectangle(new Point(6, 6), new Size(5, 5));

            _pen = new Pen(Color.Violet, 1);

            _solidBrush = new SolidBrush(Color.Violet);

            _lightText = "VL8";
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
            Rectangle pBounds = this.Bounds;
            pBounds.Inflate(pBounds.Width / 2, pBounds.Height / 2);
            this.Invalidate();
            if (this.Parent != null) this.Parent.Invalidate(pBounds, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ///////////////////////////////
            //         SETTINGS          //
            ///////////////////////////////

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

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

            graphics.FillRectangle(_solidBrush, _rectangle);
            graphics.DrawRectangle(_pen, _rectangle);

            graphics.DrawString(LightText, _font, _textBrush, 0, 15);
            ///////////////////////////////
            //       FREES MEMORY        //
            ///////////////////////////////

            brushColor.Dispose();
            bckColor.Dispose();
            pen.Dispose();

        }
    }

    internal class LightDesigner : ControlDesigner
    {
        private Light control;


        protected override void OnMouseDragBegin(int x, int y)
        {
            base.OnMouseDragBegin(x, y);
            control = (Light)(this.Control);
            control.drag = true;

        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            control = (Light)(this.Control);
            control.drag = false;

        }

    }

}
