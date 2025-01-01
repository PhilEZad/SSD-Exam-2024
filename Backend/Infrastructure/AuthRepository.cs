using Application.Interfaces;
using Domain;

namespace Infrastructure;

public class AuthRepository : IAuthRepository
{
    private readonly DatabaseContext _DbContext;

    public AuthRepository(DatabaseContext dbContext)
    {
        _DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _DbContext.Database.EnsureCreated();
    }

    public bool Create(User user)
    {
        var existingUser = _DbContext.UsersTable.Any(x => x.Username == user.Username);
        
        if (existingUser)
            throw new ArgumentException("Username already exists");
        _DbContext.UsersTable.Add(user);
        var result = _DbContext.SaveChanges() > 0;
        return result;
    }

    public User Read(User user)
    {
        var returnUser = _DbContext.UsersTable.First(x => x.Username == user.Username);

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        return returnUser;
    }

    public User Update(User user)
    {
        throw new NotImplementedException();
    }

    public bool Delete(User user)
    {
        throw new NotImplementedException();
    }
}