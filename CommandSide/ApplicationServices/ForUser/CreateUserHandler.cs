using System.Threading.Tasks;
using Domain.ForUser;
using Domain.ForUser.Commands;
using Framework;
using Framework.Commanding;
using Ports;

namespace ApplicationServices.ForUser
{
    public class CreateUserHandler : CommandHandler<CreateUser>
    {
        private readonly IStore _store;

        public CreateUserHandler(IStore store)
        {
            _store = store;
        }
        
        public override Task<Result> Execute(CreateUser c) => 
            _store.InsertNew(User.NewWith(UserId.Of(c.UserName)));
    }
}