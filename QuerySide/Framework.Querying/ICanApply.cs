using System.Threading.Tasks;

namespace Framework.Querying
{
    public interface ICanApply
    {
        Task Apply(IDomainEvent e);
    }

    public interface ICanApply<in T> where T : IDomainEvent
    {
        Task Apply(T e);
    }
}