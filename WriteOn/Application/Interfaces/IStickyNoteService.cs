using Application.DTOs.Create;
using Application.DTOs.Response;
using Application.DTOs.Update;

namespace Application.Interfaces;

public interface IStickyNoteService
{
    public StickyNoteResponse Create(StickyNoteCreate createDto);
    public StickyNoteResponse ReadById(int id);
    public StickyNoteResponse Update(StickyNoteUpdate updateDto);
    public StickyNoteResponse Delete(int id);
}