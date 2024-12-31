namespace Application.DTOs.Update;

public class NoteUpdate
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required int OwnerId { get; set; }
}