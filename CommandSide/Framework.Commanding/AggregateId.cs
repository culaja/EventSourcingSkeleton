using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Framework.Commanding
{
    public abstract class AggregateId<T> : ValueObject, IAggregateId
    {
        protected AggregateId(string aggregateName)
        {
            var aggregateType = typeof(T).Name;
            if (!Regex.IsMatch(nameof(T), @"^\S*$")) throw new InvalidAggregateIdException($"Aggregate type '{aggregateType}' can't contain any whitespace character.");
            if (aggregateType.Contains("_")) throw new InvalidAggregateIdException($"Aggregate type '{aggregateType}' can't contain _ characters.");

            if (!Regex.IsMatch(aggregateName, @"^\S*$")) throw new InvalidAggregateIdException($"Aggregate '{aggregateName}' can't contain any whitespace character.");
            if (aggregateName.Contains("|")) throw new InvalidAggregateIdException($"Aggregate '{aggregateName}' can't contain | characters.");
            
            Type = aggregateType;
            Name = aggregateName;
        }

        public string Type { get; }
        public string Name { get; }

        public static implicit operator string(AggregateId<T> aggregateId) => aggregateId.ToString();
        
        protected sealed override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Name;
        }
        
        public override string ToString() => Id;

        public string Id => $"{Type}|{Name}";
    }
}