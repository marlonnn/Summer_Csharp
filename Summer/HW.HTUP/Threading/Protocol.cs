
using System.Net;

namespace HW.HTUP.Threading
{
    /// <summary>
    /// 应用层接收协议，基础抽象类，不同协议版本从此继承
    /// </summary>
    public abstract class RxBaseProtocol
    {
        public abstract void Decode(IPEndPoint source, byte[] data);
    }

    /// <summary>
    /// 应用层发送协议，基础抽象类，不同协议版本从此继承
    /// </summary>
    public abstract class TxBaseProtocol
    {
        public abstract void Encode(IPEndPoint target, byte cmdCode, int Sequence, byte[] cmdData);
    }

}