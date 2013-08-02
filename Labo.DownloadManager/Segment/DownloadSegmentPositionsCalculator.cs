using System;

namespace Labo.DownloadManager.Segment
{
    internal sealed class DownloadSegmentPositionsCalculator : IDownloadSegmentPositionsCalculator
    {
        public DownloadSegmentPositions[] Calculate(int minimumSegmentSize, int maximumSegmentCount, int segmentCount, long fileSize)
        {
            if (fileSize <= 0)
            {
                return new[] { new DownloadSegmentPositions(-1, -1) };
            }

            if (fileSize <= minimumSegmentSize)
            {
                return new[] { new DownloadSegmentPositions(0, fileSize - 1) };
            }

            segmentCount = Math.Min(maximumSegmentCount, segmentCount == 0 ? 1 : segmentCount);

            long segmentSize = fileSize/segmentCount;
            while (segmentCount > 1 && segmentSize < minimumSegmentSize)
            {
                segmentCount--; 
                segmentSize = fileSize/segmentCount;
            }

            DownloadSegmentPositions[] segmentPositions = new DownloadSegmentPositions[segmentCount];
            for (int i = 0; i < segmentCount; i++)
            {
                long startPosition = i*segmentSize;
                long endPosition;
                if(i == segmentCount - 1)
                {
                    endPosition = fileSize - 1;
                }
                else
                {
                    endPosition = startPosition + segmentSize - 1;                    
                }
                segmentPositions[i] = new DownloadSegmentPositions(startPosition, endPosition);
            }
            return segmentPositions;
        }
    }
}