using Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure;

public class AuthRepository : IAuthRepository
{
    private readonly DatabaseContext _DbContext;

    public AuthRepository(DatabaseContext context)
    {
        _DbContext = context;
        _DbContext.Database.EnsureCreated();
    }
}