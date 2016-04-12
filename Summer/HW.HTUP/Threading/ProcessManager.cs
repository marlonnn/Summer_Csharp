using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Summer.System.Threading;
using Summer.System.Log;
using System.Net;

namespace HW.HTUP.Threading
{
    public class ProcessManager
    {
        private Latch latch;
        private TxQueue txQueue;

        private Thread udpTxStart;
        private Thread udpRxStart;
        private Thread rxProcessStart;

        private SocketThreads socketThreads;
        private RxProcess rxProcess;
        private IpClients ipClients;

        public void RunProcess()
        {
            socketThreads.UdpRxPrepare();
            socketThreads.UdpTxPrepare();
            try
            {
                udpTxStart = new Thread(new ThreadStart(socketThreads.UdpTxStart));
                udpTxStart.Start();
                LogHelper.GetLogger<ProcessManager>().Info("Thread UdpTxProcess Started.");
                udpRxStart = new Thread(new ThreadStart(socketThreads.UdpRxStart));
                udpRxStart.Start();
                LogHelper.GetLogger<ProcessManager>().Info("Thread UdpRxProcess Started.");
                rxProcessStart = new Thread(new ThreadStart(rxProcess.RxProcessStart));
                rxProcessStart.Start();
                LogHelper.GetLogger<ProcessManager>().Info("Thread ProcessRxDataThread Started.");
                //放锁，三个线程启动
                latch.Release();
            }
            catch (Exception e)
            {
                LogHelper.GetLogger<ProcessManager>().Error(e.ToString());
                LogHelper.GetLogger<ProcessManager>().Error(e.StackTrace);
            }
        }

        public void CloseProcess()
        {
            //给接收线程发送指令通知其结束(结束指令是 0xFF 87)
            TxPackage tp = new TxPackage();
            tp.Target = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), ipClients.ListenPort);
            tp.Message = new byte[2];
            tp.Message[0] = 0xFF;
            tp.Message[1] = 0x87;
            //清空发送指令
            txQueue.PopAll();
            txQueue.Push(tp);
            try
            {
                //等待200ms让发送线程发送结束指令，结束接收线程
                Thread.Sleep(200);
                //关闭数据发送线程
                socketThreads.NeedRunning = false;
                //关闭数据处理线程
                rxProcess.NeedRunning = false;
                //等待200m结束发送线程和数据处理线程
                Thread.Sleep(200);
            }
            catch (Exception e)
            {
            }
        }
    }
}
