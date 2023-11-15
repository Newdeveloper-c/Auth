using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.Infrastructure.Managers;

public class TokenManager : ITokenManager
{
    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
        return refreshToken;
    }

    public string GenerateJwtToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new ("Id", user.Id.ToString()),
            new ("Email", user.Email)
        };

        claims.Add(new("Role", user.Role.ToString()));
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1));
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
