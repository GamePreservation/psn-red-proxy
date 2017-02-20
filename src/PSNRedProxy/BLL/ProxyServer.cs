﻿using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GamePreservation.PSNRedProxy.BLL
{
    public class ProxyServer
    {
        /// <summary>
        /// Get local IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetHostIp()
        {
            var ip = Dns.GetHostEntry(Dns.GetHostName());
            var iplist = ip.AddressList.Where(p => p.AddressFamily == AddressFamily.InterNetwork).ToArray();
            return iplist;
        }
    }
}
