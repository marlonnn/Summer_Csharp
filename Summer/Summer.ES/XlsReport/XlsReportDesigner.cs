using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Config;
using log4net.Appender;
using System.IO;
using log4net.Repository.Hierarchy;
using System.Reflection;
using Aspose.Cells;
using System.Data;

using Summer.ES.BarCode;

namespace Summer.ES.XlsReport
{
    /// <summary>
    /// 使用xls做报表模板，生成报表
    /// </summary>
    /// <remark>
    /// 思路：封装Aspose.Cells库
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2015-5-7
    /// </remark>
    public class XlsReportDesigner
    {
        WorkbookDesigner designer;

        /// <summary>
        /// 打开报表模板文件
        /// </summary>
        /// <param name="fullPathName"></param>
        public void Open(string fullPathName)
        {
            designer = new WorkbookDesigner();
            designer.Workbook.Open(fullPathName);
        }

        /// <summary>
        /// 将变量更新成实际数据
        /// </summary>
        public void Process()
        {
            if (designer != null)
            {
                designer.Process();
            }
        }
        /// <summary>
        /// 保存文件，注意，系统会根据后缀名自动保存成各种格式，比如xls、xlsx、pdf等等
        /// </summary>
        /// <param name="fullPathName"></param>
        public void Save(string fullPathName)
        {
            if (designer != null)
            {
                designer.Workbook.Save(fullPathName);
            }
        }

        /// <summary>
        /// 设置单个变量数据源，xls表中命名规则为 &=$variable
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="data"></param>
        public void SetDataSource(string variable, object data)
        {
            if (designer != null)
                designer.SetDataSource(variable, data);
        }

        /// <summary>
        /// 设置列表数据源，填充内存表，xls表中命名规则为 &=[tablename].columnname 或者 &=tablename.columnname
        /// </summary>
        /// <param name="dataTable"></param>
        public void SetDataSource(DataTable dataTable)
        {
            if (designer != null)
            {
                designer.SetDataSource(dataTable);
            }
        }
        private static int id = 1;

        public string TmpFilePath = "./Tmp/";
        /// <summary>
        /// 设置条码图像
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="data"></param>
        public void SetBarCode(int sheetIndex, int row, int col, int imageWidth,int imageHeight, string barCodeText)
        {
            if (designer != null)
            {
                string filename = string.Format("/{0}.emf", id++);
                string path = TmpFilePath + filename;
                BarCodeHelper.BuildEmfFile(barCodeText, BarCodeType.Code128, path);
                designer.Workbook.Worksheets[sheetIndex].Pictures.Add(row, col, path);
            }
        }

        /// <summary>
        /// 获得xls模板中的所有标记变量
        /// </summary>
        /// <returns></returns>
        public string[] GetSmartMarkers()
        {
            if (designer != null)
                return designer.GetSmartMarkers();
            return null;
        }
    }
}
