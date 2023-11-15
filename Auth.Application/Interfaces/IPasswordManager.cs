namespace Auth.Application.Interfaces;

public interface IPasswordManager
{
    Task HashPassword(string password, out byte[] hashedPassword, out byte[] hashedSalt);
    Task<bool> VerifyHashedPassword(string password, byte[] passwordHash, byte[] passwordSalt);
}
