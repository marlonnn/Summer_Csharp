using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dart.Snmp;
using System.Net;

namespace Summer.ES.Snmp
{
    public class SnmpAgent
    {
        private Agent agent;

        private IPEndPoint managerIP;

        /// <summary>
        /// agent构造函数
        /// </summary>
        /// <param name="lsnPort">侦听Manager发来的请求消息</param>
        /// <param name="managerIP"></param>
        /// <param name="managerPort"></param>
        public SnmpAgent(int lsnPort,string managerIP,int managerPort)
        {
            agent = new Agent();
            agent.LocalEndPoint = new IPEndPoint(IPAddress.Any, lsnPort);
            this.managerIP = new IPEndPoint(IPAddress.Parse(managerIP), managerPort);
        }
        /// <summary>
        /// Example:
        /// Dart.Snmp.Usage.Identifier, "1.3.6.1.4.1.13933", "", Dart.Snmp.Access.Uninitialized, Dart.Snmp.Status.Uninitialized, "", "alstom", null, "", "", "", "", "", "", "URBALIS-MIB", ""));
        /// Dart.Snmp.Usage.Identifier, "1.3.6.1.4.1.13933.100", "", Dart.Snmp.Access.Uninitialized, Dart.Snmp.Status.Uninitialized, "", "urbalis", null, "", "", "", "", "", "", "URBALIS-MIB", ""));
        /// Dart.Snmp.Usage.Identifier, "1.3.6.1.4.1.13933.100.1", "", Dart.Snmp.Access.Uninitialized, Dart.Snmp.Status.Uninitialized, "", "product", null, "", "", "", "", "", "", "URBALIS-MIB", ""));
        /// Dart.Snmp.Usage.Identifier, "1.3.6.1.4.1.13933.100.2", "", Dart.Snmp.Access.Uninitialized, Dart.Snmp.Status.Uninitialized, "", "software", null, "", "", "", "", "", "", "URBALIS-MIB", ""));
        /// Dart.Snmp.Usage.Object, "1.3.6.1.4.1.13933.100.1.1", "OctetString (SIZE (50))", Dart.Snmp.Access.ReadOnly, Dart.Snmp.Status.Mandatory, @"This is the name of the ALSTOM product (i.e. hardware+software package) without version or index indication. The string is intended for presentation to a human. The string must begin by a number on four characters, followed by a space. This number must consist of four characters between ??and ?? This number must be unique in each subsystem for this string.", "prodName", typeof(Dart.Snmp.SimpleType.OctetString), "", "", "", "", "", "", "URBALIS-MIB", "OCTET STRING (SIZE (50))"));
        /// Dart.Snmp.Usage.Object, "1.3.6.1.4.1.13933.100.1.2", "OctetString (SIZE (160))", Dart.Snmp.Access.ReadOnly, Dart.Snmp.Status.Mandatory, @"This is the description of the ALSTOM product (i.e. the main function) in one sentence. The string is intended for presentation to a human The string must begin by a number on four characters, followed by a space. This number must consist of four characters between ??and ?? This number must be unique in each subsystem for this string.", "prodPresentation", typeof(Dart.Snmp.SimpleType.OctetString), "", "", "", "", "", "", "URBALIS-MIB", "OCTET STRING (SIZE (160))"));
        /// Dart.Snmp.Usage.Object, "1.3.6.1.4.1.13933.100.2.1", "Integer", Dart.Snmp.Access.ReadOnly, Dart.Snmp.Status.Mandatory, "The number of independents software present inside the equipment.", "swNumber", typeof(Dart.Snmp.SimpleType.Integer), "", "", "", "", "", "", "URBALIS-MIB", "INTEGER"));
        /// </summary>
        /// <returns></returns>
        public MibNode CreateMibNode(Usage usage, string oid, string syntax, Access access, Status status, string description, string name, Type valueType, string units, string defVal, string index, string augments, string objects, string notifications, string module, string composedsyntax)
        {
            MibNode node = new MibNode(usage,oid,syntax,access,status,description,name,valueType,units,defVal,index,augments,objects,notifications,module,composedsyntax);
            agent.Mib.Add(oid, node);
            return node;
        }

        /// <summary>
        /// Example:
        /// public static void requestReceived(Dart.Snmp.Agent agent, RequestMessage request, object state)
        ///{
        ///    ResponseMessage response = agent.CreateDefaultResponse(request);
        ///    agent.Send(response, request.Origin);
        /// }
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="state"></param>
        public void Start(AgentMessageReceived worker, object state)
        {
            agent.Start(worker, state);
        }

        public MibNode GetMibNode(string oid)
        {
            MibNode node = null;
            agent.Mib.TryGetValue(oid,out node);
            return node;
        }

        private void TryUpdateVariable(Variable v)
        {
            if (agent.Variables.ContainsKey(v.Id))
            {
                agent.Variables[v.Id] = v;
            }
            else
            {
                agent.Variables.Add(v.Id, v);
            }
        }

        public void TryUpdateVariable(string oid,string value)
        {
            Variable v = new Variable(oid);
            v.Value = new Dart.Snmp.SimpleType.OctetString(value);
            TryUpdateVariable(v);
        }

        public void TryUpdateVariable(string oid, int value)
        {
            Variable v = new Variable(oid);
            v.Value = new Dart.Snmp.SimpleType.Integer(value);
            TryUpdateVariable(v);
        }

        public void SendSpecificTrap(int specificTrapCode, string enterpriseId)
        {
            MessageBase mb = new Dart.Snmp.Trap.SpecificTrap(specificTrapCode, enterpriseId);
            agent.Send(mb, managerIP);
        }

        public void SendNormalTrap(SnmpTrapType trapType, string enterpriseId)
        {
            MessageBase mb = null;
            switch (trapType)
            {
                case SnmpTrapType.WARMUP:
                    mb = new Dart.Snmp.Trap.warmStart(enterpriseId);
                    break;
                case SnmpTrapType.COLDUP:
                    mb = new Dart.Snmp.Trap.coldStart(enterpriseId);
                    break;
                case SnmpTrapType.LINKUP:
                    mb = new Dart.Snmp.Trap.linkUp(enterpriseId);
                    break;
                case SnmpTrapType.LINKDOWN:
                    mb = new Dart.Snmp.Trap.linkDown(enterpriseId);
                    break;
            }
            agent.Send(mb, managerIP);
        }

    }

    public enum SnmpTrapType
    {
        WARMUP,
        COLDUP,
        LINKUP,
        LINKDOWN
    }
}
