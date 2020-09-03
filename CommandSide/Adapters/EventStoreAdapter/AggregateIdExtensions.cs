using Framework;

namespace EventStoreAdapter
{
    internal static class AggregateIdExtensions
    {
        public static string ToStreamName(this IAggregateId aggregateId, string eventStoreName) 
            => $"{eventStoreName}|{aggregateId.Id}";
    }
}