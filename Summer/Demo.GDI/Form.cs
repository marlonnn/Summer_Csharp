using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.GDI
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Graphics _g;

        public Form()
        {
            InitializeComponent();
        }

        private void form_Paint(object sender, PaintEventArgs e)
        {
            //_g = e.Graphics;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            //_g = e.Graphics;
            ////Rectangle rect = new Rectangle(50, 30, 100, 100);
            ////LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Red, Color.Yellow, LinearGradientMode.BackwardDiagonal);
            ////_g.FillRectangle(brush, rect);

            //Pen pen = new Pen(Color.Blue);
            //Rectangle rect = new Rectangle(50, 30, 100, 200);
            ////_g.DrawArc(pen, rect, 12, 84);

            //Point p1 = new Point(30, 30);
            //Point p2 = new Point(110, 110);
            ////_g.DrawLine(pen, p1, p2);
            ////_g.DrawEllipse(pen, rect);

            //Font fnt = new Font("Verdana", 16);
            //_g.DrawString("GDI+ World", fnt, new SolidBrush(Color.Brown), 14, 10);
        }

        private void Form_OnLoad(object sender, EventArgs e)
        {
            Application.Idle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs e)
        {
            _g = this.CreateGraphics();
            //Rectangle rect = new Rectangle(50, 30, 100, 100);
            //LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Red, Color.Yellow, LinearGradientMode.BackwardDiagonal);
            //_g.FillRectangle(brush, rect);

            Pen pen = new Pen(Color.Blue);
            Rectangle rect = new Rectangle(50, 30, 100, 200);
            //_g.DrawArc(pen, rect, 12, 84);

            Point p1 = new Point(30, 30);
            Point p2 = new Point(110, 110);
            //_g.DrawLine(pen, p1, p2);
            //_g.DrawEllipse(pen, rect);

            Font fnt = new Font("Verdana", 16);
            _g.DrawString("GDI+ World", fnt, new SolidBrush(Color.Brown), 14, 10);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.A)
            {
                AdvancedContextMenuStrip menu = new AdvancedContextMenuStrip();
                ToolStripMenuItem item = new ToolStripMenuItem("Hello &World");
                item.Click += new EventHandler(item_Click);
                menu.Items.Add(item);

                ToolStripMenuItem item2 = new ToolStripMenuItem("Some &Andriod");
                item2.Click += new EventHandler(item_Click);
                menu.Items.Add(item2);

                menu.Show(this, new Point(200, 200));
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hey!");
        }
    }

    public partial class AdvancedContextMenuStrip : ContextMenuStrip
    {
        public AdvancedContextMenuStrip()
        {
            //InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
                return true;

            return base.ProcessCmdKey(ref m, keyData);
        }
    }
}
