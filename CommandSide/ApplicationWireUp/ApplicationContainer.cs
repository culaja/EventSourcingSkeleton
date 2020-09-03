using System.Collections.Generic;
using ApplicationServices;
using EventStoreAdapter;
using InMemoryAdapter;
using NewtonSoftAdapter;

namespace ApplicationWireUp
{
    public sealed class ApplicationContainer
    {
        public CommandHandlerContainer CommandHandlerContainer { get; }
        public DomainEventHandlerContainer DomainEventHandlerContainer { get; }
        public NewtonSoftDomainEventResolver DomainEventResolver { get; }

        private ApplicationContainer(
            CommandHandlerContainer commandHandlerContainer,
            DomainEventHandlerContainer domainEventHandlerContainer,
            NewtonSoftDomainEventResolver domainEventResolver)
        {
            CommandHandlerContainer = commandHandlerContainer;
            DomainEventHandlerContainer = domainEventHandlerContainer;
            DomainEventResolver = domainEventResolver;
        }
        
        public static ApplicationContainer Build(
            string connectionString,
            string eventStoreName,
            IReadOnlyList<AggregateTypeCacheExpiration> aggregateTypeCacheExpirations)
        {
            var store = Store.NewUsing(connectionString, eventStoreName);
            var cachedStore = StoreCache.NewUsing(aggregateTypeCacheExpirations, store);
            var domainEventResolver = NewtonSoftDomainEventResolver.New();
            
            var commandHandlerContainer = CommandHandlerContainerBuilder.New()
                .Use(cachedStore)
                .Build();

            var domainEventHandlerContainer = DomainEventHandlerContainerBuilder.New()
                .Use(cachedStore)
                .Build();
            
            return new ApplicationContainer(
                commandHandlerContainer,
                domainEventHandlerContainer,
                domainEventResolver);
        }
    }
}