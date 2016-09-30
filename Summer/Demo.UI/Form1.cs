using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.UI
{
    public partial class Form1 : Form
    {

        public delegate void ClickDelegate(object sender, EventArgs e);

        public ClickDelegate SpectroscopeHandler;

        public ClickDelegate FilterHander;

        private ContextMenuStrip _ctxMenuStrip;
        private ToolStripMenuItem _toolStripMenuItem;
        public Form1()
        {
            InitializeComponent();
            SpectroscopeHandler += Form1_SpectroscopeHandler;
            _ctxMenuStrip = new ContextMenuStrip();
            if (components == null) components = new Container();
            this.ContextMenuStrip = new ContextMenuStrip(components);
            this.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

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
            _toolStripMenuItem.Text = "全反射镜";
            _toolStripMenuItem.CheckState = CheckState.Unchecked;
            menuStrip.Items.Add(_toolStripMenuItem);

            e.Cancel = false;
        }

        private void MenuStrip_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Form1_SpectroscopeHandler(object sender, EventArgs e)
        {
            _toolStripMenuItem = new ToolStripMenuItem();
            _toolStripMenuItem.Name = "_toolStripMenuItem";
            _toolStripMenuItem.Size = new Size(153, 22);
            _toolStripMenuItem.Text = "全反射镜";
            _toolStripMenuItem.CheckState = CheckState.Unchecked;
            _ctxMenuStrip.Items.Add(_toolStripMenuItem);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Invalidate(true);
        }
    }
}
