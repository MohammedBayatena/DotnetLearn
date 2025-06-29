using AuthLearn.AuthenticationHandlers;
using AuthLearn.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLearn.Controllers;

[ApiController]
[Route("oauth/")]
public class OAuthController : ControllerBase
{
    private const string GithubOAuthScheme = AuthSchemesConstants.GithubOAuthScheme;
    private const string MockOAuthScheme = AuthSchemesConstants.MockOAuthScheme;
    private const string RedirectInto = "/home"; // could be saved in Appsettings Instead

    [HttpGet]
    [Route("github/login")]
    public IResult GithubLogin()
    {
        return Results.Challenge(authenticationSchemes: [GithubOAuthScheme], properties: new OAuthChallengeProperties()
        {
            RedirectUri = RedirectInto
        });
    }

    [HttpGet]
    [Route("github/user")]
    [Authorize("IsAuthenticatedWithGitHub")]
    public IActionResult GithubUser()
    {
        if (HttpContext.User.Identity is { IsAuthenticated : false })
        {
            return Unauthorized();
        }

        return Ok(HttpContext.User.Claims.Select(x => new { x.Type, x.Value }).ToList());
    }
    
    // Mock OAuth Server
    [HttpGet]
    [Route("mock/login")]
    public IActionResult LoginOAuth()
    {
        // Challenge in the concept of Oauth : Redirect to OAuth Portal so go sign in Bruh
        HttpContext.ChallengeAsync(MockOAuthScheme, new AuthenticationProperties()
        {
            // After Sign in with the Portal , Return to This Path or else it returns to the samePath that called it
            RedirectUri = RedirectInto
        });
        return Empty;
    }
    
    [HttpGet]
    [Route("mock/protected")]
    [Authorize("IsAuthenticatedWithOAuth")]
    public IActionResult OAuthProtected()
    {
        var user = HttpContext.User;
        return Ok(user.Claims.Select(x => new { x.Type, x.Value }).ToList());
    }
    
    
    
}