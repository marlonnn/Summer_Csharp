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
    public partial class PMT_back : BaseButton
    {
        public enum Light
        {
            VLS,
            BL4,
            RL1,
            VLS_BL4,
            VLS_RL1,
            BL4_RL1,
            ALL,
            SSC,
        }

        private Light _pmtLight;
        [Description("Light of the PMT?"), Category("PMT"), DefaultValue(typeof(PMT), "ALL")]
        public Light PMTLight
        {
            get
            {
                return this._pmtLight;
            }
            set
            {
                this._pmtLight = value;

                SetLightRectangle();
                InvokeInvalidate(value);
                this.Invalidate();
            }
        }

        private void SetLightRectangle()
        {
            switch (this._pmtLight)
            {
                case Light.VLS:
                    _vRectangle = new Rectangle(this.Width / 2 - 5, 5, 5, 5);
                    this._btnText = "VLS";
                    break;
                case Light.BL4:
                    _bRectangle = new Rectangle(this.Width / 2 - 5, 5, 5, 5);
                    this._btnText = "BL4";
                    break;
                case Light.RL1:
                    _rRectangle = new Rectangle(this.Width / 2 - 5, 5, 5, 5);
                    this._btnText = "RL1";
                    break;
                case Light.VLS_BL4:
                    _vRectangle = new Rectangle(this.Width / 2 - 20, 5, 5, 5);
                    _bRectangle = new Rectangle(this.Width / 2 + 10, 5, 5, 5);
                    this._btnText = "VLS BL4";
                    break;
                case Light.VLS_RL1:
                    _vRectangle = new Rectangle(this.Width / 2 - 20, 5, 5, 5);
                    _rRectangle = new Rectangle(this.Width / 2 + 10, 5, 5, 5);
                    this._btnText = "VLS RL1";
                    break;
                case Light.BL4_RL1:
                    _bRectangle = new Rectangle(this.Width / 2 - 20, 5, 5, 5);
                    _rRectangle = new Rectangle(this.Width / 2 + 10, 5, 5, 5);
                    this._btnText = "BL4 RL1";
                    break;
                case Light.ALL:
                    _vRectangle = new Rectangle(this.Width / 2 - 35, 5, 5, 5);
                    _bRectangle = new Rectangle(this.Width / 2 - 15, 5, 5, 5);
                    _rRectangle = new Rectangle(this.Width / 2 + 5, 5, 5, 5);
                    _yRectangle = new Rectangle(this.Width / 2 + 25, 5, 5, 5);
                    this._btnText = "VLS BL4 RL1 YL3";
                    break;
                case Light.SSC:
                    this._btnText = "SSC";
                    break;
            }
        }

        private void InvokeInvalidate(Light value)
        {
            if (!IsHandleCreated)
                return;
            try
            {
                this.Invoke((MethodInvoker)delegate { this._pmtLight = value; });
            }
            catch { }
        }

        private Pen  _vPen, _bPen, _rPen, _yPen;
        private SolidBrush _vSolidBrush, _bSolidBrush, _rSolidBrush, _ySolidBrush;
        private Rectangle _vRectangle, _bRectangle, _rRectangle, _yRectangle;

        private ToolStripMenuItem _toolStripMenuItem;
        //private States _State = States.Normal;
        public PMT_back() 
        {
            InitializeComponent();


            _vPen = new Pen(Color.Violet, 1);
            _bPen = new Pen(Color.Blue, 1);
            _rPen = new Pen(Color.Red, 1);
            _yPen = new Pen(Color.Yellow, 1);

            _solidBrush = new SolidBrush(Color.LightSkyBlue);

            _vSolidBrush = new SolidBrush(Color.Violet);
            _bSolidBrush = new SolidBrush(Color.Blue);
            _rSolidBrush = new SolidBrush(Color.Red);
            _ySolidBrush = new SolidBrush(Color.Yellow);

            //_vRectangle = new Rectangle(this.Width / 2 - 20, 5, 5, 5);
            //_bRectangle = new Rectangle(this.Width / 2 - 5, 5, 5, 5);
            //_rRectangle = new Rectangle(this.Width / 2 + 10, 5, 5, 5);

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

            switch (this._pmtLight)
            {
                case Light.VLS:
                    graphics.FillRectangle(_vSolidBrush, _vRectangle);
                    graphics.DrawRectangle(_vPen, _vRectangle);
                    break;
                case Light.BL4:
                    graphics.FillRectangle(_bSolidBrush, _bRectangle);
                    graphics.DrawRectangle(_bPen, _bRectangle);
                    break;
                case Light.RL1:
                    graphics.FillRectangle(_rSolidBrush, _rRectangle);
                    graphics.DrawRectangle(_rPen, _rRectangle);
                    break;
                case Light.VLS_BL4:
                    graphics.FillRectangle(_vSolidBrush, _vRectangle);
                    graphics.DrawRectangle(_vPen, _vRectangle);
                    graphics.FillRectangle(_bSolidBrush, _bRectangle);
                    graphics.DrawRectangle(_bPen, _bRectangle);
                    break;
                case Light.VLS_RL1:
                    graphics.FillRectangle(_vSolidBrush, _vRectangle);
                    graphics.DrawRectangle(_vPen, _vRectangle);
                    graphics.FillRectangle(_rSolidBrush, _rRectangle);
                    graphics.DrawRectangle(_rPen, _rRectangle);
                    break;
                case Light.BL4_RL1:
                    graphics.FillRectangle(_bSolidBrush, _bRectangle);
                    graphics.DrawRectangle(_bPen, _bRectangle);
                    graphics.FillRectangle(_rSolidBrush, _rRectangle);
                    graphics.DrawRectangle(_rPen, _rRectangle);
                    break;
                case Light.ALL:
                    graphics.FillRectangle(_vSolidBrush, _vRectangle);
                    graphics.DrawRectangle(_vPen, _vRectangle);
                    graphics.FillRectangle(_bSolidBrush, _bRectangle);
                    graphics.DrawRectangle(_bPen, _bRectangle);
                    graphics.FillRectangle(_rSolidBrush, _rRectangle);
                    graphics.DrawRectangle(_rPen, _rRectangle);
                    graphics.FillRectangle(_ySolidBrush, _yRectangle);
                    graphics.DrawRectangle(_yPen, _yRectangle);
                    break;
                case Light.SSC:
                    break;
            }
            //graphics.FillRectangle(_bSolidBrush, _bRectangle);
            //graphics.DrawRectangle(_bPen, _bRectangle);

            //graphics.FillRectangle(_vSolidBrush, _vRectangle);
            //graphics.DrawRectangle(_vPen, _vRectangle);

            //graphics.FillRectangle(_rSolidBrush, _rRectangle);
            //graphics.DrawRectangle(_rPen, _rRectangle);

            //For rotation, who about rotation?
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
