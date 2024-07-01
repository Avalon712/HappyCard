using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HappyCard
{
    public sealed class TcpSyncService : ISyncService
    {
        public IPEndPoint HostEP { get; private set; }

        public void AddHostEP(int playerID, IPEndPoint endPoint)
        {
            
        }

        public void SendBroadcast(string eventName, object data)
        {
            
        }

        public void SendTo(int playerID, string eventName, object data)
        {
            
        }
    }
}
