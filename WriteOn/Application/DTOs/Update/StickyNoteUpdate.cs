﻿namespace Application.DTOs.Update;

public class StickyNoteUpdate
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}