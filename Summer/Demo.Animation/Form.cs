﻿using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Animation
{
    public partial class Form : System.Windows.Forms.Form
    {
        // The frame images.
        private Bitmap[] Frames;

        // The index of the current frame.
        private int FrameNum = 0;

        private MovieMaker maker;

        public Form()
        {
            InitializeComponent();
            maker = new MovieMaker();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            string fileFolder = System.Environment.CurrentDirectory + @"\Frames";
            string[] filter = new string[] { ".png" };
            FileInfo[] fileInfos = GetFileInfo(fileFolder, filter);
            // Load the frames.
            Frames = new Bitmap[fileInfos.Length];

            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Bitmap(fileInfos[i].FullName);
            }
            try
            {
                CreateVideo(Frames);
            }
            catch (Exception ee)
            {


            }

            // Display the first frame.
            picFrame.Image = Frames[FrameNum];

            // Size the form to fit.
            ClientSize = new Size(
                picFrame.Right + picFrame.Left,
                picFrame.Bottom + picFrame.Left);
        }

        public void CreateVideo(Bitmap[] bitmaps)
        {
            int width = 320;
            int height = 240;

            // create instance of video writer
            VideoFileWriter writer = new VideoFileWriter();
            // create new video file
            writer.Open(@"\Frames\test.avi", width, height, 25, VideoCodec.MPEG4);
            // write 1000 video frames
            for (int i = 0; i < bitmaps.Length; i++)
            {
                writer.WriteVideoFrame(bitmaps[i]);
            }
            writer.Close();
        }

        private FileInfo[] GetFileInfo(string fileFolder, string[] filter)
        {
            DirectoryInfo folder = new DirectoryInfo(fileFolder);
            try
            {
                FileInfo[] files;
                files = folder.GetFiles().Where(f => filter.Contains(f.Extension.ToLower())).ToArray();
                //Array.Sort(files, (f1, f2) => { return f1.Name.CompareTo(f2.Name); });
                SortAsFolderName(ref files);
                return files;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        private void SortAsFolderName(ref FileInfo[] dirs)
        {
            Array.Sort(dirs, delegate (FileInfo x, FileInfo y) { return x.FullName.CompareTo(y.FullName); });
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            tmrNextFrame.Enabled = !tmrNextFrame.Enabled;
            if (tmrNextFrame.Enabled) btnStartStop.Text = "Stop";
            else btnStartStop.Text = "Start";
        }

        private void hscrFps_Scroll(object sender, ScrollEventArgs e)
        {
            tmrNextFrame.Interval = 1000 / hscrFps.Value;
            lblFps.Text = hscrFps.Value.ToString();
        }

        // Display the next image.
        private void tmrNextFrame_Tick(object sender, EventArgs e)
        {
            FrameNum = ++FrameNum % Frames.Length;
            picFrame.Image = Frames[FrameNum];
        }
    }
}
