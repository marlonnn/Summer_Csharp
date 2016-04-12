using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW.HTUP.Threading;

namespace HW.HTUP.Model
{
    /// <summary>
    /// 接收到新消息后的处理接口
    /// </summary>
    public interface IObserveRxMessage
    {
        void OnNewMessage(RxPackage rxPackage);
    }
}
