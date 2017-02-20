using GamePreservation.PSNRedProxy.DAL;
using GamePreservation.PSNRedProxy.Model;

namespace GamePreservation.PSNRedProxy.BLL
{
    public class UrlOperate
    {
        private static readonly HashUrl HashurlOperate = HashUrl.Instance();

        /// <summary>
        /// Find local replace URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string MatchFile(string url)
        {
            return HashurlOperate.PsnLocalPath(url);
        }

        /// <summary>
        /// Get replaces the local connection paths exist, there are no returns an empty
        /// </summary>
        /// <param name="psnurl"></param>
        /// <returns></returns>
        public static string PsnLocalPath(string psnurl)
        {
            return HashurlOperate.PsnLocalPath(psnurl);
        }
    }
}
