using Domain;

namespace Application.Interfaces;

public interface IAuthRepository
{
    public bool Create(User user);
    public User Read(User user);
    public User Update(User user);
    public bool Delete(User user);
}