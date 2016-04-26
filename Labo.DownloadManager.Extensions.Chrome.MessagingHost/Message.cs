namespace Labo.DownloadManager.Extensions.Chrome.MessagingHost
{
    using System;

    [Serializable]
    public sealed class Message<TValue> : MessageBase
    {
        public TValue Value { get; set; }
    }
}