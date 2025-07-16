using DotNetCoreIdentityLearn.DataBase.Entities;
using DotNetCoreIdentityLearn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreIdentityLearn.Controllers;

[Route("/account")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            DateOfBirth = model.DateOfBirth, 
            Email = model.Email,
            UserName = model.Email
        };

        // Store user data in AspNetUsers database table
        var result = await _userManager.CreateAsync(user, model.Password);

        // If user is successfully created, sign-in the user using
        // SignInManager and redirect to index action of HomeController
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        // If there are any errors, add them to the ModelState object
        // which will be displayed by the validation summary tag helper
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [HttpGet]
    [Route("validate/email")]
    public async Task<IActionResult> IsEmailAvailable(string email)
    {
        //Check If the Email ID is Already in the Database
        var user = await _userManager.FindByEmailAsync(email);
        return user == null ? Json(true) : Json($"Email {email} is already in use.");
    }
    
    
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["Tab"] = "Login";
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model,string? returnUrl)
    {
        // If something failed, redisplay form
        if (!ModelState.IsValid) return View(model);
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            // Check if the ReturnUrl is not null and is a local URL
            //An Open Redirect Attack occurs when an application redirects users to an external, potentially malicious URL.
            //By using the IsLocalUrl method,
            //you can ensure that your application only redirects to URLs within your application, thereby avoiding such security risks.
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Redirect to default page
            return RedirectToAction("Index", "Home");
        }

        if (result.RequiresTwoFactor)
        {
            // Handle two-factor authentication case
        }

        if (result.IsLockedOut)
        {
            // Handle lockout scenario
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}