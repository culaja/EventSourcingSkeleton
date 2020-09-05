using System.Threading.Tasks;
using ApplicationServices;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework;
using Framework.Querying;
using InMemoryProjections;

namespace Tests.IntegrationTests.InMemoryProjections
{
    public abstract class ProjectionTests
    {
        private readonly DomainEventProjectionsContainer _domainEventProjectionsContainer =
            DomainEventProjectionsContainerBuilder.New()
                .Use(InMemoryProjectionsBuilder.BuildCreatedUsersViewProjection())
                .Build();

        protected IProjection<CreatedUsersView> CreatedUsersViewProjection => _domainEventProjectionsContainer;
        protected Task Apply(IDomainEvent domainEvent) => _domainEventProjectionsContainer.Apply(domainEvent);

        protected ProjectionTests()
        {
            
        }
    }
}