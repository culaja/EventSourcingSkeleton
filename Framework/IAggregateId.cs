namespace Framework
{
    public interface IAggregateId
    {
        string Id { get; }
        
        string TypeName { get; }
    }
}