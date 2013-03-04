using System;

namespace Labo.DownloadManager.Protocol
{
    public sealed class NetworkCredentialUserName
    {
        public string UserName { get; private set; }

        public string Domain { get; private set; }

        public NetworkCredentialUserName(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            int slashIndex = userName.IndexOf('\\');

            if (slashIndex >= 0)
            {
                UserName= userName.Substring(0, slashIndex);
                Domain = userName.Substring(slashIndex + 1);
            }
            else
            {
                UserName = userName;
                Domain = string.Empty;
            }
        }
    }
}
