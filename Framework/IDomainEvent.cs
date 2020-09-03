﻿namespace Framework
{
    public interface IDomainEvent
    {
        string AggregateId { get; }
    }
}