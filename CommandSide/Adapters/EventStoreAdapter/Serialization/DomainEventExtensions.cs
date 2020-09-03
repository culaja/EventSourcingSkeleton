using System;
using System.Text;
using EventStore.ClientAPI;
using Framework;
using Newtonsoft.Json;
using static EventStoreAdapter.Serialization.EventMetaData;

namespace EventStoreAdapter.Serialization
{
    internal static class DomainEventExtensions
    {
        public static EventData ToEventData(this IDomainEvent e) =>
            new EventData(
                Guid.NewGuid(), 
                e.GetType().Name,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)),
                EventMetaDataFrom(e).ToByteArray());
    }
}