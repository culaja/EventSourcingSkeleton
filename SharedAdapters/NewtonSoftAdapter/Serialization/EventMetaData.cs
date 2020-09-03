using System;
using Newtonsoft.Json;

namespace NewtonSoftAdapter.Serialization
{
    internal sealed class EventMetaData
    {
        public string AggregateType { get; }
        public string FullEventType { get; }

        public EventMetaData(
            string aggregateType,
            string fullEventType)
        {
            AggregateType = aggregateType;
            FullEventType = fullEventType;
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
    }
}