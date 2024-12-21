namespace Domain;

public class User
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string PlainPassword { get; set; }
}