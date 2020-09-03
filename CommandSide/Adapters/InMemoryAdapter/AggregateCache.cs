﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Framework;
using Framework.Commanding;

namespace InMemoryAdapter
{
    
    internal sealed class AggregateCache
    {
        private readonly IReadOnlyDictionary<string, CacheItemPolicy> _aggregateTypeCacheExpirations;
        private readonly MemoryCache _memoryCache = MemoryCache.Default;

        public AggregateCache(IReadOnlyList<AggregateTypeCacheExpiration> aggregateTypeCacheExpirations)
        {
            _aggregateTypeCacheExpirations = aggregateTypeCacheExpirations.ToDictionary(
                e => e.AggregateType,
                e => new CacheItemPolicy { SlidingExpiration = e.ExpirationTimeSpan});
        }

        public Optional<CachedAggregate> TryFetch(IAggregateId aggregateId) =>
            _memoryCache.GetCacheItem(aggregateId.ToString())
                .ToOptional()
                .Map(cacheItem => (CachedAggregate) cacheItem.Value);

        public void UpdateCacheWith<T>(T aggregateRoot) where T : AggregateRoot
        {
            var optionalCacheItemPolicy = TryGetCacheItemPolicyFor(aggregateRoot.Id.Type);
            if (optionalCacheItemPolicy.HasValue)
            {
                _memoryCache.Set(new CacheItem(aggregateRoot.Id.ToString(), new CachedAggregate(aggregateRoot)), optionalCacheItemPolicy.Value );
            }
        }

        private Optional<CacheItemPolicy> TryGetCacheItemPolicyFor(string aggregateType) =>
            _aggregateTypeCacheExpirations.TryGetValue(aggregateType, out var cacheItemPolicy)
                ? cacheItemPolicy
                : Optional<CacheItemPolicy>.None;
    }
}