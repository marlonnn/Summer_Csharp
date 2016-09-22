using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Mdi
{
    public partial class ChildForm : System.Windows.Forms.Form
    {
        public ChildForm()
        {
            InitializeComponent();
        }

        public ChildForm(Size size, int i)
        {
            InitializeComponent();
            this.Size = size;
            this.Text = i.ToString();
            this.MouseDown += ChildForm_MouseDown;
        }

        private void ChildForm_MouseDown(object sender, MouseEventArgs e)
        {
            var v1 = this.Top;
        }

        internal class NonClientInfo
        {
            public int CaptionTotalHeight = 26;   // force to 26 to fix in .Net 4.5 problem
            public int BottomBorder = 1;
            public int LeftBorder = 1;
            public int RightBorder = 1;

            public int TotalWidth
            {
                get { return LeftBorder + RightBorder; }
            }

            public int TotalHeight
            {
                get { return CaptionTotalHeight + BottomBorder; }
            }
        }
    }
}
