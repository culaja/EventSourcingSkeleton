using System;
using Framework;
using Newtonsoft.Json;

namespace NewtonSoftAdapter.Serialization
{
    internal static class EventDeserializationExtensions
    {
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