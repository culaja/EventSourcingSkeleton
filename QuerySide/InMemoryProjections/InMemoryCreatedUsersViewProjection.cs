using System.Threading.Tasks;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Domain.ForUser;
using Framework;
using Shared;

namespace InMemoryProjections
{
    internal sealed class InMemoryCreatedUsersViewProjection : ICreatedUsersViewProjection
    {
        /// Since this view is common for all <see cref="User"/> aggregates it needs to be protected with lock
        private object _sync = new object();
        private CreatedUsersView _createdUsersView = CreatedUsersView.New();
        
        public Task Apply(UserEvents.UserCreated e)
        {
            lock (_sync)
            {
                _createdUsersView = _createdUsersView.AddUser(e.AggregateId);
                return Task.CompletedTask;    
            }
            
        }

        public Task<Optional<CreatedUsersView>> Fetch(string id)
        {
            lock (_sync)
            {
                return Task.FromResult(Optional<CreatedUsersView>.From(_createdUsersView));    
            }
        }
    }
}