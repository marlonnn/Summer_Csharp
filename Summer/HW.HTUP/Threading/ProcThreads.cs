using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

using Summer.System.Threading;
using Summer.System.Log;
using Summer.System.Util;
using HW.HTUP.Model;

namespace HW.HTUP.Threading
{
    /// <summary>
    /// IP基本配置结构体
    /// </summary>
    public class IpClients
    {
        public int ListenPort;
        public List<string> Clients;
    }

    /// <summary>
    /// 网络通信类，建立侦听，处理数据收发
    /// </summary>
    public class SocketThreads
    {
        private Latch latch;

        private IpClients ipClients;
        private Dictionary<string, UdpClient> udpClients;

        public volatile bool NeedRunning = true;

        private TxQueue txQueue;

        private FrameProtocol frameProtocol;//通信层数据包处理，队列中存储的是数据体

        private RxBaseProtocol rxBaseProtocol;

        private static Logger log = LogHelper.GetLogger<SocketThreads>();

        public void UdpTxPrepare()
        {
            try
            {
                udpClients = new Dictionary<string, UdpClient>();
                foreach (string ipport in ipClients.Clients)
                {
                    string[] arr = ipport.Split(':');
                    UdpClient udpClient = new UdpClient();
                    udpClient.Connect(IPAddress.Parse(arr[0]), int.Parse(arr[1]));
                    udpClients.Add(ipport, udpClient);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                log.Error(e.StackTrace);
            }
        }
        /// <summary>
        /// 发送数据消费者：每50ms读取队列数据，然后发送到指定端口数据
        /// </summary>
        public void UdpTxStart()
        {
            //准备工作结束后等待启动指令
            latch.Acquire();
            //发送队列循环
            while (NeedRunning)
            {
                try
                {
                    List<TxPackage> list = txQueue.PopAll();
                    foreach (TxPackage np in list)
                    {
                        try
                        {
                            UdpClient udpClient = udpClients[np.Target.ToString()];
                            //对数据包进行加包
                            byte[] frameData = frameProtocol.EnPackage(np.Message);

                            udpClient.Send(frameData, frameData.Length);
                            log.Debug("Send to " + np.Target.ToString() + ", Data: " + ByteHelper.Byte2ReadalbeXstring(frameData));
                        }
                        catch (Exception ee)
                        {
                            log.Error(string.Format("Send to {0} Error. Reason: {1}, Detail{2}."
                                , np.Target.ToString()
                                ,ee.ToString()
                                ,ee.StackTrace));
                        }
                    }
                    if (!NeedRunning)
                        break;
                    //休眠50ms后再取发送队列
                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    log.Error(e.StackTrace);
                }
            }
            log.Info("Thread UdpTxProcess is terminated normally");
        }

        private UdpClient receivingUdpClient;
        public void UdpRxPrepare()
        {
            try
            {
                receivingUdpClient = new UdpClient(ipClients.ListenPort);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                log.Error(e.StackTrace);
            }
        }
        /// <summary>
        /// 接收数据生产者：侦听端口接收数据
        /// </summary>
        public void UdpRxStart()
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            latch.Acquire();
            //准备工作结束后等待启动指令
            while (NeedRunning)
            {
                try
                {
                    byte[] receiveBytes = receivingUdpClient.Receive(ref remoteIpEndPoint);

                    log.Debug(string.Format("Received from {0}, Data: {1}" ,
                        remoteIpEndPoint.ToString(),
                        ByteHelper.Byte2ReadalbeXstring(receiveBytes)) );

                    //收到结束指令
                    if (receiveBytes.Length == 4 && receiveBytes[1] == 0xFF && receiveBytes[2] == 0x87)
                    {
                        NeedRunning = false;
                        break;
                    }
                    else
                    {
                        //先进行通信层协议解包
                        byte[] data = frameProtocol.DePackage(receiveBytes);
                        //在进行应用层协议解包
                        if (data != null)
                        {
                            rxBaseProtocol.Decode(remoteIpEndPoint, receiveBytes);
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    log.Error(e.StackTrace);
                }
            }
            receivingUdpClient.Close();
            log.Info("Thread UdpRxProcess is terminated normally");
        }
    }

    /// <summary>
    /// 接收数据处理线程，将队列中的数据分发给各个观察者
    /// </summary>
    public class RxProcess
    {
        private Latch latch;
        private RxQueue rxQueue;
        private List<IObserveRxMessage> observers;

        private static Logger log = LogHelper.GetLogger<RxProcess>();

        public void Attach(IObserveRxMessage observer)
        {
            lock (observers)
            {
                observers.Add(observer);
            }
        }

        public void Dettach(IObserveRxMessage observer)
        {
            lock (observers)
            {
                observers.Remove(observer);
            }
        }

        public volatile bool NeedRunning = true;
        /// <summary>
        /// 接收数据消费者：接收数据处理线程，处理完成后，休眠50ms，再取一次队列数据
        /// </summary>
        public void RxProcessStart()
        {
            //准备工作结束后等待启动指令
            latch.Acquire();
            while (NeedRunning)
            {
                List<RxPackage> list = rxQueue.PopAll();
                foreach (RxPackage np in list)
                {
                    if (!NeedRunning)
                        break;
                    log.Debug(string.Format("Dispatch Data，来源：{0}，命令码：0x{1:x}，接收序号：{2}，数据体：{3}",
                        np.Source.ToString(),
                        np.CmdType,
                        np.Sequence,
                        ByteHelper.Byte2ReadalbeXstring(np.Data)));

                    //将数据分发给各个观察者
                    IObserveRxMessage[] observersCopy;
                    lock (observers)
                    {
                        observersCopy = new IObserveRxMessage[observers.Count];
                        //复制一份观察者，因为系统的观察者在此循环中可能会被注销
                        observers.CopyTo(observersCopy, 0);
                    }
                    foreach (IObserveRxMessage i in observersCopy)
                    {
                        log.Debug(string.Format("观察者{0}开始处理数据；", i.GetType().FullName));
                        i.OnNewMessage(np);
                        log.Debug(string.Format("观察者{0}处理数据完成。", i.GetType().FullName));
                    }
                    log.Debug(string.Format("Dispatch Data Finish，来源：{0}，命令码：0x{1:x}，接收序号：{2}",
                        np.Source.ToString(),
                        np.CmdType,
                        np.Sequence));
                }
                if (!NeedRunning)
                    break;
                Thread.Sleep(50);
            }
            log.Info("Thread ProcessRxDataThread is terminated normally");
        }
    }
}
