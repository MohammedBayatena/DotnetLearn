using AuthLearn.AuthenticationHandlers;
using AuthLearn.Constants;
using AuthLearn.Repositories;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(
        RequestDelegate next
    )
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IBlacklistedTokensRepository blackListedTokensRepository)
    {
        //If No Authentication continue;
        if (context.User.Identity is not { AuthenticationType: AuthSchemesConstants.JwtScheme })
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers.Authorization.ToString()
            .Replace("Bearer ", "");

        if (await blackListedTokensRepository.ExistsAsync(token))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Token revoked");
            return;
        }

        await _next(context);
    }
}