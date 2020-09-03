using Framework;
using Newtonsoft.Json;
using NewtonSoftAdapter.Serialization;
using static NewtonSoftAdapter.Serialization.EventMetaData;

namespace NewtonSoftAdapter
{
    public sealed class NewtonSoftDomainEventResolver : IDomainEventResolver
    {
        public IDomainEvent ResolveFrom(string rawDomainEvent)
        {
            var domainEventDto = DomainEventDto.From(rawDomainEvent);
            var eventMetaData = EventMetaDataFrom(domainEventDto.MetaData);
            return domainEventDto.Data.ToDomainEventUsing(eventMetaData);
        }

        public static NewtonSoftDomainEventResolver New() => new NewtonSoftDomainEventResolver();
    }
    
    internal sealed class DomainEventDto
    {
        public DomainEventDto(
            long number,
            string topicName,
            long topicVersion,
            string data,
            string metaData)
        {
            Number = number;
            TopicName = topicName;
            TopicVersion = topicVersion;
            Data = data;
            MetaData = metaData;
        }

        public long Number { get; }
        public string TopicName { get; }
        public long TopicVersion { get; }
        public string Data { get; }
        public string MetaData { get; }

        public static DomainEventDto From(string rawDomainEvent) =>
            JsonConvert.DeserializeObject<DomainEventDto>(rawDomainEvent);
    }
}