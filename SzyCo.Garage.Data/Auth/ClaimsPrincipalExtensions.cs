﻿namespace SzyCo.Garage.Data.Auth;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
        => user.FindFirstValue(AppClaimTypes.UserId);

    public static string? GetUserName(this ClaimsPrincipal user)
        => user.FindFirstValue(AppClaimTypes.UserName);

    public static string? GetEmail(this ClaimsPrincipal user)
        => user.FindFirstValue(AppClaimTypes.Email);

    public static bool Can(this ClaimsPrincipal user, params Permission[] permissions)
        => permissions.Any(p => user.HasClaim(AppClaimTypes.Permission, p.ToString()));

}