using System;

namespace EventStoreAdapter.Serialization
{
    internal sealed class EventMetaDataDeserializationException : Exception
    {
        public EventMetaDataDeserializationException(
            string serializedEventMetaData,
            Exception innerException)
            : base($"Failed to deserialize event meta data with content: {serializedEventMetaData}.", innerException)
        {
        }
    }
}