using System;
namespace Labo.DownloadManager
{
    internal sealed class DownloadSegmentPositionsCalculator : IDownloadSegmentPositionsCalculator
    {
        public DownloadSegmentPositions[] Calculate(int minimumSegmentSize, int maximumSegmentCount, int segmentCount, long fileSize)
        {
            segmentCount = Math.Min(maximumSegmentCount, segmentCount);

            if (fileSize <= minimumSegmentSize)
            {
                return new[] { new DownloadSegmentPositions { StartPosition = 0, EndPosition = fileSize } };
            }

            long segmentSize = fileSize/segmentCount;
            while (segmentCount > 1 && segmentSize < minimumSegmentSize)
            {
                segmentCount--; 
                segmentSize = fileSize/segmentCount;
            }

            DownloadSegmentPositions[] segmentPositions = new DownloadSegmentPositions[segmentCount];
            for (int i = 0; i < segmentCount; i++)
            {
                DownloadSegmentPositions segmentPosition = new DownloadSegmentPositions();
                segmentPosition.StartPosition = i*segmentSize;
                if(i == segmentCount - 1)
                {
                    segmentPosition.EndPosition = fileSize;
                }
                else
                {
                    segmentPosition.EndPosition = segmentPosition.StartPosition + segmentSize;                    
                }
                segmentPositions[i] = segmentPosition;
            }
            return segmentPositions;
        }
    }
}