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
using Summer.UI.Button;

namespace Demo.UI
{
    public partial class PMT : BaseButton
    {
        private Font _font;

        private ToolStripMenuItem _toolStripMenuItem;
        //private States _State = States.Normal;
        public PMT() 
        {
            InitializeComponent();

            _font = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Regular);

            _solidBrush = new SolidBrush(Color.LightSkyBlue);

            if (components == null) components = new Container();
            this.ContextMenuStrip = new ContextMenuStrip(components);
            this.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var v1 = sender as PMT;
            ContextMenuStrip menuStrip = sender as ContextMenuStrip;
            if (menuStrip == null) return;
            menuStrip.Items.Clear();
            //ToolStripMenuItem tsi;
            _toolStripMenuItem = new ToolStripMenuItem();
            _toolStripMenuItem.Name = "_toolStripMenuItem";
            _toolStripMenuItem.Size = new Size(153, 22);
            _toolStripMenuItem.Text = "None";
            _toolStripMenuItem.CheckState = CheckState.Unchecked;
            menuStrip.Items.Add(_toolStripMenuItem);

            e.Cancel = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            //this.orientedTextLabel1.Text = PMTText;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            float width = graphics.MeasureString(ButtonText, this.Font).Width;
            float height = graphics.MeasureString(ButtonText, this.Font).Height;

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


        protected override void OnResize(EventArgs e)
        {
            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
            _innerRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
