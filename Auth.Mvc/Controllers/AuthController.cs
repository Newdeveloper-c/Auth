using Auth.Mvc.Models;
using Auth.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _contextAccessor;
    public AuthController(IAuthService authService, 
        IHttpContextAccessor contextAccessor)
    {
        _authService = authService;
        _contextAccessor = contextAccessor;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if(! ModelState.IsValid)
            return View();

        var regResult = await _authService.Register(vm);

        if (regResult)
            ViewBag.Login = "Successfully Registered";
        else
        {
            ViewBag.Login = "Email address already exists !!! Please try again.";
            return View();
        }
            
        return RedirectToAction("UnauthorizedHomepage", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
            return View();

        var loginResult = await _authService.Login(vm);

        var currentContext = _contextAccessor.HttpContext
            ?? throw new ArgumentNullException("No context was found.");

        currentContext.Response.Headers.Add("Authorization", "Bearer " + loginResult);
        currentContext.Request.Headers.Add("Authorization", "Bearer " + loginResult);

        //currentContext.Response.Cookies.Append("Authorization", "Bearer " + loginResult, new CookieOptions
        //{
        //    HttpOnly = true,
        //    // Other options as needed
        //});

        return RedirectToAction("AuthorizedHomepage", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        return RedirectToAction("AuthorizedHomepage", "Home");
    }

    [HttpGet]
    public IActionResult SetRole()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SetRole(SetRoleVM vm)
    {

        return View();
    }
}
