using Auth.Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace Auth.Application.Interfaces;

public interface IAuthManager
{
    Task<UserDto> RegisterUser(RegisterDto dto);
    Task<string> LoginUser(LoginDto dto, HttpContext? httpContext);
    Task LogoutUser(HttpContext? httpContext);
    Task<string> UpdateRefreshToken(HttpContext? httpContext);
    Task<string> SetRole(RoleDto dto);
}
