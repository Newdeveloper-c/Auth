using Auth.Application.Dtos;
using Auth.Application.Exceptions;
using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authManager.RegisterUser(dto);
        return Ok(result);
    }

    [HttpPost("send-verification")]
    public Task<IActionResult> SendVerification([FromQuery] int code)
    {
        throw new NotImplementedException();
    }

    [HttpGet("get-verification")]
    public Task<IActionResult> GetVerification()
    {
        throw new NotImplementedException();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var currentContext = _contextAccessor.HttpContext
            ?? throw new ArgumentNullException("No context was found.");

        var token = await _authManager.LoginUser(dto, currentContext);
        currentContext.Response.Cookies.Append("Authorization", "Bearer " + token);

        return Ok(token);
            
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var currentContext = _contextAccessor.HttpContext
            ?? throw new InvalidAuthorizationException("No context was found.");
        var result = await _authManager.LogoutUser(currentContext);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken() 
    {
        var currentContext = _contextAccessor.HttpContext
            ?? throw new InvalidAuthorizationException("No context was found.");
        var token = await _authManager.UpdateRefreshToken(currentContext);
        Response.Headers.Add("Authorization", "Bearer " + token);
        return Ok(true);
    }

    [HttpPost("set-role")]
    [Authorize(Roles = "Admin, Ceo")]
    //[Authorize(Roles = "Ceo")]
    public async Task<IActionResult> SetRole(RoleDto dto) 
    {
        var resultToken = _authManager.SetRole(dto);
        Response.Headers.Add("Authorization", "Bearer " + resultToken);
        return Ok(true);
    }
}