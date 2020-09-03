using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Commanding;
using Ports;
using static Domain.Errors.General;
using static Framework.Result;

namespace InMemoryAdapter
{
    public sealed class InMemoryStore : IStore
    {
        private readonly Dictionary<string, List<IDomainEvent>> _domainEventsPerAggregate = new Dictionary<string, List<IDomainEvent>>();
        private readonly List<IDomainEvent> _allProducedDomainEvents = new List<IDomainEvent>();
        public static InMemoryStore New() => new InMemoryStore();

        public IReadOnlyList<IDomainEvent> AllProducedDomainEvents => _allProducedDomainEvents;

        public void AddDomainEventsExplicitly(IReadOnlyList<IDomainEvent> domainEvents)
        {
            foreach (var e in domainEvents)
            {
                if (!_domainEventsPerAggregate.TryGetValue(e.AggregateId, out var aggregateDomainEvents))
                {
                    aggregateDomainEvents = new List<IDomainEvent>();
                    _domainEventsPerAggregate.Add(e.AggregateId, aggregateDomainEvents);
                }
                
                aggregateDomainEvents.Add(e);
            }
        }

        public Task<Result<T>> Get<T>(IAggregateId aggregateId) where T : AggregateRoot, new()
        {
            if (!_domainEventsPerAggregate.TryGetValue(aggregateId.Id, out var domainEvents))
            {
                return Task.FromResult(Fail<T>(AggregateNotFoundInStore(aggregateId.Id)));
            }

            var optionalAggregateRoot = ReconstructAggregateFrom<T>(domainEvents);
            return optionalAggregateRoot.Unwrap(
                aggregateRoot => Task.FromResult(Ok(aggregateRoot)),
                () => throw new InvalidOperationException($"Unable to find any events for aggregate with ID '{aggregateId}'."));
        }

        public Task<Result> SaveChanges<T>(T aggregateRoot) where T : AggregateRoot
        {
            if (!_domainEventsPerAggregate.TryGetValue(aggregateRoot.Id.Id, out var domainEvents))
            {
                domainEvents = new List<IDomainEvent>();
                _domainEventsPerAggregate.Add(aggregateRoot.Id.Id, domainEvents);
            }

            if (aggregateRoot.OriginalVersion != domainEvents.Count - 1)
            {
                return Task.FromResult(Fail(AggregateVersionMismatch(
                    aggregateRoot.Id.Name, 
                    aggregateRoot.OriginalVersion)));
            }
            
            domainEvents.AddRange(aggregateRoot.UncommittedDomainEvents);
            _allProducedDomainEvents.AddRange(aggregateRoot.UncommittedDomainEvents);
            aggregateRoot.ClearUncommittedDomainEvents();
            
            return Task.FromResult(Ok());
        }

        private static Optional<T> ReconstructAggregateFrom<T>(IReadOnlyList<IDomainEvent> domainEvents) where T : AggregateRoot, new()
        {
            if (domainEvents.Count > 0)
            {
                var aggregateRoot = new T();
                aggregateRoot.ApplyAll(domainEvents);
                return aggregateRoot;
            }
            
            return Optional<T>.None;
        }
    }
}