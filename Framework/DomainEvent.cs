﻿using System.Collections.Generic;

namespace Framework
{
    public abstract class DomainEvent : ValueObject, IDomainEvent
    {
        public string AggregateId { get; }

        public DomainEvent(string aggregateId)
        {
            AggregateId = aggregateId;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AggregateId;
        }
    }
}