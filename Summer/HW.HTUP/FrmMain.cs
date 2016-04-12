using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HW.HTUP.Threading;
using Summer.System.Core;
using System.Threading;
using log4net.Appender;
using Summer.System.Log;
using log4net.Core;

namespace HW.HTUP
{
    public partial class FrmMain : Form
    {
        MemoryAppender memAppender;

        public FrmMain()
        {
            InitializeComponent();

            //启动后台处理进程
            ProcessManager pm = SpringHelper.GetObject<ProcessManager>("processManager");
            pm.RunProcess();

            //初始化内存日志
            memAppender = LogHelper.GetAppender("MemoryAppender") as MemoryAppender;

            //启动界面刷新计时器
            tmReferesh.Enabled = true;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //关闭后台处理进程
            ProcessManager pm = SpringHelper.GetObject<ProcessManager>("processManager");
            pm.CloseProcess();
            Thread.Sleep(500);
        }

        delegate void VoidFuncMsg(string msg);

        public void UpdateListMsg(string log)
        {
            lbLog.Items.Add(log);
            lbLog.SelectedIndex = lbLog.Items.Count - 1;
            if (lbLog.Items.Count > 50)   //日志大于50条就清屏
                lbLog.Items.Clear();
        }

        private void tmReferesh_Tick(object sender, EventArgs e)
        {
            VoidFuncMsg vf = new VoidFuncMsg(UpdateListMsg);

            if (memAppender != null)
            {
                LoggingEvent[] evts = memAppender.GetEvents();
                if (evts.Length > 0)
                {
                    memAppender.Clear();
                    lbLog.BeginUpdate();
                    foreach (LoggingEvent evt in evts)
                    {
                        this.Invoke(vf, string.Format("{0}.{1} [{2}] {3}",
                            evt.TimeStamp.ToLongTimeString(),
                            evt.TimeStamp.Millisecond,
                            evt.ThreadName,
                            evt.RenderedMessage));
                    }
                    lbLog.EndUpdate();
                }
            }
        }
    }
}
