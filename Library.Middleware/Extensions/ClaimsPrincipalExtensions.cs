using System;
using System.Security.Claims;

namespace Library.Middleware.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetGroupId(this ClaimsPrincipal user)
        => user.GetClaimGuid("group_id");

    public static Guid? GetUserId(this ClaimsPrincipal user)
        => user.GetClaimGuid(ClaimTypes.NameIdentifier) ?? user.GetClaimGuid("sub");

    public static long? GetTokenExpiration(this ClaimsPrincipal user)
        => user.GetClaimLong("exp");

    public static string? GetClaimValue(this ClaimsPrincipal user, string claimType)
        => user.FindFirst(claimType)?.Value;

    private static Guid? GetClaimGuid(this ClaimsPrincipal user, string claimType)
    {
        var value = user.FindFirst(claimType)?.Value;
        return Guid.TryParse(value, out var guid) ? guid : null;
    }

    private static long? GetClaimLong(this ClaimsPrincipal user, string claimType)
    {
        var value = user.FindFirst(claimType)?.Value;
        return long.TryParse(value, out var result) ? result : null;
    }
}
