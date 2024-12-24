using Application.Interfaces;
using Domain;

namespace Infrastructure;

public class NoteRepository : INoteRepository
{
    private readonly DatabaseContext _DbContext;

    public NoteRepository(DatabaseContext dbContext)
    {
        _DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _DbContext.Database.EnsureCreated();
    }
    public Note Create(Note add)
    {
        throw new NotImplementedException();
    }

    public Note Update(Note update)
    {
        throw new NotImplementedException();
    }

    public Note Read(int id)
    {
        throw new NotImplementedException();
    }

    public IList<Note> ReadAll()
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }
}