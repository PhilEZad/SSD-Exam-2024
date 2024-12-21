namespace Application.DTOs.Request;

public class LoginDto
{
    public required string Username { get; set; }
    public required string PlainPassword { get; set; }
}