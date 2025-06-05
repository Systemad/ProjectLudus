using System.Security.Claims;

namespace Ludus.Server.Features.Auth.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        if (Guid.TryParse(user?.Identity?.Name, out var userId))
        {
            return userId;
        }

        throw new UnauthorizedAccessException("Invalid or missing user ID.");
    }
}
