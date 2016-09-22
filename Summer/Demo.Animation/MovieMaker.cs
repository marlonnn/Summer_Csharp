using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Demo.Animation
{
    public class MovieMaker
    {
        public Bitmap ReduceBitmap(Bitmap original, int reducedWidth, int reducedHeight)
        {
            var reduced = new Bitmap(reducedWidth, reducedHeight);
            using (var dc = Graphics.FromImage(reduced))
            {
                // you might want to change properties like
                dc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                dc.DrawImage(original, new Rectangle(0, 0, reducedWidth, reducedHeight), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
            }

            return reduced;
        }

        public void CreateVideo(Bitmap[] bitmaps)
        {
            //int width = 320;
            //int height = 240;

            //// create instance of video writer
            //VideoFileWriter writer = new VideoFileWriter();
            //// create new video file
            //writer.Open(@"\Frames\test.avi", width, height, 25, VideoCodec.MPEG4);
            //// write 1000 video frames
            //for (int i = 0; i < bitmaps.Length; i++)
            //{
            //    writer.WriteVideoFrame(bitmaps[i]);
            //}
            //writer.Close();
        }
    }
}
