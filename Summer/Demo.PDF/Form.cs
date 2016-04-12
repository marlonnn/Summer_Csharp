using iTextSharp.text;
using Summer.System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.PDF
{
    public partial class Form : System.Windows.Forms.Form
    {
        private PDFHelper pdfHelper;
        private Document document;
        private string filePath = Environment.CurrentDirectory + @"\tmpPdfCreatorFile.pdf";

        protected readonly float OFFSET_LEFT = 76;
        protected readonly float WIDTH = 740;
        protected readonly float OFFSET_BOTTOM = 36;
        protected readonly float HEIGHT = 504;

        protected readonly float OFFSET_LOCATION = 26;
        protected readonly float WIDTH_LOCATION = 48;

        protected readonly float HEIGHT_LOCATION = 56;
        protected readonly float WIDTH_TIMESLOT = 23.125f;
        protected readonly int LOCATIONS = 9;

        public Form()
        {
            InitializeComponent();
            pdfHelper = new PDFHelper(filePath, PageSize.A4.Rotate(), 36f, 36f, 80f, 20f);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            document = pdfHelper.Document;

            pdfHelper.DrawRectangle(pdfHelper.GetPdfContentByteUnder(), OFFSET_LEFT, OFFSET_BOTTOM, HEIGHT, WIDTH);

            float left = OFFSET_LOCATION;
            float bottom = OFFSET_BOTTOM;
            float width = WIDTH_LOCATION;
            float height = HEIGHT;

            pdfHelper.DrawRectangle(pdfHelper.GetPdfContentByteUnder(), left, bottom, height, width);
            pdfHelper.DrawHorizontalLine(pdfHelper.GetPdfContentByteUnder(), 20, 30, 100);
            pdfHelper.DrawHorizontalLine(pdfHelper.GetPdfContentByteUnder(), 20, 50, 100);
            pdfHelper.DrawVerticalLine(pdfHelper.GetPdfContentByteUnder(), 30, 100, 80);
            pdfHelper.DrawVerticalLine(pdfHelper.GetPdfContentByteUnder(), 80, 100, 80);
            pdfHelper.DrawLine(pdfHelper.GetPdfContentByteUnder(), 80, 100, 100, 300);
            pdfHelper.SaveToFile();
            System.Diagnostics.Process.Start(filePath);
        }
    }
}
