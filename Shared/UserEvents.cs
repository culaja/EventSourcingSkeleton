using Framework;

namespace Shared
{
    public static class UserEvents
    {
        public sealed class UserCreated : DomainEvent
        {
            public string Name { get; }

            public UserCreated(string aggregateId, string name) : base(aggregateId)
            {
                Name = name;
            }
        }
    }
}