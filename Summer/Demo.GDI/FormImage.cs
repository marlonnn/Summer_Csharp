using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.GDI
{
    public partial class FormImage : System.Windows.Forms.Form
    {
        public FormImage()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog opdl = new OpenFileDialog();
            //opdl.Filter = "All File|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
            opdl.Filter = "All Files|*.*|Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
            if (opdl.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(opdl.FileName);
                pictureBox1.Image = image;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //保存功能
            //string str;
            //Bitmap box1 = new Bitmap(pictureBox1.Image);
            //SaveFileDialog sfdlg = new SaveFileDialog();
            //sfdlg.Filter = "All Files|*.*|Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
            //sfdlg.ShowDialog();
            //str = sfdlg.FileName;
            //box1.Save(str);

            //复制与粘贴
            Clipboard.SetDataObject(pictureBox1.Image);

            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Bitmap))
            {
                pictureBox2.Image = (Bitmap)iData.GetData
                (DataFormats.Bitmap);
            }

        }
    }
}
