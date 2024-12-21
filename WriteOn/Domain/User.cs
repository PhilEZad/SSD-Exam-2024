namespace Domain;

public class User
{
    public required int Id { get; set; }
    public required string AccountName { get; set; }
    public required string PlainPassword { get; set; }
}