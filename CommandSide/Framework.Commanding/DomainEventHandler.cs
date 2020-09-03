using System.Threading.Tasks;

namespace Framework.Commanding
{
    public abstract class DomainEventHandler<T> : IDomainEventHandler where T : IDomainEvent
    {
        public abstract Task<Result> Handle(T e);

        public Task<Result> Handle(IDomainEvent e) => Handle((T) e);
    }
}