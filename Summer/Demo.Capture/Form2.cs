using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo.Capture
{
    public partial class Form2 : Form
    {
        private bool isMouseDown = false;
        private Rectangle rcSelect = new Rectangle();
        private Point pntStart;
        public Form2()
        {
            InitializeComponent();

            this.graphicOverlay1.Owner = this;
            this.graphicOverlay1.Paint += GraphicOverlay1_Paint;
            this.MouseDown += Form2_MouseDown;
            this.MouseMove += Form2_MouseMove;
            this.MouseUp += Form2_MouseUp;
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            // Start the snip on mouse down
            if (e.Button != MouseButtons.Left) return;
            pntStart = e.Location;
            rcSelect = new Rectangle(e.Location, new Size(0, 0));
            isMouseDown = true;
            this.Invalidate(true);
        }
        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            // Modify the selection on mouse move
            if (e.Button != MouseButtons.Left) return;
            int x1 = Math.Min(e.X, pntStart.X);
            int y1 = Math.Min(e.Y, pntStart.Y);
            int x2 = Math.Max(e.X, pntStart.X);
            int y2 = Math.Max(e.Y, pntStart.Y);
            rcSelect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            this.textBox1.Text = x1 + " " + y1;
            isMouseDown = true;
            this.Invalidate(true);
        }
        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            // Complete the snip on mouse-up
            if (rcSelect.Width <= 0 || rcSelect.Height <= 0) return;
            isMouseDown = false;
            this.Invalidate(true);
        }

        private void GraphicOverlay1_Paint(object sender, PaintEventArgs e)
        {
            // To draw relative to a control, use the Coordinates method to translate the control's location to form-relative coordinates.
            if (isMouseDown)
            {
                using (Pen pen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(pen, rcSelect);
                }
            }

        }

        public static Rectangle Coordinates(Control control)
        {
            Rectangle coordinates;
            Form form = (Form)control.TopLevelControl;

            if (control == form)
                coordinates = form.ClientRectangle;
            else
                coordinates = form.RectangleToClient(control.Parent.RectangleToScreen(control.Bounds));

            return coordinates;
        }
    }
}
