using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Framework.Commanding
{
    public abstract class AggregateId<T> : ValueObject, IAggregateId
    {
        public string Id { get; }
        public string TypeName => typeof(T).Name;
        
        protected AggregateId(string id)
        {
            if (!Regex.IsMatch(id, @"^\S*$")) throw new InvalidAggregateIdException($"Aggregate '{id}' can't contain any whitespace character.");
            if (id.Contains("|")) throw new InvalidAggregateIdException($"Aggregate '{id}' can't contain | characters.");
            Id = id;
        }

        protected sealed override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
        
        public override string ToString() => Id;
        
        public static implicit operator string(AggregateId<T> aggregateId) => aggregateId.ToString();
    }
}