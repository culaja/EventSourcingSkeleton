using System;
using System.Threading.Tasks;
using Framework;
using Framework.Commanding;
using static Domain.Errors.General;

namespace Ports
{
    public static class StoreExtensions
    {
        /// <summary>
        /// Inserts an aggregate to the store.
        /// </summary>
        /// <param name="store"><see cref="IStore"/> instance which is used to insert aggregate.</param>
        /// <param name="newAggregate">New aggregate to add to the store.</param>
        /// <returns>Returns success if aggregate has been inserted to the store; otherwise, if aggregate already exists, returns failure.</returns>
        public static Task<Result> InsertNew<T>(this IStore store, T newAggregate) where T : AggregateRoot, new() => 
            store.SaveChanges(newAggregate)
                .TransformIfError(
                    AggregateVersionMismatchError, 
                    AggregateAlreadyInStore(newAggregate.Id.Id));

        /// <summary>
        /// Retrieves existing aggregate from the store and executes the <see cref="aggregateTransformer"/> in
        /// order to pass some command to the aggregate. As a result of <see cref="aggregateTransformer"/> execution
        /// all changes of the aggregate will be persisted in the database.
        /// </summary>
        /// <param name="store"><see cref="IStore"/> instance which is used to transform the aggregate.</param>
        /// <param name="aggregateId">Unique ID of an aggregate.</param>
        /// <param name="aggregateTransformer">Generic transformer function that will be called if aggregate exists.</param>
        /// <returns>Returns success if aggregate has been found and transformer executed without errors.</returns>
        public static Task<Result> Borrow<T>(
            this IStore store,
            IAggregateId aggregateId, 
            Func<T, Result<T>> aggregateTransformer)
            where T : AggregateRoot, new()
        {
            return store.Borrow<T>(aggregateId, aggregate => Task.FromResult(aggregateTransformer(aggregate)));
        }

        /// <summary>
        /// Borrow method with async aggregate transformer.
        /// </summary>
        public static Task<Result> Borrow<T>(
            this IStore store,
            IAggregateId aggregateId,
            Func<T, Task<Result<T>>> aggregateTransformer)
            where T : AggregateRoot, new() =>
            store.Get<T>(aggregateId)
                .OnSuccess(aggregateTransformer)
                .OnSuccess(aggregate => store.SaveChanges(aggregate))
                .OnFailure(
                    AggregateVersionMismatchError,
                    () => store.Borrow(aggregateId, aggregateTransformer));
    }
}