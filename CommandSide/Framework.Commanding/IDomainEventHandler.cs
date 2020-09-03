using System.Threading.Tasks;

namespace Framework.Commanding
{
    public interface IDomainEventHandler
    {
        Task<Result> Handle(IDomainEvent e);
    }
}