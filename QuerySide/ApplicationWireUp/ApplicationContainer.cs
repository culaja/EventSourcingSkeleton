using ApplicationServices;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework.Querying;
using NewtonSoftAdapter;
using RedisProjections;

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
        
        public static ApplicationContainer Build(string redisConnectionString)
        {
            var redisProjectionsBuilder = RedisProjectionsBuilder.NewWith(redisConnectionString);
            var domainEventProjectionsContainer = DomainEventProjectionsContainerBuilder.New()
                .Use(redisProjectionsBuilder.BuildCreatedUsersViewProjection())
                .Build();
            
            var domainEventResolver = NewtonSoftDomainEventResolver.New();
            
            return new ApplicationContainer(
                domainEventProjectionsContainer,
                domainEventResolver);
        }
    }
}