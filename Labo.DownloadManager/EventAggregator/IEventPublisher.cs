namespace Labo.DownloadManager.EventAggregator
{
    public interface IEventPublisher
    {
        void Publish<TEventMessage>(TEventMessage eventMessage);
    }
}
