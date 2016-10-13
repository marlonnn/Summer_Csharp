using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Frames
{
    public partial class LoadPanel : UserControl
    {
        public LoadPanel()
        {
            InitializeComponent();
        }

        public ProgressBar LoadProgressBar
        {
            get { return this.progressBar; }
        }
    }
}
