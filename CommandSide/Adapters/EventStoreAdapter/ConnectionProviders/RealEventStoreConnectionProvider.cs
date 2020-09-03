using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Framework;
using static System.Threading.Tasks.Task;
using static Framework.Optional<EventStore.ClientAPI.IEventStoreConnection>;

namespace EventStoreAdapter.ConnectionProviders
{
    internal sealed class RealEventStoreConnectionProvider : IConnectionProvider
    {
        private static readonly object SyncObject = new object();
        private static Optional<IEventStoreConnection> _eventStoreConnectionInstance = None;

        public static Task<IEventStoreConnection> GrabSingleEventStoreConnectionFor(string connectionString)
        {
            if (_eventStoreConnectionInstance.HasNoValue)
            {
                lock (SyncObject)
                {
                    if (_eventStoreConnectionInstance.HasNoValue)
                    {
                        _eventStoreConnectionInstance = From(EventStoreConnection.Create(GetConnectionBuilder(), new Uri(connectionString)));
                        return _eventStoreConnectionInstance.Value.ConnectAsync()
                            .ContinueWith(t => _eventStoreConnectionInstance.Value);
                    }
                }
            }

            return FromResult(_eventStoreConnectionInstance.Value);
        }

        private static ConnectionSettings GetConnectionBuilder()
        {
            var settings = ConnectionSettings.Create()
                .KeepReconnecting();
            return settings;
        }

        private readonly string _connectionString;

        public RealEventStoreConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<IEventStoreConnection> GrabConnection()
        {
            return GrabSingleEventStoreConnectionFor(_connectionString);
        }

        public void Dispose()
        {
        }
    }
}