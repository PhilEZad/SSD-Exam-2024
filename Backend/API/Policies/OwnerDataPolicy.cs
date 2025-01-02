using Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace API.Policies;

public class OwnerDataPolicy(IHttpContextAccessor httpContextAccessor, DatabaseContext dbContext)
    : AuthorizationHandler<OwnerDataRequirement>
{
    
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly DatabaseContext _dbContext = dbContext;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerDataRequirement requirement)
    {
        // Get the user ID from the JWT token using the full claim type
        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        // Validate userIdClaim and parse it to an integer, handling non-integer values
        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            Console.WriteLine("Authorization failed: User ID claim is missing or empty.");
            context.Fail();
            return;
        }

        // Attempt to parse the userIdClaim as an integer
        if (!int.TryParse(userIdClaim, out int userId))
        {
            Console.WriteLine($"Authorization failed: Unable to parse User ID '{userIdClaim}' as an integer.");
            context.Fail();
            return;
        }

        if (_httpContextAccessor.HttpContext == null)
        {
            Console.WriteLine("Authorization failed: Invalid or missing resource ID.");
            context.Fail();
            return;
        }
        
        // Attempt to retrieve the resource ID from the request route values
        if (!_httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("id", out var idValue) || !int.TryParse(idValue?.ToString(), out int resourceId))
        {
            Console.WriteLine("Authorization failed: Invalid or missing resource ID.");
            context.Fail();
            return;
        }
        
        // Fetch the resource from the database using the resourceId
        var resource = await _dbContext.NotesTable.FindAsync(resourceId);
        if (resource == null)
        {
            Console.WriteLine($"Authorization failed: Resource with ID {resourceId} not found.");
            context.Fail();
            return;
        }
        
        if (resource.OwnerId == userId)
        {
            context.Succeed(requirement); // User is authorized
            Console.WriteLine($"Authorization succeeded: User ID {userId} is the owner of resource ID {resourceId}.");
        }
        else
        {
            Console.WriteLine($"Authorization failed: User ID {userId} is not the owner of resource ID {resourceId}.");
            context.Fail(); // User is not authorized
        }

    }
}