using System.Collections.Generic;

namespace Labo.DownloadManager.Tests.FileServer
{
    internal sealed class Logger
    {
        private readonly ICollection<ILoggingSource> m_LoggingSources;

        public Logger(ICollection<ILoggingSource> loggingSources)
        {
            m_LoggingSources = loggingSources;
        }

        public void Log(string message)
        {
            foreach (ILoggingSource loggingSource in m_LoggingSources)
            {
                loggingSource.Write(message);
            }
        }
    }
}
