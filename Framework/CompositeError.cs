using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace Framework
{
    public sealed class CompositeError : Error
    {
        public IReadOnlyList<Error> Errors { get; }

        public CompositeError(IReadOnlyList<Error> errors) : base(CombineCodeFrom(errors), CombineMessageFrom(errors))
        {
            Errors = errors;
        }

        private static string CombineCodeFrom(IReadOnlyList<Error> errors) =>
            string.Join(";", errors.Select(e => e.Code));

        private static string CombineMessageFrom(IReadOnlyList<Error> errors) =>
            string.Join($"{NewLine}", errors.Select(e => e.Message));
    }
}