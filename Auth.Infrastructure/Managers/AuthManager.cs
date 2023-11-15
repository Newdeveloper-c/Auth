using Auth.Api.Controllers;
using Auth.Application.Dtos;
using Auth.Application.Exceptions;
using Auth.Application.Managers.Interfaces;
using Auth.Application.Services.Interfaces;
using Auth.Domain.Context;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Managers;

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

    public Task<string?> GenerateRefreshToken(HttpContext httpContext)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> LoginUser(LoginDto dto, HttpContext httpContext)
    {
        var user = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email) 
            ?? throw new UserNotFoundException("User not found.");

        if (!await _passwordManager.VerifyHashedPassword(dto.Password,
            user.PasswordHash, user.PasswordSalt))
            throw new WrongInputException("Email address or Password is wrong !!! Please try again.");

        var jwtToken = await _tokenManager.GenerateTokenAsync(user);
        var refreshToken = await _tokenManager.GenerateRefreshTokenAsync();

        await SetRefreshToken(refreshToken, httpContext, user);
        return jwtToken;
    }

    private async Task SetRefreshToken(
        RefreshToken refreshToken,
        HttpContext httpContext, 
        User user)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expires,
        };

        httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        user.RefreshToken = refreshToken.Token;
        user.CreationTime = refreshToken.Created;
        user.ExpireTime = refreshToken.Expires;

        await _authDbContext.SaveChangesAsync();
    }

    public Task<bool> LogoutUser(HttpContext httpContext)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDto> RegisterUser(RegisterDto dto)
    {
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

        var result = await _authDbContext.Users.AddAsync(user);
        await _authDbContext.SaveChangesAsync();

        return new UserDto
        {
            UserId = result.Entity.Id,
            Email = result.Entity.Email,
            RoleId = result.Entity.RoleId
        };
    }

    public Task<UserDto> SetUserRole(RoleDto dto)
    {
        throw new NotImplementedException();
    }
}
