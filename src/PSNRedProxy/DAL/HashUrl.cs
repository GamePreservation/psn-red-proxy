using System;
using System.Collections;
using System.IO;
using System.Linq;
using GamePreservation.PSNRedProxy.Model;

namespace GamePreservation.PSNRedProxy.DAL
{
    public class HashUrl
    {
        public static Hashtable UrlList;
        private static HashUrl _instance;
        private static readonly object Lock = new object();

        public static HashUrl Instance()
        {
            if (_instance == null)
                lock (Lock)
                {
                    _instance = new HashUrl();
                    if (UrlList == null)
                        UrlList = new Hashtable();
                }
            return _instance;
        }

        /// <summary>
        /// Increased psnurl corresponding to local file
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public bool PushUrl(UrlInfo ui)
        {
            try
            {
                if (UrlList.ContainsKey(ui.PsnUrl))
                    UrlList[ui.PsnUrl] = ui.ReplacePath;
                else
                    UrlList.Add(ui.PsnUrl, ui.ReplacePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get replaces the local connection paths exist, there are no returns an empty
        /// </summary>
        /// <param name="psnurl"></param>
        /// <returns></returns>
        public string PsnLocalPath(string psnurl)
        {
            try
            {
                if (!AppConfig.Instance().IsAutoFindFile)
                    return String.Empty;

                string filename = GetUrlFileName(psnurl);
                if (!String.IsNullOrEmpty(filename))
                {
                    string localFolderPath = AppConfig.Instance().LocalFileDirectory;
                    var localFilePath = Directory.EnumerateFiles(localFolderPath, filename, SearchOption.AllDirectories).FirstOrDefault();
                    return localFilePath ?? string.Empty;
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the URL of the file name
        /// </summary>
        /// <param name="psnurl"></param>
        /// <returns></returns>
        public string GetUrlFileName(string psnurl)
        {
            var rules = AppConfig.Instance().Rule.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
            if (rules.Count <= 0)
                return string.Empty;

            rules = rules.Select(r => r.Replace("*", "")).ToList();
            string filename = string.Empty;
            rules.ForEach(r =>
            {
                if (psnurl.IndexOf(r) > 0)
                {
                    filename = psnurl.Substring(0, psnurl.IndexOf(r) + r.Length);
                    filename = filename.Substring(filename.LastIndexOf("/") + 1);
                }
            });

            return filename;
        }
    }
}