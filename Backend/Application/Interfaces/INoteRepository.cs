using Domain;

namespace Application.Interfaces;

public interface INoteRepository
{
    public Note Create(Note add);
    public Note Update(Note update);
    public Note Read(int id);
    public IList<Note> ReadAllById(int id);
    public bool Delete(int id);
}