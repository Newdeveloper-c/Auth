using Auth.Mvc.Models;

namespace Auth.Mvc.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Register(RegisterVM model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);

            var content = await response.Content.ReadAsStringAsync();
            
            return bool.Parse(content);
        }
        catch (Exception ex)
        {
            return false;
        }
        
    }
    public async Task<string> Login(LoginVM model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public Task<bool> Logout()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetRole(SetRoleVM model)
    {
        throw new NotImplementedException();
    }
}
