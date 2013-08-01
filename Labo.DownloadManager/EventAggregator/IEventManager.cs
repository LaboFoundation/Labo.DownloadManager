namespace Labo.DownloadManager.EventAggregator
{
    public interface IEventManager
    {
        IEventPublisher EventPublisher { get; }

        IEventSubscriber EventSubscriber { get; }
    }
}