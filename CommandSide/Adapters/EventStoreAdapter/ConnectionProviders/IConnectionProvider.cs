using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventStoreAdapter.ConnectionProviders
{
    internal interface IConnectionProvider
    {
        Task<IEventStoreConnection> GrabConnection();
    }
}