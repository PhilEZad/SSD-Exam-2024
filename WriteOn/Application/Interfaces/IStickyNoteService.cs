using Application.DTOs.Create;
using Application.DTOs.Response;
using Application.DTOs.Update;

namespace Application.Interfaces;

public interface IStickyNoteService
{
    public StickyNoteResponse Create(StickyNoteCreate create);
    public StickyNoteResponse ReadById(string id);
    public StickyNoteResponse Update(StickyNoteUpdate update);
    public StickyNoteResponse Delete(string id);
}