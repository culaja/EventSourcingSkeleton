using System.Threading.Tasks;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework;
using Shared;

namespace InMemoryProjections
{
    internal sealed class InMemoryCreatedUsersViewProjection : ICreatedUsersViewProjection
    {
        private CreatedUsersView _createdUsersView = CreatedUsersView.New();
        
        public Task Apply(UserEvents.UserCreated e)
        {
            _createdUsersView = _createdUsersView.AddUser(e.AggregateId);
            return Task.CompletedTask;
        }

        public Task<Optional<CreatedUsersView>> Fetch(string id) => 
            Task.FromResult(Optional<CreatedUsersView>.From(_createdUsersView));
    }
}