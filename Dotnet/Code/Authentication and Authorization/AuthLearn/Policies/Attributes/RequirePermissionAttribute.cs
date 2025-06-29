using System.Security.Claims;
using AuthLearn.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthLearn.Policies.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _permissions;

    public RequirePermissionAttribute(params string[] permissions)
    {
        _permissions = permissions;
    }


    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity is { IsAuthenticated: false })
        {
            context.Result = context.Result = new ForbidResult();
            return;
        }

        //Get User Id
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            context.Result = context.Result = new ForbidResult();
            return;
        }

        //Get the Repository
        using var scope = context.HttpContext.RequestServices.CreateScope();
        var permissionsRepository = scope.ServiceProvider.GetRequiredService<IPermissionsRepository>();

        //Check Missing Permissions
        foreach (var permission in _permissions)
        {
            if (await permissionsRepository.HasPermissionAsync(int.Parse(userId), permission)) continue;
            context.Result = context.Result = new ForbidResult();
            break;
        }
    }
}