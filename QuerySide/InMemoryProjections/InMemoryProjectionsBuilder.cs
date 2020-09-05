using ApplicationServices.ProjectionDefinitions.ForUser;

namespace InMemoryProjections
{
    public static class InMemoryProjectionsBuilder
    {
        public static ICreatedUsersViewProjection BuildCreatedUsersViewProjection()
            => new InMemoryCreatedUsersViewProjection();
    }
}