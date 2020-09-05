using System.Threading.Tasks;
using FluentAssertions;
using Ports;
using Xunit;
using static Domain.Errors.General;
using static Tests.IntegrationTests.Values;

namespace Tests.IntegrationTests.Store
{
    public abstract class BasicStoreTests
    {
        private readonly IStore _store;
        
        private readonly TestAggregateId _testAggregateId = GenerateTestAggregateId();

        protected BasicStoreTests(IStore store)
        {
            _store = store;
        }
        
        [Fact]
        public async Task when_aggregate_doesnt_exist_in_store_it_can_be_saved_to_it()
        {
            var result = await _store.SaveChanges(TestAggregate.NewFrom(_testAggregateId));
            result.Should().BeSuccess();
        }

        [Fact]
        public async Task when_aggregate_is_already_in_store_another_instance_cannot_be_stored()
        {
            await _store.SaveChanges(TestAggregate.NewFrom(_testAggregateId));
            var result = await _store.SaveChanges(TestAggregate.NewFrom(_testAggregateId));
            result.Should().BeFailureWith(AggregateVersionMismatchError);
        }

        [Fact]
        public async Task stored_instance_can_be_correctly_retrieved()
        {
            var aggregateRoot = TestAggregate.NewFrom(_testAggregateId);
            aggregateRoot.DoTransformationWith("NewValue");
            
            await _store.SaveChanges(aggregateRoot);

            var result = await _store.Get<TestAggregate>(_testAggregateId);
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be("NewValue");
        }

        [Fact]
        public async Task cant_retrieve_not_existing_aggregate()
        {
            var result = await _store.Get<TestAggregate>(_testAggregateId);
            result.Error.Code.Should().Be(AggregateNotFoundInStoreError);
        }

        [Fact]
        public async Task save_changes_for_aggregate_will_clear_uncommitted_changes()
        {
            var aggregateRoot = TestAggregate.NewFrom(_testAggregateId);
            await _store.SaveChanges(aggregateRoot);

            aggregateRoot.UncommittedDomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task saving_changes_for_changed_aggregate_will_store_changes_to_store()
        {
            var aggregateRoot = TestAggregate.NewFrom(_testAggregateId);
            await _store.SaveChanges(aggregateRoot);

            aggregateRoot.DoTransformationWith("NewValue");

            var result = await _store.SaveChanges(aggregateRoot);

            result.IsSuccess.Should().BeTrue();
        }
    }
}