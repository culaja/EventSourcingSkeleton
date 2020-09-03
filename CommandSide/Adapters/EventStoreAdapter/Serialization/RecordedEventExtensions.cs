using System;
using System.Text;
using EventStore.ClientAPI;
using Framework;
using Newtonsoft.Json;

namespace EventStoreAdapter.Serialization
{
    internal static class EventDeserializationExtensions
    {
        public static IDomainEvent ToDomainEvent(this RecordedEvent recordedEvent)
        {
            var serializedEventString = Encoding.UTF8.GetString(recordedEvent.Data);
            var eventMetaData = EventMetaData.EventMetaDataFrom(recordedEvent);
            return serializedEventString.ToDomainEventUsing(eventMetaData);
        }

        public static IDomainEvent ToDomainEventUsing(
            this string serializedDomainEventData,
            EventMetaData eventMetaData)
        {
            try
            {
                return (IDomainEvent)JsonConvert
                    .DeserializeObject(
                        serializedDomainEventData,
                        Type.GetType(eventMetaData.FullEventType));
            }
            catch (Exception ex)
            {
                throw new EventDeserializationException(serializedDomainEventData, ex);
            }
        }
    }
}