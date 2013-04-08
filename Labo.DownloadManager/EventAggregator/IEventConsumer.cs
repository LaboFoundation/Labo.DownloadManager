namespace Labo.DownloadManager.EventAggregator
{
    public interface IEventConsumer<in TEventMessage>
    {
        void HandleEvent(TEventMessage eventMessage);
    }
}
