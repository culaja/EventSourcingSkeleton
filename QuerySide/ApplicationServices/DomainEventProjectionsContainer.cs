using System.Threading.Tasks;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework;
using Framework.Querying;
using Shared;

namespace ApplicationServices
{
    public sealed class DomainEventProjectionsContainer : ICanApply, IProjection<CreatedUsersView>
    {
        private readonly ICreatedUsersViewProjection _createdUsersViewProjection;

        internal DomainEventProjectionsContainer(ICreatedUsersViewProjection createdUsersViewProjection)
        {
            _createdUsersViewProjection = createdUsersViewProjection;
        }

        public Task Apply(IDomainEvent e)
        {
            switch (e)
            {
                case UserEvents.UserCreated userCreated:
                    return _createdUsersViewProjection.Apply(userCreated);
                default:
                    return IgnoreDomainEvent();
            }
        }

        private static Task<Result> IgnoreDomainEvent() => Task.FromResult(Result.Ok());
        
        public Task<Optional<CreatedUsersView>> Fetch(string projectId) => _createdUsersViewProjection.Fetch(projectId);
    }
}