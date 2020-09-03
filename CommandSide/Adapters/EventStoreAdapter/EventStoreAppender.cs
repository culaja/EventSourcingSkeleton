using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using EventStoreAdapter.ConnectionProviders;
using EventStoreAdapter.Serialization;
using Framework;

namespace EventStoreAdapter
{
    internal sealed class EventStoreAppender
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly string _eventStoreName;

        public EventStoreAppender(
            IConnectionProvider connectionProvider,
            string eventStoreName)
        {
            _connectionProvider = connectionProvider;
            _eventStoreName = eventStoreName;
        }
        
        public async Task<IReadOnlyList<IDomainEvent>> AsyncLoadAllEventsFor(IAggregateId aggregateId)
        {
            var connection = await _connectionProvider.GrabConnection();
            var resolvedEvents = await connection.ReadAllStreamEventsForward(aggregateId.ToStreamName(_eventStoreName));
            return resolvedEvents.Select(e => e.Event.ToDomainEvent()).ToList();
        }

        public async Task AppendAsync(
            IAggregateId aggregateId,
            IReadOnlyList<IDomainEvent> domainEvents,
            long expectedVersion)
        {
            if (domainEvents.Count > 0)
            {
                var connection = await _connectionProvider.GrabConnection();
                var results = await connection.ConditionalAppendToStreamAsync(
                    aggregateId.ToStreamName(_eventStoreName),
                    expectedVersion,
                    domainEvents.Select(e => e.ToEventData()));

                switch (results.Status)
                {
                    case ConditionalWriteStatus.Succeeded:
                        break;
                    case ConditionalWriteStatus.VersionMismatch:
                        throw new VersionMismatchException(aggregateId.ToStreamName(_eventStoreName), expectedVersion);
                    case ConditionalWriteStatus.StreamDeleted:
                        throw new StreamDeletedException(aggregateId.ToStreamName(_eventStoreName));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(results.Status), results.Status.ToString());
                }
            }
        }
    }
}