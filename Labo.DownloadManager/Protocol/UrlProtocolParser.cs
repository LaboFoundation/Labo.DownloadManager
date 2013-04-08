using System;
using System.Text.RegularExpressions;

namespace Labo.DownloadManager.Protocol
{
    internal sealed class UrlProtocolParser : IUrlProtocolParser
    {
        private readonly Regex m_UrlRegex = new Regex(@"^(?<proto>\w+)://[^/]+?", RegexOptions.Singleline | RegexOptions.Compiled);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public string Parse(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            Match match = m_UrlRegex.Match(url);
            if (match.Success)
            {
                return m_UrlRegex.Match(url).Result("${proto}").ToLowerInvariant();
            }
            return null;
        }
    }
}