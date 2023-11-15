using Auth.Application.Dtos;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface ITokenManager
{
    Task<string> GenerateTokenAsync(User user);
    RefreshToken GenerateRefreshTokenAsync();
}
