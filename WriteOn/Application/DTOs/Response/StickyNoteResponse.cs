namespace Application.DTOs.Response;

public class StickyNoteResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}