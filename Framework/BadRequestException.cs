using System;

namespace Framework
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message) : base(message)
        {
        }
    }
}