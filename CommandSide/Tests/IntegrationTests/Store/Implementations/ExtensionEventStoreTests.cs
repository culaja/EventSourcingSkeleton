namespace Tests.IntegrationTests.Store.Implementations
{
    public sealed class ExtensionEventStoreTests : ExtensionStoreTests
    {
        public ExtensionEventStoreTests() 
            : base(EventStoreAdapter.Store.NewUsing(
                "tcp://localhost:1113",
                "TestStore"))
        {
        }
    }
}