using Microsoft.AspNetCore.Mvc;

namespace SzyCo.Garage.Web;

public static class ReturnUrlHelper
{
    public static string GetLocalOrDefault(IUrlHelper urlHelper, string? returnUrl, string defaultValue = "/")
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && urlHelper.IsLocalUrl(returnUrl)
            ? returnUrl
            : defaultValue;
    }
}
