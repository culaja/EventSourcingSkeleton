using Framework;

namespace Shared
{
    public static class UserEvents
    {
        public sealed class UserCreated : DomainEvent
        {
            public UserCreated(string aggregateId) : base(aggregateId)
            {
            }
        }
    }
}