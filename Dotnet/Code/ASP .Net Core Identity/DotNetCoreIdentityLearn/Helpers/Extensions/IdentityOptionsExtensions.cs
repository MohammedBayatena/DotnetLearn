using Microsoft.AspNetCore.Identity;

namespace DotNetCoreIdentityLearn.Helpers.Extensions;

public static class IdentityOptionsExtensions
{
    public static IdentityOptions AddPasswordConfiguration(this IdentityOptions options)
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredUniqueChars = 4;
        return options;
    }
}