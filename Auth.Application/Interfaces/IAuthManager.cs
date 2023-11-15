using Auth.Api.Controllers;
using Auth.Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace Auth.Application.Interfaces;

public interface IAuthManager
{
    Task<UserDto> RegisterUser(RegisterDto dto);
    Task<string?> LoginUser(LoginDto dto, HttpContext httpContext);
    Task<bool> LogoutUser(HttpContext httpContext);
    Task<string?> GenerateRefreshToken(HttpContext httpContext);
    Task<UserDto> SetUserRole(RoleDto dto);
}
