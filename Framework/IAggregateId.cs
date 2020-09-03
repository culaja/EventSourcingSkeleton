namespace Framework
{
    public interface IAggregateId : IAmId
    {
        string Name { get; }
    }
}