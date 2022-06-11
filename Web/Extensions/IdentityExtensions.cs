using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Web.Extensions;

public static class IdentityExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid? GetUserId(this ClaimsPrincipal principal) =>
        Guid.TryParse(principal.Identity?.Name, out var guid)
            ? guid
            : null;
}