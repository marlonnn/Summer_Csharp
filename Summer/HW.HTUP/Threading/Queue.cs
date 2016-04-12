
using Summer.System.Collections.Concurrent;

namespace HW.HTUP.Threading
{
    /// <summary>
    /// 接收数据队列
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-14
    /// </remark>
    public class RxQueue : ConcurrentQueue<RxPackage>
    {
    }
    /// <summary>
    /// 发送数据队列
    /// </summary>
    /// <remark>
    /// 公司：CASCO
    /// 作者：张立鹏
    /// 创建日期：2013-5-14
    /// </remark>
    public class TxQueue : ConcurrentQueue<TxPackage>
    {
    }
}