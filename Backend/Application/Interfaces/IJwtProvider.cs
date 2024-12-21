using System.Security.Claims;

namespace Application.Interfaces;

public interface IJwtProvider
{ 
    public string GenerateToken(int id, string username, IEnumerable<Claim> additionalClaims = null);
}