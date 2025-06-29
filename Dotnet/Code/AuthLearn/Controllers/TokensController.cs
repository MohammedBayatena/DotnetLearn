using AuthLearn.Constants;
using AuthLearn.Entities;
using AuthLearn.JWTAuthentication;
using AuthLearn.Models;
using AuthLearn.Policies;
using AuthLearn.Policies.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLearn.Controllers;

[ApiController]
[Route("token/")]
public class TokensController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;

    private readonly User _testUser = new User
    {
        Id = 10005,
        Name = "Hatsune Miku",
        PasswordHash = "passWordHash",
        Email = "HatsuneMiku@MikuWorld.com",
        DateOfBirth = "21-11-1998",
        IsAdmin = true
    };

    public TokensController(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("jwt/create")]
    public async Task<IActionResult> CreateJwtToken()
    {
        var token = await _jwtTokenService.GenerateTokenAsync(_testUser);
        return Ok(token);
    }

    [HttpPost]
    [Route("jwt/refresh")]
    public async Task<IActionResult> RefreshJwtToken([FromBody] RefreshRequestModel model)
    {
        var result = await _jwtTokenService.ValidateRefreshTokenAsync(_testUser, model.Token);
        return result is not null ? Ok(result) : Unauthorized();
    }


    [HttpPatch]
    [Route("jwt/revoke/refreshToken")]
    public async Task<IActionResult> RevokeJwtRefreshToken([FromBody] string refreshToken)
    {
        await _jwtTokenService.InvalidateRefreshTokenAsync(refreshToken);
        return Ok();
    }

    [HttpPatch]
    [Route("jwt/revoke/accessToken")]
    public async Task<IActionResult> RevokeJwtToken([FromBody] string token)
    {
        await _jwtTokenService.InvalidateTokenAsync(token);
        return Ok();
    }


    [HttpGet("jwt/protected")]
    [Authorize("IsAuthenticatedWithJwt")]
    [RequiresClaim(AuthPoliciesConstants.AdminUserClaimType, "true")]
    public IActionResult JwtProtected()
    {
        return Ok("Access Granted");
    }
}