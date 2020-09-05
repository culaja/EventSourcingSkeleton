using InMemoryAdapter;

namespace Tests.IntegrationTests.Store.Implementations
{
    public sealed class ExtensionInMemoryStoreTests : ExtensionStoreTests
    {
        public ExtensionInMemoryStoreTests() : base(new InMemoryStore())
        {
        }
    }
}