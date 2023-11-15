using Auth.Application.Managers.Interfaces;

namespace Auth.Application.Managers;

public class PasswordManager : IPasswordManager
{
    public Task HashPassword(string password, out byte[] hashedPassword, out byte[] hashedSalt)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyHashedPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        throw new NotImplementedException();
    }
}
