using Framework.Commanding;

namespace Domain.ForUser.Commands
{
    public sealed class CreateUser : ICommand
    {
        public string UserName { get; }

        public CreateUser(string userName)
        {
            UserName = userName;
        }
    }
}