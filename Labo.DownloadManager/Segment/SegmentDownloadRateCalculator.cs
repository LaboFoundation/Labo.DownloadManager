using System;

namespace Labo.DownloadManager.Segment
{
    internal sealed class SegmentDownloadRateCalculator : ISegmentDownloadRateCalculator
    {
        private DateTime? m_LastCalculation;
        private long m_LastPosition;

        public SegmentDownloadRateCalculator(long position)
        {
            m_LastPosition = position;
        }

        public double? CalculateDownloadRate(long currentPosition)
        {
            DateTime now = DateTime.Now;
            if (!m_LastCalculation.HasValue)
            {
                m_LastCalculation = now;
                m_LastPosition = currentPosition;
                return null;
            }

            m_LastCalculation = now;

            double seconds = now.Subtract(m_LastCalculation.Value).TotalSeconds;
            if (Math.Abs(seconds - 0) < 0.00001)
            {
                return null;
            }

            double rate = (currentPosition - m_LastPosition)/seconds;

            m_LastPosition = currentPosition;

            return rate;
        }
    }
}