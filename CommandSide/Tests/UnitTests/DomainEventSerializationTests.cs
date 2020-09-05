using System.Linq;
using System.Reflection;
using FluentAssertions;
using Framework;
using Xunit;
using static Newtonsoft.Json.JsonConvert;

namespace Tests.UnitTests
{
    public sealed class DomainEventSerializationTests
    {
        [Fact]
        public void all_domain_events_are_correctly_serialized_and_deserialized()
        {
            var allDomainEvents = typeof(Values)
                .GetFields(PublicAndStatic)
                .Where(FieldIsDomainEvent)
                .Select(FieldValue)
                .ToList();

            foreach (var domainEvent in allDomainEvents)
            {
                DeserializeObject(SerializeObject(domainEvent), domainEvent.GetType())
                    .Should().Be(domainEvent);
            }
        }

        private static BindingFlags PublicAndStatic => BindingFlags.Static | BindingFlags.Public;

        private static bool FieldIsDomainEvent(FieldInfo fieldInfo) =>
            typeof(IDomainEvent).IsAssignableFrom(fieldInfo.FieldType);

        private static IDomainEvent FieldValue(FieldInfo fieldInfo) => (IDomainEvent)fieldInfo.GetValue(default);
    }
}