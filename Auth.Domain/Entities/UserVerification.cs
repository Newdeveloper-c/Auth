namespace Auth.Domain.Entities;

public class UserVerification
{
    public int Id { get; set; }
    public int Code { get; set; }
    public int UserId { get; set; }
}
