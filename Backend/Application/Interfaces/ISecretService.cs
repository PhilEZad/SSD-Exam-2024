namespace Application.Interfaces;

public interface ISecretService
{
    Task<string> GetSecretAsync(string path, string key);
}