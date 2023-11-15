using System.Runtime.Serialization;

namespace Auth.Application.Exceptions;

[Serializable]
public class WrongInputException : Exception
{
    public WrongInputException()
    {
    }

    public WrongInputException(string? message) : base(message)
    {
    }

    public WrongInputException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected WrongInputException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}