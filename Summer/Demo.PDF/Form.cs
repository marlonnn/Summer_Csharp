using iTextSharp.text;
using iTextSharp.text.pdf;
using Summer.System.IO;
using Summer.System.Util;
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
        private PDFHelper pdfCreator;
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
            pdfCreator = new PDFHelper(filePath, PageSize.A4, 36f, 36f, 80f, 20f);
        }

        private void OnLoad(object sender, EventArgs e)
        {

            try
            {
                document = pdfCreator.Document;
                createHeadImage();
                createTitle();
                createMessage();
                createImage();
                createBottomMessage();
                saveToFile();
                openPdfFile();
            }
            catch (Exception ex)
            {

            }
            //try
            //{
            //    //ImageModify imageModify = new ImageModify();
            //    //FileStream fileStream = null;
            //    //string imagePath = string.Format("{0}\\{1}", Environment.CurrentDirectory, "Resource\\Image.png");
            //    //string savePath = string.Format("{0}\\{1}", Environment.CurrentDirectory, "Resource\\Image_1.png");

            //    //try
            //    //{
            //    //    fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    fileStream.Close();
            //    //}
            //    //SuperImage.ZoomAuto(fileStream, savePath, 800, 800);
            //    //fileStream.Close();
            //    document = pdfCreator.Document;
            //    //createHeadImage();
            //    //createTitle();
            //    //createMessage();
            //    //createImage();
            //    //createBottomMessage();
            //    test();
            //    saveToFile();
            //    openPdfFile();
            //}
            //catch (Exception ex)
            //{

            //}

        }

        private void test()
        {
            PdfWriter pdfWriter = pdfCreator.PdfWriter;
            PdfContentByte pdfContentByte = pdfWriter.DirectContentUnder;
            pdfContentByte.SaveState();
            float llx, lly, urx, ury;

            llx = OFFSET_LEFT;
            lly = OFFSET_BOTTOM;
            urx = OFFSET_LEFT + WIDTH;
            ury = OFFSET_BOTTOM + HEIGHT;

            pdfContentByte.MoveTo(llx, lly);
            pdfContentByte.LineTo(urx, lly);
            pdfContentByte.LineTo(urx, ury);
            pdfContentByte.LineTo(llx, ury);
            pdfContentByte.ClosePath();
            pdfContentByte.Stroke();

            llx = OFFSET_LOCATION;
            lly = OFFSET_BOTTOM;
            urx = OFFSET_LOCATION + WIDTH_LOCATION;
            ury = OFFSET_BOTTOM + HEIGHT;
            pdfContentByte.MoveTo(llx, lly);
            pdfContentByte.LineTo(urx, lly);
            pdfContentByte.LineTo(urx, ury);
            pdfContentByte.LineTo(llx, ury);
            pdfContentByte.ClosePathStroke();

            pdfContentByte.SetLineWidth(1);
            pdfContentByte.MoveTo(OFFSET_LOCATION + WIDTH_LOCATION / 2, OFFSET_BOTTOM);
            pdfContentByte.LineTo(OFFSET_LOCATION + WIDTH_LOCATION / 2, OFFSET_BOTTOM + HEIGHT);

            float y;

            for (int i = 0; i < LOCATIONS; i++)
            {
                y = OFFSET_BOTTOM + (i * HEIGHT_LOCATION);

                if (i == 2 || i == 6)
                {
                    pdfContentByte.MoveTo(OFFSET_LOCATION, y);
                    pdfContentByte.LineTo(OFFSET_LOCATION + WIDTH_LOCATION, y);
                }
                else
                {
                    pdfContentByte.MoveTo(OFFSET_LOCATION + WIDTH_LOCATION / 2, y);
                    pdfContentByte.LineTo(OFFSET_LOCATION + WIDTH_LOCATION, y);
                }
                pdfContentByte.MoveTo(OFFSET_LEFT, y);
                pdfContentByte.LineTo(OFFSET_LEFT + WIDTH, y);

            }

            pdfContentByte.Stroke();

            pdfContentByte.RestoreState();
        }

        //创建标题
        private void createTitle()
        {
            pdfCreator.CreatePdfTitle("Report of Sample1", 20f, 15f, 15f);
        }

        //创建测试内容
        private void createMessage()
        {
            PdfPTable table = pdfCreator.CreatePdfTable(4, 90, 20, 10);

            //cell = pdfCreator.CreatePdfCell("Sample Name", "Sample1", Element.ALIGN_LEFT, 2, Rectangle.NO_BORDER, 10);
            //table.AddCell(cell);

            //cell = pdfCreator.CreatePdfCell("Run Date", "11/04/2014", Element.ALIGN_LEFT, 2, Rectangle.NO_BORDER, 10);
            //table.AddCell(cell);

            //cell = pdfCreator.CreatePdfCell("Sample Type", "Unknown Sample", Element.ALIGN_LEFT, 2, Rectangle.NO_BORDER, 10);
            //table.AddCell(cell);

            //cell = pdfCreator.CreatePdfCell("Software", "NovoExpress 1.2.1", Element.ALIGN_LEFT, 2, Rectangle.NO_BORDER, 10);
            //table.AddCell(cell);

            //cell = pdfCreator.CreatePdfCell("BEADYPLEX Kit Lot No", "BYP Lot 150903", Element.ALIGN_LEFT, 2, Rectangle.NO_BORDER, 10);
            //table.AddCell(cell);

            //cell = pdfCreator.CreatePdfCell("Cytometer", "NovoCyte 12345", Element.ALIGN_LEFT, 2, Rectangle.NO_BORDER, 10);
            //table.AddCell(cell);

            //cell = pdfCreator.CreatePdfCell("Test ID", "160129_1535", Element.ALIGN_LEFT, 4, Rectangle.NO_BORDER, 10);
            //table.AddCell(cell);

            PdfPCell cell = pdfCreator.CreatePdfCell("Sample Name", "Sample1", Element.ALIGN_LEFT, 2, 10);
            table.AddCell(cell);

            cell = pdfCreator.CreatePdfCell("Run Date", "11/04/2014", Element.ALIGN_LEFT, 2, 10);
            table.AddCell(cell);

            cell = pdfCreator.CreatePdfCell("Sample Type", "Unknown Sample", Element.ALIGN_LEFT, 2, 10);
            table.AddCell(cell);

            cell = pdfCreator.CreatePdfCell("Software", "NovoExpress 1.2.1", Element.ALIGN_LEFT, 2, 10);
            table.AddCell(cell);

            cell = pdfCreator.CreatePdfCell("BEADYPLEX Kit Lot No", "BYP Lot 150903", Element.ALIGN_LEFT, 2, 10);
            table.AddCell(cell);

            cell = pdfCreator.CreatePdfCell("Cytometer", "NovoCyte 12345", Element.ALIGN_LEFT, 2, 10);
            table.AddCell(cell);

            cell = pdfCreator.CreatePdfCell("Test ID", "160129_1535", Element.ALIGN_LEFT, 4, 10);
            table.AddCell(cell);

            document.Add(table);
        }

        //创建测试图像
        private void createImage()
        {
            string imagePath = string.Format("{0}\\{1}", Environment.CurrentDirectory, "Resource\\data_sheet.png");
            PdfPTable table = pdfCreator.CreatePdfTable(1, 80, 10, 20);
            table.WidthPercentage = 80;

            PdfPCell cell = pdfCreator.CreatePdfCell(imagePath);
            table.AddCell(cell);
            document.Add(table);
        }

        //创建测试logo
        private void createHeadImage()
        {
            try
            {
                string leftLogo = string.Format("{0}\\{1}", Environment.CurrentDirectory, "Resource\\logo1.png");
                string rightLogo = string.Format("{0}\\{1}", Environment.CurrentDirectory, "Resource\\logo2.png");

                pdfCreator.CreateLogoLeft(leftLogo, 85f);
                pdfCreator.CreateLogoRight(rightLogo, 85f);
            }
            catch (Exception e)
            {

            }
        }

        //创建测试内容
        private void createBottomMessage()
        {
            PdfPTable table = pdfCreator.CreatePdfTable(1, 90, 20, 10);
            PdfPCell cell = pdfCreator.CreatePdfCell("Test Validation", "VALID", Element.ALIGN_LEFT, 1, iTextSharp.text.Rectangle.NO_BORDER, 10);
            table.AddCell(cell);

            cell = pdfCreator.CreatePdfCell("Operator", "User1", Element.ALIGN_LEFT, 1, iTextSharp.text.Rectangle.NO_BORDER, 10);
            table.AddCell(cell);
            document.Add(table);
        }

        //保存文件到本地
        private void saveToFile()
        {
            pdfCreator.SaveToFile();
        }

        private void openPdfFile()
        {
            System.Diagnostics.Process.Start(filePath);
        }
    }
}
