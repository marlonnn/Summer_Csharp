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
    public partial class ProgressPanel : UserControl
    {
        public ProgressPanel()
        {
            InitializeComponent();
        }

        public ProgressBar ProgressBar { get { return this.progressBar; } }

        public Label LabelInfo { get { return this.lblInfo; } }

        public void IsVisible (bool visiable)
        {
            this.lblInfo.Visible = visiable;
            this.progressBar.Visible = visiable;
        }
    }
}
