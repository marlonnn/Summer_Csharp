using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Frames
{
    public class Frames
    {
        private double _time;

        private Image _image;

        public double Time
        {
            get { return this._time; }
        }

        public Image Image
        {
            get { return this._image; }
        }

        public Frames(double time, Image image)
        {
            this._time = time;
            this._image = image;
        }
    }
}
