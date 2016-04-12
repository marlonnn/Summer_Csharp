using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Summer.System.Log;
using System.Net;
using Summer.System.Util;

namespace HW.HTUP.Threading
{
    /// <summary>
    /// 接收协议封装 v1 版本，接收完成后，将处理后的数据压入RxQueue
    /// [0] CmdCode;
    /// [1-2] Sequence(1是高位，2是低位);
    /// [3] ErrorState;
    /// [4] ErrorCode;
    /// [5-n] Data; 
    /// </summary>
    public class RxBaseV1Protocol : RxBaseProtocol
    {
        protected RxQueue rxQueue;

        public override void Decode(IPEndPoint source, byte[] data)
        {
            LogHelper.GetLogger<RxBaseV1Protocol>().Debug(string.Format("应用层解包接收数据，来源：{0}，数据体：{1}",
                source.ToString(),
                ByteHelper.Byte2ReadalbeXstring(data)));

            if (data.Length < 7)
            {
                LogHelper.GetLogger<RxBaseV1Protocol>().Error(string.Format("应用层预解包数据长度不够，来源：{0}，数据体：{1}",
                source.ToString(),
                ByteHelper.Byte2ReadalbeXstring(data)));
                return;
            }
            RxPackage rp = new RxPackage();
            rp.Source = source;
            rp.CmdType = data[0];
            rp.Sequence = data[1] << 8 | data[2];
            rp.ErrorState = data[3];
            rp.ErrorCode = data[4];
            rp.Data = new byte[data.Length - 5];
            Array.Copy(data, 5, rp.Data, 0, rp.Data.Length);
            rxQueue.Push(rp);

            if (rp.ErrorState != 0)
            {
                LogHelper.GetLogger<RxBaseV1Protocol>().Error(string.Format("应用层解包的数据包含错误码，来源：{0}，错误码0x{1:x}，错误索引0x{2:x}，数据体：{3}",
                source.ToString(),
                rp.ErrorState,
                rp.ErrorCode,
                ByteHelper.Byte2ReadalbeXstring(data)));
            }
        }
    }

    public class TxBaseV1Protocol : TxBaseProtocol
    {
        protected TxQueue txQueue;

        /// <summary>
        /// 将命令体编码，编码完成后，将待发送数据压入TxQueue
        /// [0] cmdCode;
        /// [1-2] Sequence;
        /// [3-n] CmdData;
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cmdData"></param>
        public override void Encode(IPEndPoint target, byte cmdCode, int Sequence, byte[] cmdData)
        {
            LogHelper.GetLogger<TxBaseV1Protocol>().Debug(string.Format("应用层预封装发送数据，目标：{0}，命令码：0x{1:x}，发送序号：{2}，数据体：{3}",
                target.ToString(),
                cmdCode,
                Sequence,
                ByteHelper.Byte2ReadalbeXstring(cmdData)));
            if (cmdData == null || cmdData.Length == 0)
            {
                LogHelper.GetLogger<TxBaseV1Protocol>().Error(string.Format("应用层预封装发送数据的数据体内容为空，目标：{0}，命令码：0x{1:x}，发送序号：{2}",
                    target.ToString(),
                    cmdCode,
                    Sequence));
            }
            TxPackage tp = new TxPackage();
            tp.Target = target;
            byte[] Data = new byte[cmdData.Length + 3];
            Data[0] = cmdCode;
            Data[2] = (byte)Sequence;           //低位
            Data[1] = (byte)(Sequence >> 8);    //高位
            Array.Copy(cmdData, 0, Data, 3, cmdData.Length);
            tp.Message = cmdData;

            LogHelper.GetLogger<TxBaseV1Protocol>().Debug(string.Format("应用层完成封装发送数据，目标：{0}，命令体：{1}",
                target.ToString(),
                ByteHelper.Byte2ReadalbeXstring(Data)));
            tp.Message = Data;

            txQueue.Push(tp);
        }
    }
}
