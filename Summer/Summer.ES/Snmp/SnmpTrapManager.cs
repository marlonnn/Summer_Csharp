using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dart.Snmp;
using System.Net;

namespace Summer.ES.Snmp
{
    public class SnmpTrapManager
    {
        Manager manager;

        public SnmpTrapManager(int lsnPort)
        {
            manager = new Manager();
            manager.LocalEndPoint = new IPEndPoint(IPAddress.Any, lsnPort);
        }

        /// <summary>
        /// Example:
        /// private void button1_Click(object sender, EventArgs e)
        ///{
        ///     //Start listening for notifications
        ///     manager1.Start(manager1_NotificationReceived, null);
        ///}
        ///private void manager1_NotificationReceived(Manager manager, MessageBase message, object state)
        ///{
        ///    //Marshal message to the UI thread using the Message event
        ///    if (message is Trap1Message)
        ///        manager.Marshal(new MessageBase[] { message }, "trap1", null);
        ///    else if (message is Trap2Message)
        ///        manager.Marshal(new MessageBase[] { message }, "trap2", null);
        ///    else if (message is InformMessage)
        ///    {
        ///        manager.Marshal(new MessageBase[] { message }, "inform", null);
        ///        //Send response to inform message origin
        ///        manager.Send(new ResponseMessage(message as InformMessage, null), message.Origin);
        ///    }
        ///}
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="state"></param>
        public void Start(ManagerMessageReceived worker, object state)
        {
            manager.Start(worker, state);
        }
    }
}
