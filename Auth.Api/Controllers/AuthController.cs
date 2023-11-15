using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(RegisterDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(LoginDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult Logout()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public IActionResult RefreshToken() 
    {
        throw new NotImplementedException();
    }
}
