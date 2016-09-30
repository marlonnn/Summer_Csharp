using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.UI
{
    static class Program
    {

        private static Form1 _mainForm;

        /// <summary>
        /// The reference of program main form
        /// </summary>
        public static Form1 MainForm
        {
            get { return _mainForm; }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _mainForm = new Form1();
            Application.Run(_mainForm);
        }
    }
}
