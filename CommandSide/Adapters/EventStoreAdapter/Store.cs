﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreAdapter.ConnectionProviders;
using Framework;
using Framework.Commanding;
using Ports;
using static Domain.Errors.General;
using static Framework.Result;

namespace EventStoreAdapter
{
    public sealed class Store : IStore
    {
        private readonly EventStoreAppender _eventStoreAppender;

        private Store(IConnectionProvider connectionProvider, string eventStoreName)
        {
            _eventStoreAppender = new EventStoreAppender(
                connectionProvider,
                eventStoreName);
        }
        
        public static Store NewUsing(string connectionString, string eventStoreName) =>
            new Store(new RealEventStoreConnectionProvider(connectionString), eventStoreName);
        
        public async Task<Result<T>> Get<T>(IAggregateId aggregateId) where T : AggregateRoot, new()
        {
            var domainEvents = await _eventStoreAppender.AsyncLoadAllEventsFor(aggregateId);
            if (domainEvents.Count > 0)
            {
                var aggregateRoot = ReconstructAggregateFrom<T>(domainEvents);
                return Ok(aggregateRoot);
            }

            return Fail<T>(AggregateNotFoundInStore(aggregateId.Id));
        }
        
        private static T ReconstructAggregateFrom<T>(IReadOnlyList<IDomainEvent> domainEvents) where T : AggregateRoot, new()
        {
            var aggregateRoot = new T();
            aggregateRoot.ApplyAll(domainEvents);
            return aggregateRoot;
        }

        public async Task<Result> SaveChanges<T>(T aggregateRoot) where T : AggregateRoot
        {
            try
            {
                await _eventStoreAppender.AppendAsync(aggregateRoot.Id, aggregateRoot.UncommittedDomainEvents, aggregateRoot.OriginalVersion);
                aggregateRoot.ClearUncommittedDomainEvents();
                return Ok();
            }
            catch (VersionMismatchException)
            {
                return Fail(AggregateVersionMismatch(aggregateRoot.Id.Id, aggregateRoot.OriginalVersion));
            }
        }
    }
}