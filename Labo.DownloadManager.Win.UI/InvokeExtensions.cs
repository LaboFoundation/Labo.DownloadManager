namespace Labo.DownloadManager.Win.UI
{
    using System;
    using System.ComponentModel;

    public static class InvokeExtensions
    {
        public static void UpdateControlThreadSafely<TControl>(this TControl control, Action<TControl> action)
            where TControl : ISynchronizeInvoke
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke((Action)(() => action(control)), new object[0]);
            }
            else
            {
                action(control);
            }
        }
    }
}