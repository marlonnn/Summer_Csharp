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
                MediaPlayer newVideo = new MediaPlayer(this.openFileDialog.FileName);

                //if (!this.mediaManager.videos.Contains(this.openFileDialog.FileName))
                {
                    double fps = 0;
                    double duration = 0;
                    fps = newVideo.getFramePerSecond();
                    duration = newVideo.getDuration();
                    int i = videoFilesList.Count;
                    newVideo.LoadSnapShots();

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
    }
}
