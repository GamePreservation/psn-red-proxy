using System;
using System.Linq;
using System.Text.RegularExpressions;
using GamePreservation.PSNRedProxy.Model;

namespace GamePreservation.PSNRedProxy.BLL
{
    public class MonitorLog
    {
        /// <summary>
        /// Match links
        /// </summary>
        /// <returns></returns>
        public static bool RegexUrl(string urls)
        {
            var rules = AppConfig.Instance().Rule.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (rules.Length <= 0 || String.IsNullOrEmpty(urls))
                return false;

            return
                rules.Select(rule => new Regex(rule.ToLower().Replace(".", @"\.").Replace("*", ".*?")))
                     .Any(regex => regex.Match(urls.ToLower()).Success);
        }
    }
}
