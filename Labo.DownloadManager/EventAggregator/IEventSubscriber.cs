namespace Labo.DownloadManager.EventAggregator
{
    public interface IEventSubscriber
    {
        void RegisterConsumer<TEventMessage>(IEventConsumer<TEventMessage> consumer);
    }
}