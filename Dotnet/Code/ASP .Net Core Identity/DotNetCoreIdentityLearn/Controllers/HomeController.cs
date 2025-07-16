using DotNetCoreIdentityLearn.DataBase.Entities;
using DotNetCoreIdentityLearn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreIdentityLearn.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    // GET
    public IActionResult Index()
    {
        ViewData["Tab"] = "Index";
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (User.Identity is not { IsAuthenticated: true })
        {
            return RedirectToAction("Index", "Home");
        }

        var user = _userManager.FindByNameAsync(User.Identity!.Name!).Result;

        if (user is null)
        {
            return RedirectToAction("Index", "Home");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var profileDataModel = new UserViewModel()
        {
            Id = user.Id,
            Email = user.Email!,
            Username = user.FirstName + " " + user.LastName,
            Birthday = user.DateOfBirth.ToShortDateString(),
            Roles = roles.ToList()
        };
        ViewData["Tab"] = "Profile";
        return View(profileDataModel);
    }

    [Route("Error/{statusCode}")]
    public IActionResult Error(string statusCode)
    {
        var model = statusCode switch
        {
            "404" => new ErrorViewModel()
            {
                Name = "Page Not Found",
                Code = "404",
                TextStyle = "text-warning",
                ErrorMessage = "Sorry, the page you're looking for doesn't exist."
            },
            "403" => new ErrorViewModel()
            {
                Name = "Access Denied",
                Code = "403",
                TextStyle = "text-danger",
                ErrorMessage = "You don't have the necessary permissions to access this page."
            },
            _ => new ErrorViewModel()
            {
                Name = "Internal Server Error",
                Code = "500",
                TextStyle = "text-danger",
                ErrorMessage = "Oops, something happened on our side. Try again later."
            }
        };
        return View(model);
    }
}