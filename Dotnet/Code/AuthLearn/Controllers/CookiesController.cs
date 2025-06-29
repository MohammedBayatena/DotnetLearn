using System.Security.Claims;
using AuthLearn.Constants;
using AuthLearn.Helpers;
using AuthLearn.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLearn.Controllers;

[ApiController]
[Route("cookie/")]
public class CookiesController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly IBlacklistedSessionsRepository _blacklistedSessionsRepository;

    private const string LocalAuthScheme = AuthSchemesConstants.LocalCookieScheme;

    public CookiesController(IUsersRepository usersRepository,
        IBlacklistedSessionsRepository blacklistedSessionsRepository)
    {
        _usersRepository = usersRepository;
        _blacklistedSessionsRepository = blacklistedSessionsRepository;
    }


    [HttpGet]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await _usersRepository.GetUserByName(userName);

        if (user == null || !_usersRepository.VerifyPassword(user, password))
        {
            return BadRequest("Invalid username or password");
        }

        //Set Cookie Data
        var userClaimsPrincipal = UserHelper.Convert(user, LocalAuthScheme);

        //Create a new SessionId for the User
        var sessionId = Guid.NewGuid().ToString();
        ((ClaimsIdentity)userClaimsPrincipal.Identity!).AddClaim(new Claim(ClaimTypes.Sid, sessionId));
        
        //Login With Cookie
        await HttpContext.SignInAsync(LocalAuthScheme, userClaimsPrincipal);

        return Ok("Cookie was Set - Login Successful");
    }


    [HttpGet]
    [Route("logout")]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(LocalAuthScheme);
        return Ok("Logout Successful");
    }


    [HttpGet]
    [Route("protected")]
    [Authorize("IsAuthenticatedWithLocalCookieScheme")]
    public IActionResult Protected()
    {
        var authSchemes = string.Join(",", HttpContext.User.Identities.Select(i => i.AuthenticationType));
        return Ok($"You are authenticated with the following schemes: {authSchemes}");
    }

    [HttpGet]
    [Route("admin")]
    [Authorize("IsAdmin")]
    public IActionResult Admin()
    {
        return Ok("Admin");
    }

    [HttpGet]
    [Route("adult")]
    [Authorize("IsOlderThan18")]
    public IActionResult Adult()
    {
        return Ok("Adult");
    }

    [HttpGet]
    [Route("blacklist")]
    [Authorize("IsAuthenticated")]
    public async Task<IActionResult> Blacklist()
    {
        //User could have multiple Identities
        var identity =
            HttpContext.User.Identities.FirstOrDefault(scheme => scheme.AuthenticationType == LocalAuthScheme);
        if (identity is null)
            return Ok();
        //Get the Local Scheme SessionId
        var sessionId = identity.FindFirst(ClaimTypes.Sid)?.Value;
        if (sessionId is null)
            return Ok();

        await _blacklistedSessionsRepository.Blacklist(sessionId);
        return Ok($"Blacklisted session: {sessionId}");
    }
}