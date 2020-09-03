﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Commanding;
using Ports;

namespace InMemoryAdapter
{
    public sealed class StoreCache : IStore
    {
        private readonly AggregateCache _aggregateCache;

        private readonly IStore _store;

        private StoreCache(
            IReadOnlyList<AggregateTypeCacheExpiration> aggregateTypeCacheExpirations, 
            IStore store)
        {
            _aggregateCache = new AggregateCache(aggregateTypeCacheExpirations);
            _store = store;
        }
        
        public static IStore NewUsing(
            IReadOnlyList<AggregateTypeCacheExpiration> aggregateTypeCacheExpirations,
            IStore store) => new StoreCache(aggregateTypeCacheExpirations, store);

        public Task<Result<T>> Get<T>(IAggregateId aggregateId) where T : AggregateRoot, new() =>
            _aggregateCache.TryFetch(aggregateId)
                .Unwrap(
                    cachedAggregate => cachedAggregate.TryTakeValue<T>()
                        .Unwrap(
                            aggregate => Task.FromResult(Result.Ok(aggregate)),
                            () => _store.Get<T>(aggregateId)),
                    () => _store.Get<T>(aggregateId));

        public Task<Result> SaveChanges<T>(T aggregateRoot) where T : AggregateRoot =>
            _store.SaveChanges(aggregateRoot)
                .OnSuccess(() => _aggregateCache.UpdateCacheWith(aggregateRoot));
    }
}