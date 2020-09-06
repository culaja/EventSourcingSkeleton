using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework;
using StackExchange.Redis;

namespace RedisProjections
{
    public sealed class RedisProjectionsBuilder
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private string _createdUsersViewProjectionHashName = nameof(CreatedUsersView);

        private RedisProjectionsBuilder(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        
        public static RedisProjectionsBuilder NewWith(string databaseConnectionString)
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(databaseConnectionString);
            return new RedisProjectionsBuilder(connectionMultiplexer);
        }

        public RedisProjectionsBuilder WithCreatedUsersViewProjectionHashName(string name)
        {
            _createdUsersViewProjectionHashName = name;
            return this;
        }
        
        public ICreatedUsersViewProjection BuildCreatedUsersViewProjection()
            => new RedisCreatedUsersViewProjection(_connectionMultiplexer, _createdUsersViewProjectionHashName);
    }
}