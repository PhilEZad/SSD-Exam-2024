namespace Application.DTOs.Request;

public class StickyNoteRequest
{
    public required string Title { get; set; }
    public required string Content { get; set; }
}