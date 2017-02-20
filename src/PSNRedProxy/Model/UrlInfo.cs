namespace GamePreservation.PSNRedProxy.Model
{
    public class UrlInfo
    {
        public UrlInfo()
        {
            SetLixian = false;
            IsLixian = false;
        }

        public UrlInfo(string psnurl, string replacepath, string marktxt, bool isLixian = false, string lixianurl = null)
        {
            SetLixian = false;
            PsnUrl = psnurl;
            ReplacePath = replacepath;
            MarkTxt = marktxt;
            LixianUrl = lixianurl;
            IsLixian = isLixian;
        }

        /// <summary>
        /// PSN connection
        /// </summary>
        public string PsnUrl { get; set; }

        /// <summary>
        /// Replace address
        /// </summary>
        public string ReplacePath { get; set; }

        /// <summary>
        /// Notes on the current connection information
        /// </summary>
        public string MarkTxt { get; set; }

        /// <summary>
        /// Offline address
        /// </summary>
        public string LixianUrl { get; set; }

        /// <summary>
        /// Is the line
        /// </summary>
        public bool IsLixian { get; set; }

        /// <summary>
        /// Increase offline
        /// </summary>
        public bool SetLixian { get; set; }

        /// <summary>
        /// Is a CDN address
        /// </summary>
        public bool IsCdn { get; set; }

        /// <summary>
        /// Host address
        /// </summary>
        public string Host { get; set; }
    }
}