namespace PaintyTask.Domain.Exceptions;

public class InvalidJwtTokenException : Exception
{
    public InvalidJwtTokenException(string message) : base(message)
    {
    }
}