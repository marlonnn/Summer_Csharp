using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Mdi
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                _mainForm = new Form();
                Application.Run(_mainForm);
            }
            catch (Exception e)
            {
            }
        }

        private static Form _mainForm;
        public static Form MainForm
        {
            get { return _mainForm; }
        }
    }
}
