using ApplicationServices;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework.Querying;
using InMemoryProjections;
using NewtonSoftAdapter;

namespace ApplicationWireUp
{
    public sealed class ApplicationContainer
    {
        public DomainEventProjectionsContainer DomainEventAppliers { get; }
        public NewtonSoftDomainEventResolver DomainEventResolver { get; }
        public IProjection<CreatedUsersView> CreatedUsersViewProjection => DomainEventAppliers;

        private ApplicationContainer(
            DomainEventProjectionsContainer domainEventAppliers,
            NewtonSoftDomainEventResolver domainEventResolver)
        {
            DomainEventAppliers = domainEventAppliers;
            DomainEventResolver = domainEventResolver;
        }
        
        public static ApplicationContainer Build()
        {
            var domainEventProjectionsContainer = DomainEventProjectionsContainerBuilder.New()
                .Use(InMemoryProjectionsBuilder.BuildCreatedUsersViewProjection())
                .Build();
            
            var domainEventResolver = NewtonSoftDomainEventResolver.New();
            
            return new ApplicationContainer(
                domainEventProjectionsContainer,
                domainEventResolver);
        }
    }
}