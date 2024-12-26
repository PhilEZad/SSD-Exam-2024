namespace Domain;

public class User
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string HashedPassword { get; set; }
    public required string Created { get; set; }
    public required string Modified { get; set; }
}