namespace Framework
{
    public interface IAggregateId : IAmId
    {
        string Type { get; }
        string Name { get; }
    }
}