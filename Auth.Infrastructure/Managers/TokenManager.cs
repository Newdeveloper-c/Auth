using Auth.Application.Dtos;
using Auth.Application.Managers.Interfaces;
using Auth.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.Application.Managers;

public class TokenManager : ITokenManager
{
    private readonly 
    public RefreshToken GenerateRefreshTokenAsync()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(1),
            Created = DateTime.UtcNow
        };
        return refreshToken;
    }

    public Task<string> GenerateTokenAsync(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new ("Id", user.Id.ToString()),
            new ("Email", user.Email)
        };

        if(user.RoleId is not null)
        {
            ERoles role = (ERoles)user.RoleId;
            claims.Add(new("Role", role.ToString()));
        }
        
        
    }
}
