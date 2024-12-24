using System.Data;
using System.Reflection;
using System.Text.Json;
using Application.Interfaces;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Infrastructure;

public class VaultService : ISecretService
{
    private const string Address = "http://vault:8200";

    private readonly IVaultClient _vaultClient;

    public VaultService()
    {
        // content root path
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var keyPath = Path.Combine(path, "vault_keys.json");

        var json = File.ReadAllText(keyPath);
        var keys = JsonSerializer.Deserialize<VaultKeys>(json);

        if (keys == null)
        {
            throw new Exception("Vault keys could not be loaded.");
        }

        var authMethod = new TokenAuthMethodInfo(keys.Token);
        var vaultClientSettings = new VaultClientSettings(Address, authMethod);
        _vaultClient = new VaultClient(vaultClientSettings);

        // Unseal the vault
        foreach (var key in keys.Keys)
        {
            _vaultClient.V1.System.UnsealAsync(key).Wait();
        }
    }

    public async Task<string> GetSecretAsync(string path, string key)
    {
        // Read the secret from the KV v1 secrets engine
        var secret = await _vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync(path, mountPoint: "data");

        if (secret?.Data == null)
        {
            throw new KeyNotFoundException($"Secret not found at path: {path}");
        }

        // Try to get the value associated with the specified key
        if (secret.Data.TryGetValue(key, out var value) && value != null)
        {
            return value.ToString();
        }
        else
        {
            throw new KeyNotFoundException($"Key '{key}' not found in secret at path: {path}");
        }
    }


    public Task SealVaultAsync()
    {
        return _vaultClient.V1.System.SealAsync();
    }
}