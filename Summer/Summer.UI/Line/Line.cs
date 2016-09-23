using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Summer.UI.Line
{
    [Designer(typeof(ShapeControlDesigner))]
    public class Line : UserControl, IShape, INotifyPropertyChanged
    {

        #region Fields

        private Point _p1;
        private Point _p2;
        private float _penWidth = 1.0f;
        private Color _lineColor = Color.Black;
        private int _edgeOffset = 5;
        private float _opacity = 1.0f;

        #endregion

        #region Properties

        [Browsable(false), Category("Layout"), Description("Start point of the line in control coordinates.")]
        public Point StartPoint
        {
            get { return _p1; }
            set
            {
                if (value != _p1)
                {
                    _p1 = value;
                    OnPropertyChanged("StartPoint");
                    RecalcSize();
                }
            }
        }

        [Browsable(false), Category("Layout"), Description("End point of the line in control coordinates.")]
        public Point EndPoint
        {
            get { return _p2; }
            set
            {
                if (value != _p2)
                {
                    _p2 = value;
                    OnPropertyChanged("EndPoint");
                    RecalcSize();
                }
            }
        }

        [Category("Appearance"), Description("Color of the line drawn."), DefaultValue(typeof(Color), "Black")]
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                if (value != _lineColor)
                {
                    _lineColor = value;
                    OnPropertyChanged("LineColor");
                    InvokeInvalidate();
                }
            }
        }

        [Category("Appearance"), Description("Width of the line drawn."), DefaultValue(1.0f)]
        public float LineWidth
        {
            get { return _penWidth; }
            set
            {
                if (value != _penWidth)
                {
                    _penWidth = value;
                    OnPropertyChanged("LineWidth");
                    InvokeInvalidate();
                }
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(50, 50);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x20;

                return cp;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public int PointCount
        {
            get { return 2; }
        }

        [Category("Appearance"), Description("Sets the opacity of the line color."), DefaultValue(1.0f)]
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                if (value != _opacity)
                {
                    if (value > 1.0f)
                        value = 1.0f;
                    if (value < 0.0f)
                        value = 0.0f;
                    _opacity = value;
                    OnPropertyChanged("Opacity");
                    InvokeInvalidate();
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler PointCountChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Construction / Deconstruction

        public Line()
        {
            _p1 = new Point(5, 5);
            _p2 = new Point(45, 45);
            //DoubleBuffered = true;
        }

        #endregion

        #region Public Methods

        public Point GetPoint(int i)
        {
            if (i == 0)
                return _p1;
            else if (i == 1)
                return _p2;
            else
                return Point.Empty;
        }

        public void SetPoint(int i, Point p)
        {
            if (i == 0)
                StartPoint = p;
            else if (i == 1)
                EndPoint = p;
        }

        #endregion

        #region Protected Methods

        protected override void OnResize(EventArgs e)
        {
            InvokeInvalidate();
            base.OnResize(e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color opacityColor = Color.FromArgb((int)(255 * _opacity), _lineColor);
            using (Pen p = new Pen(opacityColor, _penWidth))
            using (AdjustableArrowCap lineCap =
                new AdjustableArrowCap(4, 4, true))
            {
                // A triangle
                p.CustomEndCap = lineCap;
                //p.EndCap = LineCap.ArrowAnchor;
                e.Graphics.DrawLine(p, _p1, _p2);
            }

        }

        #endregion

        #region Private Methods

        private void RecalcSize()
        {
            AdjustTopEdge();
            AdjustBottomEdge();
            AdjustLeftEdge();
            AdjustRightEdge();

            InvokeInvalidate();

            base.OnResize(EventArgs.Empty);
        }

        private void InvokeInvalidate()
        {
            if (!IsHandleCreated)
                return;

            Rectangle rect = new Rectangle(Location, Size);

            try
            {
                //RecreateHandle(); Invalidate();
                this.Invoke((MethodInvoker)delegate { Parent.Invalidate(rect, true); });
            }
            catch { }
        }

        private void AdjustTopEdge()
        {
            //Find the top most point
            int minY = Math.Min(_p1.Y, _p2.Y);
            bool useP1 = false;

            if (_p1.Y < _p2.Y)
                useP1 = true;

            int adjust = minY - _edgeOffset;

            Top += adjust;

            if (useP1)
            {
                _p1.Y = _edgeOffset;
                _p2.Y -= adjust;
                Height -= adjust;
            }
            else
            {
                _p2.Y = _edgeOffset;
                _p1.Y -= adjust;
                Height -= adjust;
            }
        }

        private void AdjustBottomEdge()
        {
            int maxY = Math.Max(_p1.Y, _p2.Y);

            int height = maxY + _edgeOffset;

            Height = height;
        }

        private void AdjustLeftEdge()
        {
            int minX = Math.Min(_p1.X, _p2.X);
            bool useP1 = false;

            if (_p1.X < _p2.X)
                useP1 = true;

            int adjust = minX - _edgeOffset;

            Left += adjust;

            if (useP1)
            {
                _p1.X = _edgeOffset;
                _p2.X -= adjust;
                Width -= adjust;
            }
            else
            {
                _p2.X = _edgeOffset;
                _p1.X -= adjust;
                Width -= adjust;
            }
        }

        private void AdjustRightEdge()
        {
            int maxX = Math.Max(_p1.X, _p2.X);

            int width = maxX + _edgeOffset;

            Width = width;
        }

        #endregion

    }
}
