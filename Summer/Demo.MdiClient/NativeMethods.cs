using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Mdi
{
    public static class NativeMethods
    {
        // Window Styles
        public const int WS_HSCROLL = 0x00100000;
        public const int WS_VSCROLL = 0x00200000;
        public const int SM_CXVSCROLL = 2;
        public const int GWL_STYLE = -16;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static int GetWindowLong(IntPtr hWnd, int index);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetSystemMetrics(int nIndex);
    }
}
