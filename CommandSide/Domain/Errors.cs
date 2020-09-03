using Framework;

namespace Domain
{
    public static class Errors
    {
        public static class General
        {
            public static string AggregateNotFoundInStoreError => $"{nameof(General)}.{nameof(AggregateNotFoundInStoreError)}";
            
            public static Error AggregateNotFoundInStore(string aggregateId) =>
                new Error(AggregateNotFoundInStoreError, $"'{aggregateId}' is not in the store.");

            public static string AggregateAlreadyInStoreError => $"{nameof(General)}.{nameof(AggregateAlreadyInStoreError)}";
            
            public static Error AggregateAlreadyInStore(string aggregateId) =>
                new Error(AggregateAlreadyInStoreError, $"'{aggregateId}' is already in the store.");
            
            public static string AggregateVersionMismatchError => $"{nameof(General)}.{nameof(AggregateVersionMismatchError)}";
            
            public static Error AggregateVersionMismatch(string aggregateId, long expectedVersion) =>
                new Error(AggregateVersionMismatchError, $"'{aggregateId}' version mismatch. Expected version to be {expectedVersion} but there is more events in the stream.");
        }
    }
}