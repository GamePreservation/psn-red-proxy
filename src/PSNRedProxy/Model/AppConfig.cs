namespace GamePreservation.PSNRedProxy.Model
{
    public class AppConfig
    {
        private static AppConfig _instance;
        private static readonly object Lock = new object();
        public static AppConfig Instance()
        {
            if (_instance == null)
                lock (Lock)
                {
                    _instance = new AppConfig();
                }

            return _instance;
        }

        /// <summary>
        /// Matching rules
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// Connection mode
        /// </summary>
        public int ConnType { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// Whether to use a custom host address
        /// </summary>
        public bool IsUseCustomHosts { get; set; }

        /// <summary>
        /// Whether to automatically find the local file to replace
        /// </summary>
        public bool IsAutoFindFile { get; set; }

        /// <summary>
        /// Local override directory
        /// </summary>
        public string LocalFileDirectory { get; set; }
    }
}