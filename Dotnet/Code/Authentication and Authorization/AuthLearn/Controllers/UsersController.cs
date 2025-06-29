using System.Security.Claims;
using AuthLearn.Models;
using AuthLearn.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLearn.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _usersRepository.GetUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserModel userModel)
    {
        var createdUser = await _usersRepository.RegisterUserAsync(userModel);
        return Ok(createdUser);
    }

    [HttpPatch]
    [Route("SetRole")]
    [Authorize("IsAuthenticatedWithLocalCookieScheme")]
    public IActionResult SetRole([FromBody] string role = "Admin")
    {
        //Get Current Identity
        var userIdentity = (ClaimsIdentity?)HttpContext.User.Identity;
        if (userIdentity is null)
            return BadRequest("Not authenticated");

        //Add Admin Claim
        userIdentity.AddClaim(new Claim(ClaimTypes.Role,role));

        //Force Re-login/Cookie Update with the Scheme user Authenticated With
        var currentAuthScheme = HttpContext.User.Identity!.AuthenticationType;
        HttpContext.SignInAsync(currentAuthScheme, HttpContext.User);

        return Ok("Successfully added role");
    }
}