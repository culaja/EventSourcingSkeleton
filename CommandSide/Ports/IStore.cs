using System.Threading.Tasks;
using Framework;
using Framework.Commanding;

namespace Ports
{
    public interface IStore
    {
        /// <summary>
        /// Retrieves specific aggregate instance from the store.
        /// </summary>
        /// <param name="aggregateId">Aggregate ID specifying which aggregate should be retrieved.</param>
        /// <returns>Returns success result with aggregate instance if aggregate has been found in the store; otherwise, if aggregate doesn't exists, returns failure.</returns>
        Task<Result<T>> Get<T>(IAggregateId aggregateId) where T : AggregateRoot, new();
        
        /// <summary>
        /// Save all uncommitted changes in specified aggregate to the store.
        /// </summary>
        /// <param name="aggregateRoot">Aggregate root instance which changes should be stored.</param>
        /// <returns>Returns success result if aggregate changes are successfully saved; if there is version mismatch, returns failure.</returns>
        Task<Result> SaveChanges<T>(T aggregateRoot) where T : AggregateRoot;
    }
}