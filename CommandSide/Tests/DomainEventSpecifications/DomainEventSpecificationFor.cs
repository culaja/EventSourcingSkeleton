using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Commanding;
using InMemoryAdapter;
using Ports;
using Xunit;

namespace Tests.DomainEventSpecifications
{
    public abstract class DomainEventSpecificationFor<T> 
        where T : IDomainEvent
    {
        private readonly InMemoryStore _inMemoryStore = InMemoryStore.New();
        protected IStore Store { get; }

        protected DomainEventSpecificationFor()
        {
            Store = _inMemoryStore;
            ApplyGivenEventsToTheStore();
            HandleDomainEventAndStoreResult();
        }
        
        private void ApplyGivenEventsToTheStore()
        {
            _inMemoryStore.AddDomainEventsExplicitly(WhenGiven);
        }
        
        private void HandleDomainEventAndStoreResult() => 
            Result = By().Handle(AfterHandling).Result;
        
        protected abstract IReadOnlyList<IDomainEvent> WhenGiven { get; }
        protected IReadOnlyList<IDomainEvent> Events(params IDomainEvent[] domainEvents) => domainEvents;
        protected IReadOnlyList<IDomainEvent> NoEvents => new List<IDomainEvent>();

        protected abstract T AfterHandling { get; }
        
        protected abstract IDomainEventHandler By();
        
        protected Result Result { get; private set; }
        
        protected IReadOnlyList<IDomainEvent> ProducedEvents => _inMemoryStore.AllProducedDomainEvents;
        
        [Fact]
        public void Checks()
        {
            foreach (var assert in Outcome)
            {
                assert();
            }
        }
        
        protected abstract IReadOnlyList<Action> Outcome { get; }
        
        protected IReadOnlyList<Action> Is(params Action[] asserts) => asserts.ToList();
    }
}