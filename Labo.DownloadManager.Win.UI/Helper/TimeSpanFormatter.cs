namespace Labo.DownloadManager.Win.UI.Helper
{
    using System;

    public static class TimeSpanFormatter
    {
        public static string ToString(TimeSpan? timeSpan)
        {
            if (!timeSpan.HasValue || timeSpan == TimeSpan.MaxValue)
            {
                return "???";
            }

            string timeText = timeSpan.ToString();
            int index = timeText.LastIndexOf('.');
            return index > 0 ? timeText.Remove(index) : timeText;
        }
    }
}