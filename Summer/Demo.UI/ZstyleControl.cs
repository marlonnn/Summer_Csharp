using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Demo.UI
{
    public partial class ZstyleControl : UserControl
    {
        private GraphicsPath graphicsPath;
        Pen p = new Pen(SystemColors.WindowFrame, 2);

        public ZstyleControl()
        {
            this.graphicsPath = new GraphicsPath();
            this.setGraphicsPath();
            this.Region = new Region(graphicsPath);
        }

        private void setGraphicsPath()
        {
            // add a Z style path
            int w = this.Width - 2;
            int h = this.Height - 2;

            this.graphicsPath.AddLine(
                new Point(2, 2), new Point(w, 2));
            this.graphicsPath.AddLine(
                new Point(w, 2), new Point(2, h));
            this.graphicsPath.AddLine(
                new Point(2, h), new Point(w, h));

            this.graphicsPath.Widen(p);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TranslateTransform(-1, -1);
            e.Graphics.DrawPath(p, graphicsPath);
            e.Graphics.ResetTransform();
        }
    }
}
