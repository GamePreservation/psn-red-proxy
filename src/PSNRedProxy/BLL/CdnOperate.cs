using System.Net;
using GamePreservation.PSNRedProxy.DAL;

namespace GamePreservation.PSNRedProxy.BLL
{
    public class CdnOperate
    {
        private static readonly CdnHost CdnInstance = CdnHost.Instance();

        /// <summary>
        /// Read the CDN link
        /// </summary>
        public static void ReadCdnConfig()
        {
            CdnInstance.ReadCdnConfig();
        }

        /// <summary>
        /// Get DNS resolution, if there is a host to return CDN host address, or return to the default resolution
        /// </summary>
        /// <param name="host"></param>
        /// <param name="isCdn"></param>
        /// <returns></returns>
        public static IPAddress GetCdnAddress(string host,out bool isCdn)
        {
            return CdnInstance.GetCdnAddress(host,out isCdn);
        }
    }
}
