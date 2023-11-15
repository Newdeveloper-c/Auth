namespace Auth.Application.Exceptions;

[Serializable]
public class WrongInputException : Exception
{
    public WrongInputException(string? message) : base(message)
    {
    }

    public WrongInputException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}