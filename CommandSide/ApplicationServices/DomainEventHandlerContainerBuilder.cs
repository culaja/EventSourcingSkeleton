using System;
using Framework;
using Ports;

namespace ApplicationServices
{
    public sealed class DomainEventHandlerContainerBuilder
    {
        private Optional<IStore> _optionalStore;
        
        public static DomainEventHandlerContainerBuilder New() => new DomainEventHandlerContainerBuilder();

        public DomainEventHandlerContainerBuilder Use(IStore store)
        {
            _optionalStore = Optional<IStore>.From(store);
            return this;
        }

        public DomainEventHandlerContainer Build()
        {
            if (_optionalStore.HasNoValue) throw new ArgumentException("{CommandHandlerContainerBuilder} value not set.", nameof(_optionalStore));

            var store = _optionalStore.Value;
            return new DomainEventHandlerContainer();
        }
    }
}