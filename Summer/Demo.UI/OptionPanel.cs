using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.UI
{
    public partial class OptionPanel : UserControl
    {
        public OptionPanel()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

        }

        private void pmt7_Load(object sender, EventArgs e)
        {

        }

        private void baseLine1_Load(object sender, EventArgs e)
        {

        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT 

        //        return cp;
        //    }
        //}

        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //    // Don't paint background
        //}
    }
}
