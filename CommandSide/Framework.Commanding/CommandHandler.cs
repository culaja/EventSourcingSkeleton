using System.Threading.Tasks;

namespace Framework.Commanding
{
    public abstract class CommandHandler<T> : ICommandHandler where T : ICommand
    {
        public Task<Result> Execute(ICommand c) => Execute((T) c);

        public abstract Task<Result> Execute(T c);
    }
}