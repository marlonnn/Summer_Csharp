
using System.Net;
using Summer.System.Log;
using Summer.System.Util;
using System;

namespace HW.HTUP.Threading
{
    /// <summary>
    /// 数据帧协议（通信层），用于处理数据的分包、组包以及奇偶校验等
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-14
    /// </remark>
    public class FrameProtocol
    {
        /// <summary>
        /// 应用标志位
        /// </summary>
        private byte AppMark;
        /// <summary>
        /// 将数据解包（第一个字节是标识符，最后一个字节是奇偶校验）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] DePackage(byte[] data)
        {
            if (data == null || data.Length < 2 )
            {
                LogHelper.GetLogger<FrameProtocol>().Error("通信层接收到数据包为空或者数据长度不足，丢弃。");
                return null;
            }
            if (data[0] != AppMark)
            {
                LogHelper.GetLogger<FrameProtocol>().Error("通信层接收到数据包不是本应用需要接受的数据包，丢弃。");
                return null;
            }
            //计算奇偶校验和，最后一位不参与奇偶校验
            byte oddCheck = data[0];
            for (int i = 1; i < data.Length - 1; i++)
            {
                oddCheck ^= data[i];
            }
            if (oddCheck != data[data.Length - 1])
            {
                LogHelper.GetLogger<FrameProtocol>().Error(string.Format("通信层进行就校验出错, Data: {0}，最后一位应为0x {1:x2}，丢弃。"
                    , ByteHelper.Byte2ReadalbeXstring(data)
                    , oddCheck));
                return null;
            }
            //数据正常，去掉头尾返回
            byte[] realData = new byte[data.Length - 2];
            Array.Copy(data, 1, realData, 0, data.Length - 2);
            LogHelper.GetLogger<FrameProtocol>().Debug(string.Format("通信层解包完成, 解包后数据: {0}"
                    , ByteHelper.Byte2ReadalbeXstring(realData) ) );
            return realData;
        }

        /// <summary>
        /// 将数据加包，包首增加应用标志，包尾增加奇偶校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] EnPackage(byte[] data)
        {
            if (data == null)
            {
                LogHelper.GetLogger<FrameProtocol>().Error("通信层待编码数据为空，丢弃。");
                return null;
            }
            byte[] enData = new byte[data.Length + 2];
            enData[0] = AppMark;
            Array.Copy(data, 0, enData, 1, data.Length);  //把需要发送的数据复制到帧数据体中
            //计算奇偶校验和，最后一位不参与奇偶校验
            byte oddCheck = enData[0];
            for (int i = 1; i < enData.Length - 1; i++)
            {
                oddCheck ^= enData[i];
            }
            enData[enData.Length - 1] = oddCheck;
            LogHelper.GetLogger<FrameProtocol>().Debug(string.Format("通信层加包完成, 加包后数据: {0}"
                    , ByteHelper.Byte2ReadalbeXstring(enData) ) );
            return enData;
        }
    }
    /// <summary>
    /// 发送数据包结构
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-14
    /// </remark>
    public struct TxPackage
    {
        /// <summary>
        /// 数据包的目标机
        /// </summary>
        public IPEndPoint Target;

        /// <summary>
        /// 命令体
        /// </summary>
        public byte[] Message;
    }

    /// <summary>
    /// 接收数据包结构
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-14
    /// </remark>
    public struct RxPackage
    {
        /// <summary>
        /// 数据包来源，接收数据时有效，发送数据时为null
        /// </summary>
        public IPEndPoint Source;

        /// <summary>
        /// 命令类型
        /// </summary>
        public byte CmdType;

        /// <summary>
        /// 响应序号
        /// </summary>
        public int Sequence;

        /// <summary>
        /// 错误状态
        /// </summary>
        public byte ErrorState;

        /// <summary>
        /// 错误码
        /// </summary>
        public byte ErrorCode;

        /// <summary>
        /// 数据体
        /// </summary>
        public byte[] Data;
    }
}