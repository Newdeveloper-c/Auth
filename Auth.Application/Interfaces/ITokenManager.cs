using Auth.Application.Dtos;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface ITokenManager
{
    string GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken();
}
