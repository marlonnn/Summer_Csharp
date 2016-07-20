using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.MiniDump
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread;
            thread = new Thread(new ThreadStart(ThreadMethod));
            thread.Start();
        }

        private void ThreadMethod()
        {
            throw new Exception("From new thread");
        }
    }
}
