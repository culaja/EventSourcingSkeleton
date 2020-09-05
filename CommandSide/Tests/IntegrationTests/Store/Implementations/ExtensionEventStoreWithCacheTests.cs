using System;
using System.Collections.Generic;
using Domain.ForUser;
using InMemoryAdapter;

namespace Tests.IntegrationTests.Store.Implementations
{
    public sealed class ExtensionEventStoreWithCacheTests : ExtensionStoreTests
    {
        public ExtensionEventStoreWithCacheTests() 
            : base(
                StoreCache.NewUsing(
                    new List<AggregateTypeCacheExpiration> { AggregateTypeCacheExpiration.Of(nameof(User), TimeSpan.FromSeconds(10)) },
                    EventStoreAdapter.Store.NewUsing("tcp://localhost:1113", "TestStore")))
        {
        }
    }
}