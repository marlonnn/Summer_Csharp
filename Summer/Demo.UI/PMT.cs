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
        private Pen  _vPen, _bPen, _rPen;
        private SolidBrush _vSolidBrush, _bSolidBrush, _rSolidBrush;
        private Rectangle _vRectangle, _bRectangle, _rRectangle;

        private ToolStripMenuItem _toolStripMenuItem;
        //private States _State = States.Normal;
        public PMT() 
        {
            InitializeComponent();

            _vPen = new Pen(Color.Violet, 1);
            _bPen = new Pen(Color.Blue, 1);
            _rPen = new Pen(Color.Red, 1);

            _solidBrush = new SolidBrush(Color.LightSkyBlue);

            _vSolidBrush = new SolidBrush(Color.Violet);
            _bSolidBrush = new SolidBrush(Color.Blue);
            _rSolidBrush = new SolidBrush(Color.Red);

            _vRectangle = new Rectangle(this.Width / 2 - 20, 5, 5, 5);
            _bRectangle = new Rectangle(this.Width / 2 - 5, 5, 5, 5);
            _rRectangle = new Rectangle(this.Width / 2 + 10, 5, 5, 5);

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
            //tsi = new ToolStripMenuItem(Resources.StrPaste);

            //tsi.Enabled = ((ReportForm)this.ParentForm).ReportOperateManage.CanBePaste(((ReportForm)this.ParentForm).Report.IsSpecimenReport);
            //tsi.Click += new EventHandler(Paste);
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
            if (String.IsNullOrEmpty(ButtonText))
            {
                ButtonText = this.Name;
            }
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

            graphics.FillRectangle(_bSolidBrush, _bRectangle);
            graphics.DrawRectangle(_bPen, _bRectangle);

            graphics.FillRectangle(_vSolidBrush, _vRectangle);
            graphics.DrawRectangle(_vPen, _vRectangle);

            graphics.FillRectangle(_rSolidBrush, _rRectangle);
            graphics.DrawRectangle(_rPen, _rRectangle);

            //For rotation, who about rotation?
            double angle = (_rotationAngle / 180) * Math.PI;
            graphics.TranslateTransform(
                (ClientRectangle.Width + (float)(height * Math.Sin(angle)) - (float)(width * Math.Cos(angle))) / 2,
                (ClientRectangle.Height - (float)(height * Math.Cos(angle)) - (float)(width * Math.Sin(angle))) / 2 + 5);
            graphics.RotateTransform((float)_rotationAngle);
            graphics.DrawString(ButtonText, this.Font, _textBrush, 0, 0);
            graphics.ResetTransform();
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
            _vRectangle = new Rectangle(this.Width / 2 - 20, 5, 5, 5);
            _bRectangle = new Rectangle(this.Width / 2 - 5, 5, 5, 5);
            _rRectangle = new Rectangle(this.Width / 2 + 10, 5, 5, 5);

            _fillRect = new Rectangle(0, 0, this.Width, this.Height);
            _innerRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _vSolidBrush.Dispose();
                _bSolidBrush.Dispose();
                _rSolidBrush.Dispose();

                _vPen.Dispose();
                _bPen.Dispose();
                _rPen.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
