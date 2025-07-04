using AuthLearn.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthLearn.Policies.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresClaimAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _claimType;
    private readonly string _claimValue;

    public RequiresClaimAttribute(string claimType, string claimValue)
    {
        _claimType = claimType;
        _claimValue = claimValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.HasClaim(_claimType, _claimValue))
        {
            context.Result = new ForbidResult(AuthSchemesConstants.JwtScheme);
        }
    }
}