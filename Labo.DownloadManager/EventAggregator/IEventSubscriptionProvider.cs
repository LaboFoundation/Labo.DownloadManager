using System.Collections.Generic;

namespace Labo.DownloadManager.EventAggregator
{
    public interface IEventSubscriptionProvider : IEventSubscriber
    {
        IList<IEventConsumer<TEventMessage>> GetSubscriptions<TEventMessage>();
    }
}
