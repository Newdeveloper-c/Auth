namespace Auth.Application.Exceptions;

[Serializable]
public class InvalidAuthorizationException : Exception
{
    public InvalidAuthorizationException(string? message) : base(message)
    {
    }

    public InvalidAuthorizationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
