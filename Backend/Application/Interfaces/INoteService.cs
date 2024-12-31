using Application.DTOs.Create;
using Application.DTOs.Response;
using Application.DTOs.Update;

namespace Application.Interfaces;

public interface INoteService
{
    public NoteResponse Create(NoteCreate createDto, int userID);
    public NoteResponse ReadById(int id, int userId);
    public List<NoteResponse> ReadByUser(int id);
    public NoteResponse Update(NoteUpdate updateDto, int userId);
    public bool Delete(int id);
}