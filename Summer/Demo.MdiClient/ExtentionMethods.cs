using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Mdi
{
    public static class ExtentionMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static int GetWindowLong(IntPtr hWnd, int index);

        public static bool IsHScrollVisible(this Control c)
        {
            return (NativeMethods.GetWindowLong(c.Handle, NativeMethods.GWL_STYLE) & NativeMethods.WS_HSCROLL) != 0;
        }

        public static bool IsVScrollVisible(this Control c)
        {
            return (NativeMethods.GetWindowLong(c.Handle, NativeMethods.GWL_STYLE) & NativeMethods.WS_VSCROLL) != 0;
        }
    }
}
