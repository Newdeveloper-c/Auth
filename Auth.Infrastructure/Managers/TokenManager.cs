using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Application.Options;
using Auth.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.Infrastructure.Managers;

public class TokenManager : ITokenManager
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public TokenManager(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

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

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _jwtOptions.Value.Key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            audience: _jwtOptions.Value.Audience,
            issuer: _jwtOptions.Value.Issuer,
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
