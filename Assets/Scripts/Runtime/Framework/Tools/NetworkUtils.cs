using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace HappyCard
{
    public static class NetworkUtils
    {
        /// <summary>
        /// 获取本机上DHCP协议的IP地址，同时与目标IP处于同一个网络
        /// 注：玩家处于同一个局域网下，连接同一个wifi，玩家的ip地址都是处于同一个网段
        /// </summary>
        public static IPAddress GetDhcpIPv4Address()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                NetworkInterface @interface = interfaces[i];

                IPInterfaceProperties properties = @interface.GetIPProperties();

                if (properties == null) { continue; }

                var unicastIPs = properties.UnicastAddresses;

                foreach (var ip in unicastIPs)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork && ip.PrefixOrigin == PrefixOrigin.Dhcp)
                    {
                        return ip.Address;
                    }
                }
            }

            return null;
        }
    }
}
