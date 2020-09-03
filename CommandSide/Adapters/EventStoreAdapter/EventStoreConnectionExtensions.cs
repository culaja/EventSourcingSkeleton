using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventStoreAdapter
{
    internal static class EventStoreConnectionExtensions
    {
        public static async Task<IReadOnlyList<ResolvedEvent>> ReadAllStreamEventsForward(
            this IEventStoreConnection eventStoreConnection,
            string streamName)
        {
            List<ResolvedEvent> resolvedEvents = new List<ResolvedEvent>();
            StreamEventsSlice streamEventsSlice;
            long i = 0;
            do
            {
                streamEventsSlice = await eventStoreConnection.ReadStreamEventsForwardAsync(streamName, i, 4096, false);
                resolvedEvents.AddRange(streamEventsSlice.Events);
                i += 4096;
            } while (!streamEventsSlice.IsEndOfStream);

            return resolvedEvents;
        }
    }
}