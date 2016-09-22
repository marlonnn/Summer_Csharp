using System;
using System.Windows.Forms.Design.Behavior;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;

namespace Summer.UI.Line
{
    class ShapeControlDesigner : ControlDesigner
    {

        #region Fields

        private Adorner _shapeAdorner;

        private string[] _remProperties = new string[] { "AccessibleDescription", "AccessibleRole",
            "AccessibleName", "BackColor", "BackgroundImage", "BackgroundImageLayout", "Cursor",
            "Font", "ForeColor", "RightToLeft", "Text", "UseWaitCursor", "AllowDrop", "Enabled",
            "ImeMode", "Anchor", "Dock", "Margin", "Padding", "MaximumSize", "MinimumSize",
            "CausesValidation" };

        private ISelectionService _selectionSvc;

        #endregion

        #region Properties

        public override bool ParticipatesWithSnapLines
        {
            get
            {
                return false;
            }
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return SelectionRules.Moveable |
                    SelectionRules.Visible;
            }
        }

        #endregion

        #region Construction / Deconstruction

        protected override void Dispose(bool disposing)
        {
            BehaviorService b = BehaviorService;

            if (b != null && b.Adorners.Contains(_shapeAdorner))
                b.Adorners.Remove(_shapeAdorner);

            base.Dispose(disposing);
        }

        #endregion

        #region Public Methods

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            _selectionSvc = GetService(typeof(ISelectionService)) as ISelectionService;

            _selectionSvc.SelectionChanged += new EventHandler(SelectionSvc_SelectionChanged);

            Control.Resize += new EventHandler(Control_Resize);

            IShape shape = Control as IShape;

            if (shape != null)
                shape.PointCountChanged += new EventHandler(Shape_PointCountChanged);

            RecreateAdorner();
        }

        #endregion

        #region Protected Methods

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            foreach (string propName in _remProperties)
                properties.Remove(propName);
        }

        #endregion

        #region Private Methods

        private void Shape_PointCountChanged(object sender, EventArgs e)
        {
            RecreateAdorner();
        }

        private void Control_Resize(object sender, EventArgs e)
        {
            BehaviorService.SyncSelection();
        }

        private void SelectionSvc_SelectionChanged(object sender, EventArgs e)
        {
            if (_selectionSvc.PrimarySelection == Control && _selectionSvc.SelectionCount == 1)
                _shapeAdorner.Enabled = true;
            else
                _shapeAdorner.Enabled = false;
        }

        private void RecreateAdorner()
        {
            if (_shapeAdorner != null)
            {
                _shapeAdorner.Glyphs.Clear();
            }
            else
            {
                _shapeAdorner = new Adorner();
                BehaviorService.Adorners.Add(_shapeAdorner);
            }

            IShape shape = Control as IShape;

            if (shape == null)
                return;

            for (int i = 0; i < shape.PointCount; i++)
            {
                _shapeAdorner.Glyphs.Add(new PointGlyph(BehaviorService, shape, i, Control));
            }

        }

        #endregion

    }
}
