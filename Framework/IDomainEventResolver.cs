namespace Framework
{
    public interface IDomainEventResolver
    {
        IDomainEvent ResolveFrom(string rawDomainEvent);
    }
}