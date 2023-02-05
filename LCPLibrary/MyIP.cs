using System;
using System.Net;

namespace LCPLibrary
{
    public static class MyIP
    {
        public static IPAddress? IPv6
        {
            get
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        return ip;
                return null;
            }
        }
        public static IPAddress? IPv4
        {
            get
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return ip;
                return null;
            }
        }
    }
}
