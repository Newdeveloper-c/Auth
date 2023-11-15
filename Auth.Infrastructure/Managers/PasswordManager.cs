using Auth.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.Managers;

public class PasswordManager : IPasswordManager
{
    public Task HashPassword(string password, out byte[] passwordHash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Task.CompletedTask;
    }

    public Task<bool> VerifyHashedPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var passHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Task.FromResult(passHash.SequenceEqual(passwordHash));
    }
}
