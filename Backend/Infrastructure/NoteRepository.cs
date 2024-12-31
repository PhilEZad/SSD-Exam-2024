using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

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
        var returnEntity = _DbContext.Add(add);
        _DbContext.SaveChanges();
        return returnEntity.Entity;
    }

    public Note Update(Note update)
    {
        var existingEntity = _DbContext.Find<Note>(update.Id);

        if (existingEntity != null)
        {
            existingEntity.Title = update.Title;
            existingEntity.Content = update.Content;
            existingEntity.Modified = update.Modified;

            _DbContext.Entry(existingEntity).State = EntityState.Modified;
            
            var result = _DbContext.SaveChanges();

            if (result > 0)
            {
                return existingEntity;
            }
            else
            {
                throw new Exception("Note could not be updated");
            }
            
        }
        
        throw new Exception("Credentials not found.");
    }

    public Note Read(int id)
    {
        var returnEntity = _DbContext.NotesTable.First(n => n.Id == id);
        return returnEntity;
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