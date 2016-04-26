namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;

    internal sealed class SegmentDownloadRateCalculator : ISegmentDownloadRateCalculator
    {
        private const int ONE_SECOND_TICKS = 10000000;

        private readonly LinkedList<Tuple<long, long>> m_Bytes = new LinkedList<Tuple<long, long>>();

        public double? CalculateDownloadRate(long currentPosition)
        {
            long ticks = DateTime.UtcNow.Ticks;

            m_Bytes.AddLast(new Tuple<long, long>(currentPosition, ticks));

            if (m_Bytes.Count < 2)
            {
                return null;
            }

            if (m_Bytes.Count > 2)
            {
                // ReSharper disable once PossibleNullReferenceException
                if (m_Bytes.Last.Value.Item2 - m_Bytes.First.Next.Value.Item2 >= ONE_SECOND_TICKS)
                {
                    m_Bytes.RemoveFirst();
                }
            }

            Tuple<long, long> lastItem = m_Bytes.Last.Value;
            Tuple<long, long> firstItem = m_Bytes.First.Value;
            long totalTicks = lastItem.Item2 - firstItem.Item2;
            if (totalTicks < ONE_SECOND_TICKS)
            {
                return null;
            }

            double totalSeconds = (double)totalTicks / ONE_SECOND_TICKS;
            long totalBytes = lastItem.Item1 - firstItem.Item1;
            return totalBytes / totalSeconds;
        }
    }
}