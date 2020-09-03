using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.Commanding
{
    public abstract class AggregateRoot
    {
        private Optional<IAggregateId> _maybeAggregateId = Optional<IAggregateId>.None;

        public IAggregateId Id => _maybeAggregateId
            .Ensure(m => m.HasValue,
                () => throw new InvalidOperationException("Aggregate Id needs to be set during object creation in order to use the aggregate."))
            .Value;
        
        public long Version { get; private set; } = -1;

        public long OriginalVersion => Version - UncommittedDomainEvents.Count;
        
        private readonly List<IDomainEvent> _uncommittedDomainEvents = new List<IDomainEvent>();
        public IReadOnlyList<IDomainEvent> UncommittedDomainEvents => _uncommittedDomainEvents;

        protected void SetIdentity(IAggregateId aggregateId) =>
            _maybeAggregateId = Optional<IAggregateId>.From(aggregateId);

        protected abstract void When(IDomainEvent domainEvent);
        
        public void ClearUncommittedDomainEvents()
        {
            _uncommittedDomainEvents.Clear();
        }
        
        public AggregateRoot ApplyAll(IReadOnlyList<IDomainEvent> domainEvents)
        {
            foreach (var e in domainEvents)
            {
                ApplyChange(e, false);
            }
            
            return this;
        }
        
        protected AggregateRoot ApplyChange(IDomainEvent e)
        {
            ApplyChange(e, true);
            return this;
        }
        
        private void ApplyChange(IDomainEvent e, bool isNew)
        {
            When(e);
            IncrementedVersion();
            if (isNew)
            {
                _uncommittedDomainEvents.Add(e);
            }
        }
        
        private void IncrementedVersion() => ++Version;
    }
}