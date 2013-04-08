using System;

namespace Labo.DownloadManager.EventAggregator
{
    internal sealed class EventManager : IEventManager
    {
        private static readonly Lazy<EventSubscriptionProvider> s_EventSubscriber = new Lazy<EventSubscriptionProvider>(() => new EventSubscriptionProvider(), true);
        private static readonly Lazy<IEventPublisher> s_EventPublisher = new Lazy<IEventPublisher>(() => new EventPublisher(s_EventSubscriber.Value), true);
        
        public IEventPublisher EventPublisher { get { return s_EventPublisher.Value; } }

        public IEventSubscriber EventSubscriber { get { return s_EventSubscriber.Value; } }
    }
}
