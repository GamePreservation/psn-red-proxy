using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using CommandLine;
using GamePreservation.PSNRedProxy.BLL;
using GamePreservation.PSNRedProxy.HttpsHelp;
using GamePreservation.PSNRedProxy.Model;

namespace GamePreservation.PSNRedProxy
{
    class Program
    {
        private static HttpListenerHelp _listener;
        private static object _outputLock = new object();

        static void Main(string[] args)
        {
            var options = new MyOptions();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine(options.GetUsage());
                return;
            }

            CdnOperate.ReadCdnConfig();

            var config = AppConfig.Instance();
            config.Rule = "*.pkg*|*.pup*|*.json*";
            config.ConnType = 0;
            config.LocalFileDirectory = options.LocalFolder;
            config.Port = options.Port;
            config.IsAutoFindFile = !string.IsNullOrEmpty(options.LocalFolder);

            var address = IPAddress.Parse(options.IPAddress);
            var port = options.Port;
            _listener = new HttpListenerHelp(address, port, AddUrl);
            _listener.Start();

            Application.Run(new MyAppContext());
        }

        static void AddUrl(UrlInfo urlinfo)
        {
            if (!MonitorLog.RegexUrl(urlinfo.PsnUrl))
                return;

            lock (_outputLock)
            {
                Console.ForegroundColor = string.IsNullOrEmpty(urlinfo.ReplacePath) ? ConsoleColor.DarkMagenta : ConsoleColor.Blue;
                Console.WriteLine(TrimUrlQuery(urlinfo.PsnUrl));
                Console.ResetColor();
            }
        }

        private static string TrimUrlQuery(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return uri.GetComponents(UriComponents.Host | UriComponents.Path | UriComponents.Scheme, UriFormat.Unescaped);
            }
            else
            {
                return url;
            }
        }
    }

    class MyAppContext : ApplicationContext
    {
    }

    class MyOptions
    {
        [Option('i', "ip", HelpText = "IP Address", Required = true)]
        public string IPAddress { get; set; }

        [Option('p', "port", HelpText = "Port", Required = false, DefaultValue = 8080)]
        public int Port { get; set; }

        [Option('l', "LocalFolder", HelpText = "Path to local folder cache", Required = false, DefaultValue = "")]
        public string LocalFolder { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("PSX Download Helper CLI 1.0");
            usage.AppendLine("Read user manual for usage instructions...");
            return usage.ToString();
        }
    }
}
