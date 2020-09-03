using System.Collections.Generic;

namespace Framework
{
    public class Error : ValueObject
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(Message)}: {Message}";
        }
        
        public static implicit operator Error(string code) => new Error(code, "");
    }
}