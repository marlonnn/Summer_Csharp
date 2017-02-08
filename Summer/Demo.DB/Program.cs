using Demo.DB.ADO;
using Summer.System.Core;
using System;
using System.Windows.Forms;

namespace Demo.DB
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
                //Application.Run(new Form1());

                var v = SpringHelper.GetObject<MainADO>("mainADO");
            }
            catch (Exception ee)
            {

            }
        }
    }
}
