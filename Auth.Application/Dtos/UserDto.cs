using Auth.Domain.Entities;

namespace Auth.Application.Dtos;

public class UserDto
{
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public ERoles? Role { get; set; }
}