using System.Threading.Tasks;
using FluentAssertions;
using Framework;
using Ports;
using Xunit;
using static Domain.Errors.General;
using static Tests.IntegrationTests.Values;

namespace Tests.IntegrationTests.Store
{
    public abstract class ExtensionStoreTests
    {
        private readonly IStore _store;
        
        private readonly TestAggregateId _testAggregateId = GenerateTestAggregateId();

        protected ExtensionStoreTests(IStore store)
        {
            _store = store;
        }

        [Fact]
        public async Task when_aggregate_doesnt_exist_in_store_it_can_be_inserted()
        {
            var result = await _store.InsertNew(TestAggregate.NewFrom(_testAggregateId));
            
            result.Should().BeSuccess();
        }

        [Fact]
        public async Task when_aggregate_already_exists_in_store_it_cant_be_inserted()
        {
            await _store.InsertNew(TestAggregate.NewFrom(_testAggregateId));
            
            var result = await _store.InsertNew(TestAggregate.NewFrom(_testAggregateId));
            
            result.Should().BeFailureWith(AggregateAlreadyInStore(_testAggregateId));
        }

        [Fact]
        public async Task when_aggregate_doesnt_exist_in_store_it_cant_be_borrowed()
        {
            var result = await _store.Borrow<TestAggregate>(_testAggregateId, Result.Ok);
            
            result.Should().BeFailureWith(AggregateNotFoundInStore(_testAggregateId));
        }

        [Fact]
        public async Task when_aggregate_exists_in_store_it_can_be_borrowed()
        {
            await _store.InsertNew(TestAggregate.NewFrom(_testAggregateId));
            
            var result = await _store.Borrow<TestAggregate>(_testAggregateId, Result.Ok);
            
            result.Should().BeSuccess();
        }

        [Fact]
        public async Task when_aggregate_exists_in_store_borrow_persists_transformation()
        {
            await _store.InsertNew(TestAggregate.NewFrom(_testAggregateId));
            await _store.Borrow<TestAggregate>(_testAggregateId, ta => ta.DoTransformationWith("SomeString"));

            string fetchedValue = default;
            await _store.Borrow<TestAggregate>(_testAggregateId, ta =>
            {
                fetchedValue = ta.Value;
                return Result.Ok(ta);
            });
            
            fetchedValue.Should().Be("SomeString");
        }
    }
}