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

    public IList<Note> ReadAllById(int id)
    {
        return _DbContext.NotesTable.Where(n => n.Id == id).ToList();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }
}