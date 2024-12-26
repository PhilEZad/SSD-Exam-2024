namespace Domain;

public class Note
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required DateTime Created { get; set; }
    public required DateTime Modified { get; set; }
}