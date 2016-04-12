using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dart.Snmp;
using System.Net;

namespace Summer.ES.Snmp
{
    public class SnmpManager
    {
        Manager manager;

        IPEndPoint agentIP;

        public SnmpManager(string agentIP, int agentPort)
        {
            manager = new Manager();
            this.agentIP = new IPEndPoint(IPAddress.Parse(agentIP), agentPort);
        }

        public void SendGetRequest(Variable variable)
        {
            manager.Start( (managerSlave, state) => {
                GetMessage request = new GetMessage();
                request.Variables.Add(state as Variable);
                managerSlave.Send(request, agentIP);
                

            }, variable);
        }
    }
}
