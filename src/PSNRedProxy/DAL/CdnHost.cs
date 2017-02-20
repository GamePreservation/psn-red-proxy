using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamePreservation.PSNRedProxy.Model;

namespace GamePreservation.PSNRedProxy.DAL
{
    public class CdnHost
    {
        private static Hashtable _cdnhost = new Hashtable();
        private static Hashtable _blur = new Hashtable();
        private static Hashtable _temp = new Hashtable();
        private static object _lock = new object();
        private static CdnHost _instance;

        public Hashtable CdnHostList
        {
            get { return _cdnhost; }
        }

        public static CdnHost Instance()
        {
            if (_instance == null)
                lock (_lock)
                {
                    return _instance = new CdnHost();
                }
            return _instance;
        }

        /// <summary>
        /// Read CDN links
        /// </summary>
        public void ReadCdnConfig()
        {
            _cdnhost.Clear();
            _blur.Clear();
            _temp.Clear();
            ReadCdnConfig(@"Hosts\CdnHosts.ini", true);
            if (AppConfig.Instance().IsUseCustomHosts)
                ReadCdnConfig(@"Hosts\CustomHosts.ini", false);
        }

        /// <summary>
        /// Read the host information
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isPing"></param>
        static void ReadCdnConfig(string path, bool isPing)
        {
            if (!File.Exists(path))
                throw new Exception(path + "配置文件未找到！");

            using (var sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                while (sr.Peek() > 0)
                {
                    var lineStr = sr.ReadLine();
                    if (String.IsNullOrEmpty(lineStr) || lineStr.StartsWith("#"))
                        continue;

                    var t = new Task(delegate()
                    {
                        var isForceUse = lineStr.StartsWith("``");
                        Regex regex = new Regex(@"\s*\s");
                        lineStr = regex.Replace(lineStr.Replace("``", ""), "&");
                        var cdnInfo = lineStr.Split('&');
                        try
                        {
                            if (cdnInfo.Length < 2)
                                return;

                            PingReply reply = null;
                            if (isPing && !isForceUse)
                            {
                                reply = PingHost(cdnInfo[0], cdnInfo[1]);
                                if (reply == null)
                                    return;

                                cdnInfo[0] = reply.Address.ToString();
                            }
                            BuildCdnList(cdnInfo, reply);
                        }
                        catch
                        {
                            return;
                        }
                    });
                    t.Start();
                }
            }
        }

        private static object _buildlock = new object();

        /// <summary>
        /// Returns ping results when compared to established Hashtable
        /// </summary>
        /// <param name="cdnInfo"></param>
        /// <param name="reply"></param>
        static void BuildCdnList(string[] cdnInfo, PingReply reply)
        {
            lock (_buildlock)
            {
                if (reply == null)
                {
                    AddToHashTable(cdnInfo);
                    return;
                }

                if (!_temp.ContainsKey(cdnInfo[1]))
                {
                    AddToHashTable(cdnInfo);
                    _temp.Add(cdnInfo[1], reply.RoundtripTime);
                }
                else if ((long)_temp[cdnInfo[1]] > reply.RoundtripTime)
                {
                    AddToHashTable(cdnInfo);
                    _temp[cdnInfo[1]] = reply.RoundtripTime;
                }
            }
        }

        /// <summary>
        /// The host address and the corresponding domain names in Hashtable
        /// </summary>
        /// <param name="cdnInfo"></param>
        private static void AddToHashTable(string[] cdnInfo)
        {
            if (cdnInfo[1].StartsWith("*."))
                AddToBlur(cdnInfo[1], cdnInfo[0]);
            else
                AddToCdnHost(cdnInfo[1], cdnInfo[0]);
        }

        private static void AddToBlur(string key, string value)
        {
            key = key.Replace("*.", "");
            if (_blur.ContainsKey(key))
                _blur[key] = value;
            else
                _blur.Add(key, value);
        }

        private static void AddToCdnHost(string key, string value)
        {
            if (_cdnhost.ContainsKey(key))
                _cdnhost[key] = value;
            else
                _cdnhost.Add(key, value);
        }

        /// <summary>
        /// Synchronous PING remote host, returns after a successful system and CDN fastest IP address in the table
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        private static PingReply PingHost(string ip, string host)
        {
            if (String.IsNullOrEmpty(ip))
                return null;

            try
            {
                var pinghandle = new Ping();
                var reply = pinghandle.Send(ip, 1000);
                if (reply.Status != IPStatus.Success)
                    return null;

                if (reply.RoundtripTime <= 150)
                    return reply;

                host = host.Replace("*", "1");
                var tempreply = pinghandle.Send(host);
                return tempreply != null && tempreply.Status == IPStatus.Success &&
                       tempreply.RoundtripTime < reply.RoundtripTime
                           ? null
                           : reply;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get CDN address, did not return to the system to resolve addresses
        /// </summary>
        /// <param name="host"></param>
        /// <param name="isCdn"></param>
        /// <returns></returns>
        public IPAddress GetCdnAddress(string host, out bool isCdn)
        {
            isCdn = true;
            if (_cdnhost.ContainsKey(host))
                return IPAddress.Parse(_cdnhost[host].ToString());

            var temphost = host.Substring(host.IndexOf(".") + 1);
            if (_blur.ContainsKey(temphost))
                return IPAddress.Parse(_blur[temphost].ToString());

            isCdn = false;
            return Dns.GetHostEntry(host).AddressList[0];
        }

        /// <summary>
        /// Export a list of the final CDN
        /// </summary>
        /// <returns></returns>
        public bool ExportHost()
        {
            using (StreamWriter sw = new StreamWriter(@"Hosts\ExportCdnHosts.txt"))
            {
                sw.WriteLine("#PSX Download Helper\r\n\r\n");
                foreach (var host in _blur.Keys)
                    sw.WriteLine(_blur[host] + " *." + host);
                foreach (var host in _cdnhost.Keys)
                    sw.WriteLine(_cdnhost[host] + " " + host);
                return true;
            }
        }
    }
}