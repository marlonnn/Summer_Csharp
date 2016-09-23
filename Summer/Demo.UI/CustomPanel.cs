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
    public partial class CustomPanel : UserControl
    {
        public CustomPanel()
        {
            InitializeComponent();
            this.spectroscope1.ShapeType = Spectroscope.Shape.BOTTOM;
            this.spectroscope3.ShapeType = Spectroscope.Shape.BOTTOM;
            this.spectroscope5.ShapeType = Spectroscope.Shape.BOTTOM;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

        }

        private void pmt7_Load(object sender, EventArgs e)
        {

        }
    }
}
