namespace Application.DTOs.Create;

public class RegisterDto
{
    public required string Username { get; set; }
    public required string PlainPassword { get; set; }
}