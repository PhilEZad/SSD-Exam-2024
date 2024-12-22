using Application.Interfaces;
using Domain;

namespace Infrastructure;

public class AuthRepository : IAuthRepository
{
    private readonly DatabaseContext _DbContext;

    public AuthRepository(DatabaseContext context)
    {
        _DbContext = context;
        _DbContext.Database.EnsureCreated();
    }

    public bool Create(User user)
    {
        throw new NotImplementedException();
    }

    public User Read(User user)
    {
        throw new NotImplementedException();
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