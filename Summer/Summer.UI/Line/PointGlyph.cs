using System.Windows.Forms.Design.Behavior;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Summer.UI.Line
{
    class PointGlyph : Glyph
    {

        #region Fields

        private IShape _shape;
        private int _pointIdx;
        private BehaviorService _behaviorSvc;
        private Control _baseControl;
        private int _glyphSize = 10;
        private Color _glyphFillColor = Color.White;
        private Color _glyphOutlineColor = Color.Black;
        private int _glyphCornerRadius = 4;

        #endregion

        #region Properties

        public override Rectangle Bounds
        {
            get
            {
                Point p = _shape.GetPoint(_pointIdx);

                p = _behaviorSvc.MapAdornerWindowPoint(_baseControl.Handle, p);

                int x = p.X - (_glyphSize / 2);
                int y = p.Y - (_glyphSize / 2);

                return new Rectangle(x, y, _glyphSize, _glyphSize);
            }
        }

        #endregion

        #region Construction / Deconstruction

        public PointGlyph(BehaviorService behaviorSvc, IShape shape, int pointIdx, Control baseControl)
            : base(new ShapeGlyphBehavior(shape, pointIdx))
        {
            _shape = shape;
            _pointIdx = pointIdx;
            _behaviorSvc = behaviorSvc;
            _baseControl = baseControl;
        }

        #endregion

        #region Public Methods

        public override Cursor GetHitTest(Point p)
        {
            Rectangle hitBounds = Bounds;
            hitBounds.Inflate(4, 4);

            if (hitBounds.Contains(p))
                return Cursors.Hand;

            return null;
        }

        public override void Paint(PaintEventArgs pe)
        {
            Rectangle glyphRect = Bounds;

            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //First draw the fill...
            using (SolidBrush sb = new SolidBrush(_glyphFillColor))
            {
                pe.Graphics.FillRoundedRectangle(sb, glyphRect, _glyphCornerRadius);
            }

            //And then  the outline
            using (Pen p = new Pen(_glyphOutlineColor))
            {
                pe.Graphics.DrawRoundedRectangle(p, glyphRect, _glyphCornerRadius);
            }
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
