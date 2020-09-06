using System.Threading.Tasks;
using ApplicationServices;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework;
using Framework.Querying;
using RedisProjections;
using static System.Guid;

namespace Tests.IntegrationTests.RedisProjections
{
    public abstract class ProjectionTests
    {
        private readonly DomainEventProjectionsContainer _domainEventProjectionsContainer =
            DomainEventProjectionsContainerBuilder.New()
                .Use(RedisProjectionsBuilder.NewWith("localhost")
                    .WithCreatedUsersViewProjectionHashName(NewGuid().ToString())
                    .BuildCreatedUsersViewProjection())
                .Build();

        protected IProjection<CreatedUsersView> CreatedUsersViewProjection => _domainEventProjectionsContainer;
        protected Task Apply(IDomainEvent domainEvent) => _domainEventProjectionsContainer.Apply(domainEvent);

        protected ProjectionTests()
        {
            
        }
    }
}