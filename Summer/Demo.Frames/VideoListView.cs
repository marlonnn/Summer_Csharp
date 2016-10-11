using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Demo.Frames
{
    public partial class VideoListView : ListView
    {
        public event EventHandler OnLoadComplete;

        private int _thumbNailSize = 32;
        public int ThumbNailSize
        {
            get { return _thumbNailSize; }
            set { _thumbNailSize = value; }
        }

        private Color thumbBorderColor = Color.Black;
        public Color ThumbBorderColor
        {
            get { return thumbBorderColor; }
            set { thumbBorderColor = value; }
        }

        public VideoListView()
        {
            InitializeComponent();

            ImageList il = new ImageList();
            il.ImageSize = new Size(_thumbNailSize, _thumbNailSize);
            il.ColorDepth = ColorDepth.Depth32Bit;
            il.TransparentColor = Color.White;
            LargeImageList = il;
            //components.Add(myWorker);
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += DoWork;
            _worker.RunWorkerCompleted += RunWorkerCompleted;
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (OnLoadComplete != null)
                OnLoadComplete(this, new EventArgs());
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            if (_worker.CancellationPending) return;
            string[] fileList = (string[])e.Argument;

            foreach (string fileName in fileList)
                SetThumbnail(GetThumbNail(fileName));

        }

        private delegate void SetThumbnailDelegate(Image image);
        private void SetThumbnail(Image image)
        {
            if (Disposing) return;

            if (this.InvokeRequired)
            {
                SetThumbnailDelegate d = new SetThumbnailDelegate(SetThumbnail);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                LargeImageList.Images.Add(image); //Images[i].repl  
                int index = LargeImageList.Images.Count - 1;
                Items[index - 1].ImageIndex = index;
            }
        }

        public Image GetThumbNail(string fileName)
        {
            return GetThumbNail(fileName, _thumbNailSize, _thumbNailSize, thumbBorderColor);
        }

        public static Image GetThumbNail(string fileName, int imgWidth, int imgHeight, Color penColor)
        {
            Bitmap bmp;

            try
            {
                bmp = new Bitmap(fileName);
            }
            catch
            {
                bmp = new Bitmap(imgWidth, imgHeight); //If we cant load the image, create a blank one with ThumbSize
            }


            imgWidth = bmp.Width > imgWidth ? imgWidth : bmp.Width;
            imgHeight = bmp.Height > imgHeight ? imgHeight : bmp.Height;

            Bitmap retBmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format64bppPArgb);

            Graphics grp = Graphics.FromImage(retBmp);


            int tnWidth = imgWidth, tnHeight = imgHeight;

            if (bmp.Width > bmp.Height)
                tnHeight = (int)(((float)bmp.Height / (float)bmp.Width) * tnWidth);
            else if (bmp.Width < bmp.Height)
                tnWidth = (int)(((float)bmp.Width / (float)bmp.Height) * tnHeight);

            int iLeft = (imgWidth / 2) - (tnWidth / 2);
            int iTop = (imgHeight / 2) - (tnHeight / 2);

            grp.PixelOffsetMode = PixelOffsetMode.None;
            grp.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grp.DrawImage(bmp, iLeft, iTop, tnWidth, tnHeight);

            Pen pn = new Pen(penColor, 1); //Color.Wheat
            grp.DrawRectangle(pn, 0, 0, retBmp.Width - 1, retBmp.Height - 1);

            return retBmp;
        }

        public void LoadBinItem(List<VideoFiles> videoFilesList)
        {
            if ((_worker != null) && (_worker.IsBusy))
                _worker.CancelAsync();

            BeginUpdate();
            Items.Clear();
            LargeImageList.Images.Clear();
            AddDefaultThumb();

            List<string> fileList = new List<string>();
            for (int i = 0; i < videoFilesList.Count; i++)
            {
                ListViewItem liTemp = Items.Add(videoFilesList[i].FileName);
                liTemp.ImageIndex = 0;
                liTemp.Tag = videoFilesList[i].FilePath;
                fileList.Add(videoFilesList[i].FilePath);
            }

            EndUpdate();
            if (_worker != null)
            {
                if (!_worker.CancellationPending)
                    _worker.RunWorkerAsync(fileList.ToArray());
            }
        }

        private void AddDefaultThumb()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(LargeImageList.ImageSize.Width, LargeImageList.ImageSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Graphics grp = Graphics.FromImage(bmp);
            Brush brs = new SolidBrush(Color.White);
            Rectangle rect = new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1);
            grp.FillRectangle(brs, rect);
            Pen pn = new Pen(Color.Wheat, 1);
            grp.DrawRectangle(pn, 0, 0, bmp.Width - 1, bmp.Height - 1);
            LargeImageList.Images.Add(bmp);
        }
    }
}
