
using System.Net;

namespace HappyCard
{
    public interface ISyncService
    {
        void AddHostEP(int playerID, IPEndPoint endPoint);

        IPEndPoint HostEP { get; }

        void SendBroadcast(string eventName, object data);

        void SendTo(int receiverID, string eventName, object data);
    }
}
