using System;
using ApplicationServices.ProjectionDefinitions.ForUser;
using Framework;

namespace ApplicationServices
{
    public sealed class DomainEventProjectionsContainerBuilder
    {
        private Optional<ICreatedUsersViewProjection> _optionalCreatedUsersViewProjection;
        
        public static DomainEventProjectionsContainerBuilder New() => new DomainEventProjectionsContainerBuilder();

        public DomainEventProjectionsContainerBuilder Use(ICreatedUsersViewProjection createdUsersViewProjection)
        {
            _optionalCreatedUsersViewProjection = Optional<ICreatedUsersViewProjection>.From(createdUsersViewProjection);
            return this;
        }

        public DomainEventProjectionsContainer Build()
        {
            if (_optionalCreatedUsersViewProjection.HasNoValue) throw new ArgumentException($"{nameof(DomainEventProjectionsContainerBuilder)} value not set.", nameof(_optionalCreatedUsersViewProjection));

            var createdUsersViewProjection = _optionalCreatedUsersViewProjection.Value;
            return new DomainEventProjectionsContainer(
                createdUsersViewProjection);
        }
    }
}