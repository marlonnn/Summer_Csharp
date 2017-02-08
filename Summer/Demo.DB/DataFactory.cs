using Demo.DB.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DB
{
    /// <summary>
    /// 模拟不断插入数据到数据库
    /// </summary>
    public class DataFactory
    {
        private MainADO mainADO;
        public void SimulateDataInternal()
        {
            TMain tMain = new TMain();
            tMain.Key = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss: ffff");
            tMain.Start = DateTime.Now;
            tMain.End = DateTime.Now;
            tMain.Note = "This is simulate test data.";
            mainADO.Insert(tMain);
        }

    }
}
