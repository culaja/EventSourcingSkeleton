using System;
using System.Text;
using EventStore.ClientAPI;
using Framework;
using Newtonsoft.Json;

namespace EventStoreAdapter.Serialization
{
    internal sealed class EventMetaData
    {
        public string FullEventType { get; }

        public EventMetaData(string fullEventType)
        {
            FullEventType = fullEventType;
        }
        
        public static EventMetaData EventMetaDataFrom(IDomainEvent domainEvent) =>
            new EventMetaData(domainEvent.GetType().AssemblyQualifiedName);

        public static EventMetaData EventMetaDataFrom(RecordedEvent recordedEvent)
        {
            var serializedEventMetaDataString = Encoding.UTF8.GetString(recordedEvent.Metadata);
            return EventMetaDataFrom(serializedEventMetaDataString);
        }

        public static EventMetaData EventMetaDataFrom(string serializedEventMetaDataString)
        {
            try
            {
                return JsonConvert.DeserializeObject<EventMetaData>(serializedEventMetaDataString);
            }
            catch (Exception ex)
            {
                throw new EventMetaDataDeserializationException(serializedEventMetaDataString, ex);
            }
        }

        public byte[] ToByteArray() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
    }
}