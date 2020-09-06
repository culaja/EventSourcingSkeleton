using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework;
using Shared;
using StackExchange.Redis;

namespace RedisProjections
{
    internal sealed class RedisCreatedUsersViewProjection : ICreatedUsersViewProjection
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly string _createdUsersViewHashKey;

        private IDatabase Database => _connectionMultiplexer.GetDatabase();

        public RedisCreatedUsersViewProjection(
            ConnectionMultiplexer connectionMultiplexer,
            string createdUsersViewHashKey)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _createdUsersViewHashKey = createdUsersViewHashKey;
        }
        
        public async Task<Optional<CreatedUsersView>> Fetch(string id)
        {
            var allHash = await Database.HashGetAllAsync(_createdUsersViewHashKey);
            var allUsers = allHash.Select(h => h.Name.ToString()).ToList();
            return CreatedUsersView.From(allUsers);
        }

        public async Task Apply(UserEvents.UserCreated e)
        {
            var hashEntries = new [] { new HashEntry(e.AggregateId, 0) };
            await Database.HashSetAsync(_createdUsersViewHashKey, hashEntries);
        }
    }
}