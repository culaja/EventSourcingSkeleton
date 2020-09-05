using Framework.Querying;
using Shared;

namespace ApplicationServices.ProjectionDefinitions.ForUser
{
    public interface ICreatedUsersViewProjection : IProjection<CreatedUsersView>,
        ICanApply<UserEvents.UserCreated>
    {
    }
}