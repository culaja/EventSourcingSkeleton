namespace Framework
{
    public class InvalidAggregateIdException : BadRequestException
    {
        public InvalidAggregateIdException(string message) : base(message)
        {
        }
    }
}