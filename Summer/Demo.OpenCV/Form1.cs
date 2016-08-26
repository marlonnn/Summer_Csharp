﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.OpenCV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                //string winName = "Hello word";
                //CvInvoke.NamedWindow(winName);
                //using (Mat img = new Mat(200, 400, Emgu.CV.CvEnum.DepthType.Cv8U, 3))
                //{
                //    CvInvoke.PutText(img, "Hello world",
                //        new System.Drawing.Point(10, 80),
                //        FontFace.HersheyComplex,
                //        1.0, new Bgr(0, 255, 0).MCvScalar);
                //    //Show the image using ImageViewer from Emgu.CV.UI
                //    ImageViewer.Show(img, "Test Window");
                //}
                String win1 = "Test Window"; //The name of the window
                CvInvoke.NamedWindow(win1); //Create the window using the specific name

                Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
                img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

                //Draw "Hello, world." on the image using the specific font
                CvInvoke.PutText(
                   img,
                   "Hello, world",
                   new System.Drawing.Point(10, 80),
                   FontFace.HersheyComplex,
                   1.0,
                   new Bgr(0, 255, 0).MCvScalar);


                CvInvoke.Imshow(win1, img); //Show the image
                CvInvoke.WaitKey(0);  //Wait for the key pressing event
                CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed
            }
            catch (Exception ee)
            {

            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(320, 240, new Bgr(255,0, 0));
            pictureBox1.Image = img1.ToBitmap();
        }
    }
}
