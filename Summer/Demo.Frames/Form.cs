using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoEditor;

namespace Demo.Frames
{
    public partial class Form : System.Windows.Forms.Form
    {
        private System.Windows.Forms.OpenFileDialog openFileDialog;

        private static List<VideoFiles> videoFilesList = new List<VideoFiles>();

        private MediaPlayer mediaPlayer;

        public Form()
        {
            InitializeComponent();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.openFileDialog.FileName);
            if ((!fileInfo.Exists) || (!CheckExtension(fileInfo)))
                return;

            bool bFound = false;
            string sFilePath = string.Empty;
            for (int i = 0; i < videoFilesList.Count; i++)
            {
                sFilePath = videoFilesList[i].FilePath;
                if (fileInfo.FullName == sFilePath)
                {
                    bFound = true;
                    break;
                }
            }
            if (bFound)
                return;
            try
            {
                mediaPlayer = new MediaPlayer(this.openFileDialog.FileName);

                //if (!this.mediaManager.videos.Contains(this.openFileDialog.FileName))
                {
                    double fps = 0;
                    double duration = 0;
                    fps = mediaPlayer.getFramePerSecond();
                    duration = mediaPlayer.getDuration();
                    int i = videoFilesList.Count;
                    mediaPlayer.LoadSnapShots();
                    var v = mediaPlayer.segmentationImages;

                    this.ReloadInfo();

                    videoFilesList.Add(new VideoFiles(fileInfo.Name, fileInfo.FullName, 0, fps, duration));
                    this.videoListView1.LoadBinItem(videoFilesList);

                    //Bill SerGio: Dispose of DataSet
                    //ds.Dispose();
                }
            }
            catch (COMException comex)
            {
                MessageBox.Show("Failed to load video: " + comex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ReloadInfo()
        {
            this.snapShots1.Clear();
            int i = 0;
            PictureBox box;
            //if (this.mediaManager.currentMedia != null)
            {
                List<Frames> list = mediaPlayer.segmentationImages;
                ShowInfo();
                //this.images = list;
                //if (this.mediaManager.currentMedia.fileType == MediaPlayer.FileType.Video)
                {
                    this.snapShots1.LoadFrames(list);
                }
            }
        }

        private void ShowInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FilePath: ");
            sb.Append(mediaPlayer.mediaInfo.sFilePath);
            sb.Append("\r\n");

            sb.Append("Duration: ");
            sb.Append(mediaPlayer.mediaInfo.duration.ToString());
            sb.Append("\r\n");

            sb.Append("FPS: ");
            sb.Append(mediaPlayer.mediaInfo.fps.ToString());
            sb.Append("\r\n");

            sb.Append("Height: ");
            sb.Append(mediaPlayer.mediaInfo.MediaHeight.ToString());
            sb.Append("\r\n");

            sb.Append("Width: ");
            sb.Append(mediaPlayer.mediaInfo.MediaWidth.ToString());
            sb.Append("\r\n");

            lblInfo.Text = sb.ToString();
        }

        private bool CheckExtension(FileInfo f)
        {
            if (f.Extension.StartsWith(".mp"))
                return true;
            if (f.Extension.StartsWith(".wm"))
                return true;
            if (f.Extension.StartsWith(".avi"))
                return true;
            if (f.Extension.StartsWith(".asf"))
                return true;
            if (f.Extension.StartsWith(".mov"))
                return true;
            if (f.Extension.StartsWith(".rm"))
                return true;
            if (f.Extension.StartsWith(".ram"))
                return true;
            if (f.Extension.StartsWith(".m4v"))
                return true;
            return false;
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            this.snapShots1.Clear();

            System.Threading.Thread t = new System.Threading.Thread(delegate ()
            {
                Environment.Exit(1);
            });
            t.Start();
            t.Join();

            System.Windows.Forms.Application.Exit();
            System.Environment.Exit(0);
        }
    }
}
