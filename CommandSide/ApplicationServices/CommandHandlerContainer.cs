using System;
using System.Threading.Tasks;
using ApplicationServices.ForUser;
using Domain.ForUser.Commands;
using Framework;
using Framework.Commanding;

namespace ApplicationServices
{
    public sealed class CommandHandlerContainer : ICommandHandler
    {
        private readonly CreateUserHandler _createUserHandler;

        internal CommandHandlerContainer(
            CreateUserHandler createUserHandler)
        {
            _createUserHandler = createUserHandler;
        }

        public Task<Result> Execute(ICommand c)
        {
            switch (c)
            {
                case CreateUser createUser: return Execute(createUser);
                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }

        private Task<Result> Execute(CreateUser c) => _createUserHandler.Execute(c);
    }
}