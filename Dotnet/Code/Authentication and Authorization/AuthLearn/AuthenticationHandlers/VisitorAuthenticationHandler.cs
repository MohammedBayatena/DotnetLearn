using System.Security.Claims;
using System.Text.Encodings.Web;
using AuthLearn.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace AuthLearn.AuthenticationHandlers;

public class VisitorAuthenticationHandler : CookieAuthenticationHandler
{
    public VisitorAuthenticationHandler(
        IOptionsMonitor<CookieAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    //Force Sign In as Some Anonymous User , This will create a Visitor Cookie for Each API That requires this Schema in its Authentication
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = await base.HandleAuthenticateAsync();
        if (result.Succeeded) return result;

        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, $"User {userId}"),
            new Claim(ClaimTypes.Sid, sessionId.ToString()),
            new(ClaimTypes.Role, "Visitor")
        ];
        var identity = new ClaimsIdentity(claims, AuthSchemesConstants.VisitorCookieScheme);
        var user = new ClaimsPrincipal(identity);

        await Context.SignInAsync(AuthSchemesConstants.VisitorCookieScheme, user);

        return AuthenticateResult.Success(new AuthenticationTicket(user, AuthSchemesConstants.VisitorCookieScheme));
    }
}