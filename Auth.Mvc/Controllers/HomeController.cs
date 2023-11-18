using Auth.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Auth.Mvc.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult UnauthorizedHomepage()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthorizedHomepage()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}