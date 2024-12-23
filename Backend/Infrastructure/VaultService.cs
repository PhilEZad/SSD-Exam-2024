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
        var secret = await _vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync(path);

        if (secret == null)
        {
            throw new NoNullAllowedException("Secret is null");
        }

        secret.Data.TryGetValue("data", out var keyValueData);

        if (keyValueData == null)
        {
            throw new NoNullAllowedException("Key value data is null");
        }

        var value = ((JsonElement)keyValueData).GetProperty(key).GetString();

        return value ?? throw new NoNullAllowedException("Value is null");
    }


    public Task SealVaultAsync()
    {
        return _vaultClient.V1.System.SealAsync();
    }
}