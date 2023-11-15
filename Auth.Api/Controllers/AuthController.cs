using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthManager _authManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthController(
        IAuthManager authManager,
        IHttpContextAccessor contextAccessor)
    {
        _authManager = authManager;
        _contextAccessor = contextAccessor;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authManager.RegisterUser(dto);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var currentContext = _contextAccessor.HttpContext;
        var token = await _authManager.LoginUser(dto, currentContext);
        return Ok(token);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var currentContext = _contextAccessor.HttpContext;
        await _authManager.LogoutUser(currentContext);
        return Ok("Successfully logged out.");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RefreshToken() 
    {
        var currentContext = _contextAccessor.HttpContext;
        var token = await _authManager.UpdateRefreshToken(currentContext);
        return Ok(token);
    }

    [HttpPost]
    [Authorize(Policy = "AdminsOnly")]
    [Authorize(Policy = "Ceo")]
    public async Task<IActionResult> SetRole(RoleDto dto) 
    {
        var result = _authManager.SetRole(dto);
        return Ok(result);
    }
}
