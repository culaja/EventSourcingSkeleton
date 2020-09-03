using System;
using ApplicationServices.ForUser;
using Framework;
using Ports;

namespace ApplicationServices
{
    public sealed class CommandHandlerContainerBuilder
    {
        private Optional<IStore> _optionalRepository;
        
        public static CommandHandlerContainerBuilder New() => new CommandHandlerContainerBuilder();

        public CommandHandlerContainerBuilder Use(IStore store)
        {
            _optionalRepository = Optional<IStore>.From(store);
            return this;
        }

        public CommandHandlerContainer Build()
        {
            if (_optionalRepository.HasNoValue) throw new ArgumentException("{CommandHandlerContainerBuilder} value not set.", nameof(_optionalRepository));

            var store = _optionalRepository.Value;
            return new CommandHandlerContainer(
                new CreateUserHandler(store));
        }
    }
}