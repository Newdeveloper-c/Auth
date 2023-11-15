using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Application.Exceptions;
using Auth.Domain.Context;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Managers;

public class AuthManager : IAuthManager
{
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenManager _tokenManager;
    private readonly AuthDbContext _authDbContext;

    public AuthManager(
        IPasswordManager passwordManager,
        ITokenManager tokenManager, 
        AuthDbContext authDbContext)
    {
        _passwordManager = passwordManager;
        _tokenManager = tokenManager;
        _authDbContext = authDbContext;
    }

    public async Task<UserDto> RegisterUser(RegisterDto dto)
    {
        var userCheck = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if(userCheck is not null)
            throw new WrongInputException("User with this email address already exists.");
        
        await _passwordManager.HashPassword(
            dto.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        var entry = await _authDbContext.Users.AddAsync(user);
        await _authDbContext.SaveChangesAsync();

        return new UserDto
        {
            UserId = entry.Entity.Id,
            Email = entry.Entity.Email,
            Role = entry.Entity.Role
        };
    }

    public async Task<string> LoginUser(LoginDto dto, HttpContext? httpContext)
    {
        var user = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email)
            ?? throw new WrongInputException("Email address or Password is wrong !!! Please try again.");

        if (!await _passwordManager.VerifyHashedPassword(dto.Password,
            user.PasswordHash, user.PasswordSalt))
            throw new WrongInputException("Email address or Password is wrong !!! Please try again.");

        var jwtToken = _tokenManager.GenerateJwtToken(user);
        var refreshToken = _tokenManager.GenerateRefreshToken();

        if (user.Role == ERoles.Unauthorized)
            user.Role = ERoles.Authorized;
        
        await SetRefreshToken(refreshToken, httpContext, user);

        return jwtToken;
    }

    public async Task LogoutUser(HttpContext? httpContext)
    {
        if (!int.TryParse(httpContext.User.FindFirst("Id")?.Value, out var id))
            throw new InvalidAuthorizationException("User Id not found.");

        var user = await _authDbContext.Users.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new UserNotFoundException("User not found.");

        user.RefreshToken = null;
        user.CreationTime = null;
        user.ExpireTime = null;
        await _authDbContext.SaveChangesAsync();


    }

    public async Task<string?> UpdateRefreshToken(HttpContext httpContext)
    {
        var refreshToken = httpContext.Request.Cookies["refreshToken"];
        if (refreshToken == null)
            throw new InvalidAuthorizationException("Cookies not found.");

        if (!int.TryParse(httpContext.User.FindFirst("Id")?.Value, out var id))
            throw new InvalidAuthorizationException("User Id not found.");

        var user = await _authDbContext.Users.SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new UserNotFoundException("User not found.");

        if (!user.RefreshToken.Equals(refreshToken))
            throw new InvalidAuthorizationException("Refresh tokens did not match.");

        if (user.ExpireTime < DateTime.UtcNow)
            throw new WrongInputException("Refresh token did not expired yet.");

        var newJwt = _tokenManager.GenerateJwtToken(user);
        var newRefreshToken = _tokenManager.GenerateRefreshToken();
        await SetRefreshToken(newRefreshToken, httpContext, user);

        return newJwt;
    }

    private async Task SetRefreshToken(
        RefreshToken refreshToken,
        HttpContext? httpContext, 
        User user)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expires,
        };

        httpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        user.RefreshToken = refreshToken.Token;
        user.CreationTime = refreshToken.Created;
        user.ExpireTime = refreshToken.Expires;

        await _authDbContext.SaveChangesAsync();
    }

    public async Task<string> SetRole(RoleDto dto)
    {
        var user = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId)
            ?? throw new UserNotFoundException("User not found.");
        
        user.Role = dto.Role;
        await _authDbContext.SaveChangesAsync();

        var newJwt = _tokenManager.GenerateJwtToken(user);
        return newJwt;
    }
}
