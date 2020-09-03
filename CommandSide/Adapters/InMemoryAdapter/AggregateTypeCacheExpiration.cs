using System;
using System.Collections.Generic;
using Framework;

namespace InMemoryAdapter
{
    public sealed class AggregateTypeCacheExpiration : ValueObject
    {
        public string AggregateType { get; }
        public TimeSpan ExpirationTimeSpan { get; }

        private AggregateTypeCacheExpiration(string aggregateType, TimeSpan expirationTimeSpan)
        {
            AggregateType = aggregateType;
            ExpirationTimeSpan = expirationTimeSpan;
        }

        public static AggregateTypeCacheExpiration Of(Optional<string> optionalAggregateType, TimeSpan expirationTimeSpan) =>
            optionalAggregateType.Unwrap(
                aggregateType => new AggregateTypeCacheExpiration(aggregateType, expirationTimeSpan),
                () => throw new ArgumentNullException(nameof(optionalAggregateType)));
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AggregateType;
            yield return ExpirationTimeSpan;
        }
    }
}