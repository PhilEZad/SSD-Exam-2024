using Application.Interfaces;
using Domain;

namespace Infrastructure;

public class NoteRepository : INoteRepository
{
    private readonly DatabaseContext _DbContext;

    public NoteRepository(DatabaseContext dbContext)
    {
        _DbContext = dbContext;
        _DbContext.Database.EnsureCreated();
    }
    public Note Add(Note add)
    {
        throw new NotImplementedException();
    }

    public Note Update(Note update)
    {
        throw new NotImplementedException();
    }

    public Note Get(int id)
    {
        throw new NotImplementedException();
    }

    public IList<Note> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }
}