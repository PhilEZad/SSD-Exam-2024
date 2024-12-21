using Domain;

namespace Application.Interfaces;

public interface INoteRepository
{
    public Note Add(Note add);
    public Note Update(Note update);
    public Note Get(int id);
    public IList<Note> GetAll();
    public bool Delete(int id);
}