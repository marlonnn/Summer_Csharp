using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Log;
using System.Threading;
using Common.Logging;

namespace Demo.Summer.System.Log
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                //LogManager.GetLogger<Program>().Debug("00000000000000000000");
                //LogManager.GetLogger<Program>().Debug("00000000000000000000");
                //LogManager.GetLogger<Program>().Info("1111111111111111111111");
                //LogManager.GetLogger<Program>().Warn("2222222222222222222222222");
                //LogManager.GetLogger<Program>().Error("33333333333333333333333333");
                //LogManager.GetLogger<Program>().Fatal("4444444444444444444444444");

                //LogHelper.GetLogger("JobLog").Fatal("4444444444444444444444444");
                LogHelper.GetLogger<Program>().Info("xxxxxxxxxxxxxx");

                //LogHelper.SetRootLevel(Level.Debug);

                Thread.Sleep(5000);
            }
        }
    }
}
