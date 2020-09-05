using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Commanding;
using InMemoryAdapter;
using Ports;
using Xunit;

namespace Tests.CommandSpecifications
{
    public abstract class CommandSpecificationFor<T> 
        where T : ICommand
    {
        private readonly InMemoryStore _inMemoryStore = InMemoryStore.New();
        protected IStore Store { get; }

        protected CommandSpecificationFor()
        {
            Store = _inMemoryStore;
            ApplyGivenEventsToTheStore();
            ExecuteCommandAndStoreResult();
        }
        
        private void ApplyGivenEventsToTheStore()
        {
            _inMemoryStore.AddDomainEventsExplicitly(WhenGiven);
        }
        
        private void ExecuteCommandAndStoreResult() => 
            Result = By().Execute(AfterExecuting).Result;
        
        protected abstract IReadOnlyList<IDomainEvent> WhenGiven { get; }
        protected IReadOnlyList<IDomainEvent> Events(params IDomainEvent[] domainEvents) => domainEvents;
        protected IReadOnlyList<IDomainEvent> NoEvents => new List<IDomainEvent>();

        protected abstract T AfterExecuting { get; }
        
        protected abstract ICommandHandler By();
        
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