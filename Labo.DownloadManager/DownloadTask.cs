using System.Collections.Generic;
using System.IO;
using Labo.DownloadManager.Protocol;

namespace Labo.DownloadManager
{
    public sealed class DownloadTask : IDownloadTask
    {
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;
        private readonly IDownloadSegmentPositionsCalculator m_DownloadSegmentCalculator;
        private readonly ILocalFileAllocator m_LocalFileAllocator;
        private readonly DownloadFile m_File;
        private readonly int m_SegmentCount;

        public DownloadTask(INetworkProtocolProviderFactory networkProtocolProviderFactory, 
            IDownloadSegmentPositionsCalculator downloadSegmentCalculator, 
            ILocalFileAllocator localFileAllocator,
            DownloadFile file, int segmentCount)
        {
            m_NetworkProtocolProviderFactory = networkProtocolProviderFactory;
            m_DownloadSegmentCalculator = downloadSegmentCalculator;
            m_LocalFileAllocator = localFileAllocator;
            m_File = file;
            m_SegmentCount = segmentCount;
        }

        public void StartDownload()
        {
            INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(m_File.Url);
            RemoteFileInfo remoteFileInfo = networkProtocolProvider.GetRemoteFileInfo(m_File);
            DownloadSegmentPositions[] segmentPositionInfos = m_DownloadSegmentCalculator.Calculate(200, 5, m_SegmentCount, remoteFileInfo.FileSize);
            LocalFileInfo localFileInfo = m_LocalFileAllocator.AllocateFile(remoteFileInfo.FileName, remoteFileInfo.FileSize);

            using (FileStream fs = new FileStream(localFileInfo.FileName, FileMode.Open, FileAccess.Write))
            {
                SegmentWriter segmentWriter = new SegmentWriter(fs);
                IList<ISegmentDownloadTask> segmentDownloadTasks = new List<ISegmentDownloadTask>(segmentPositionInfos.Length);
                for (int i = 0; i < segmentPositionInfos.Length; i++)
                {
                    DownloadSegmentPositions segmentPosition = segmentPositionInfos[i];
                    segmentDownloadTasks.Add(new DoubleBufferSegmentDownloadTask(8192, new SegmentDownloader(networkProtocolProvider.CreateStream(m_File, segmentPosition.StartPosition, segmentPosition.EndPosition), segmentPosition), segmentWriter));
                }

                SegmentDownloadManager segmentDownloadManager = new SegmentDownloadManager(segmentDownloadTasks);
                segmentDownloadManager.Start();
            }
        }
    }
}
