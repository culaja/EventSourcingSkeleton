using System.Threading.Tasks;

namespace Framework.Commanding
{
    public interface ICommandHandler
    {
        Task<Result> Execute(ICommand c);
    }
}