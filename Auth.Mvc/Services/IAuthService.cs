using Auth.Mvc.Models;

namespace Auth.Mvc.Services;

public interface IAuthService
{
    Task<bool> Register(RegisterVM model);
    Task<string> Login(LoginVM model);
    Task<bool> Logout();
    Task<bool> SetRole(SetRoleVM model);
}
