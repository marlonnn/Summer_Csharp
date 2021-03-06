﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Summer.UI.Button;

namespace Demo.UI
{
    public partial class Filters : BaseButton
    {
        public enum Orientation
        {
            Horizontal,
            Vertical
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

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            if (_Active)
            {
                switch (_State)
                {
                    case Summer.UI.Attribute.States.Normal:
                        graphics.FillRectangle(_solidBrush, _fillRect);
                        graphics.DrawRectangle(_pen, _innerRect);
                        break;
                    case Summer.UI.Attribute.States.MouseOver:
                        graphics.FillRectangle(_lightSolidBrush, _fillRect);
                        graphics.DrawRectangle(_pen, _innerRect);
                        break;
                    case Summer.UI.Attribute.States.Clicked:
                        graphics.FillRectangle(_lightSolidBrush, _fillRect);
                        graphics.DrawRectangle(_clickPen, _innerRect);
                        break;
                }
            }
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
            _innerRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
