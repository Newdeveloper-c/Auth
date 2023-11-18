namespace Auth.Mvc.Models;

public class RegisterVM
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
