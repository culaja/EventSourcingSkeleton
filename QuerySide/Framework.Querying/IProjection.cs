using System.Threading.Tasks;

namespace Framework.Querying
{
    public interface IProjection<T>
    {
        Task<Optional<T>> Fetch(string id);
    }
}