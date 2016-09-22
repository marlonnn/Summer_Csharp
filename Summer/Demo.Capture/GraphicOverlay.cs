using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo.Capture
{
    public partial class GraphicOverlay : Component
    {
        private Form _owner;
        public event EventHandler<PaintEventArgs> Paint;

        public Form Owner
        {
            get { return _owner; }
            set
            {
                // The owner form cannot be set to null.
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                // The owner form can only be set once.
                if (_owner != null)
                {
                    throw new InvalidOperationException();
                }
                this._owner = value;
                // Handle the form's Resize event.
                this._owner.Resize += new EventHandler(Form_Resize);

                // Handle the Paint event for each of the controls in the form's hierarchy.
                ConnectPaintEventHandlers(_owner);
            }
        }
        public GraphicOverlay()
        {
            InitializeComponent();
        }

        public GraphicOverlay(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            _owner.Invalidate(true);
        }

        private void ConnectPaintEventHandlers(Control control)
        {
            // Connect the paint event handler for this control.
            // Remove the existing handler first (if one exists) and replace it.
            control.Paint -= new PaintEventHandler(Control_Paint);
            control.Paint += new PaintEventHandler(Control_Paint);

            control.ControlAdded -= new ControlEventHandler(Control_ControlAdded);
            control.ControlAdded += new ControlEventHandler(Control_ControlAdded);

            // Recurse the hierarchy.
            foreach (Control child in control.Controls)
                ConnectPaintEventHandlers(child);
        }

        private void Control_ControlAdded(object sender, ControlEventArgs e)
        {
            // Connect the paint event handler for the new control.
            ConnectPaintEventHandlers(e.Control);
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            // As each control on the form is repainted, this handler is called.

            Control control = sender as Control;
            Point location;

            // Determine the location of the control's client area relative to the form's client area.
            if (control == _owner)
                // The form's client area is already form-relative.
                location = control.Location;
            else
            {
                // The control may be in a hierarchy, so convert to screen coordinates and then back to form coordinates.
                location = _owner.PointToClient(control.Parent.PointToScreen(control.Location));

                // If the control has a border shift the location of the control's client area.
                location += new Size((control.Width - control.ClientSize.Width) / 2, (control.Height - control.ClientSize.Height) / 2);
            }

            // Translate the location so that we can use form-relative coordinates to draw on the control.
            if (control != _owner)
                e.Graphics.TranslateTransform(-location.X, -location.Y);

            // Fire a paint event.
            OnPaint(sender, e);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            // Fire a paint event.
            // The paint event will be handled in Form1.graphicalOverlay1_Paint().

            if (Paint != null)
                Paint(sender, e);
        }
    }
}
