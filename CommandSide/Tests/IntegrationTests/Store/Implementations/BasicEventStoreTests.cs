namespace Tests.IntegrationTests.Store.Implementations
{
    public sealed class BasicEventStoreTests : BasicStoreTests
    {
        public BasicEventStoreTests() 
            : base(EventStoreAdapter.Store.NewUsing(
                "tcp://localhost:1113",
                "TestStore"))
        {
        }
    }
}