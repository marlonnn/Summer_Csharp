using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Capture
{
    public partial class Form1 : Form
    {
        private CopyScreen _copyScreen;
        private SnippingTool _snip;

        public Form1()
        {
            _copyScreen = new CopyScreen();
            //_snip = SnippingTool.Snipping();
            InitializeComponent();
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //_snip.MouseDown(e);
            //SnippingTool.Snip();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //_snip.MouseMove(e);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //_snip.MouseUp(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _copyScreen.GerScreenFormRectangle();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _copyScreen.GetScreenImage += _copyScreen_GetScreenImage;
        }

        private void _copyScreen_GetScreenImage(Image p_Image)
        {
            pictureBox1.Image = p_Image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SnippingTool.Snipping();
            //SnippingTool.Snip();
            //var bmp = SnippingTool.Snip();
            //if (bmp != null)
            //{
            //    // Do something with the bitmap
            //    //...
            //}
        }
    }
}
