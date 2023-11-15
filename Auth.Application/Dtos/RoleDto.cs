using Auth.Domain.Entities;

namespace Auth.Application.Dtos;

public class RoleDto
{
    public required int UserId { get; set; }
    public required ERoles Role { get; set; }
}