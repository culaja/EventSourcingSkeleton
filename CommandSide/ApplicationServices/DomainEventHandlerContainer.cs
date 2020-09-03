using System.Threading.Tasks;
using Framework;
using Framework.Commanding;

namespace ApplicationServices
{
    public sealed class DomainEventHandlerContainer : IDomainEventHandler
    {
        internal DomainEventHandlerContainer()
        {
        }

        public Task<Result> Handle(IDomainEvent e)
        {
            switch (e)
            {
                default: return IgnoreDomainEvent();
            }
        }

        private static Task<Result> IgnoreDomainEvent() => Task.FromResult(Result.Ok());
    }
}