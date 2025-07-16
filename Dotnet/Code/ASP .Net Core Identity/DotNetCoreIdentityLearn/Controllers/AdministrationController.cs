using DotNetCoreIdentityLearn.DataBase.Entities;
using DotNetCoreIdentityLearn.Helpers.Constants;
using DotNetCoreIdentityLearn.Helpers.Extensions;
using DotNetCoreIdentityLearn.Models;
using DotNetCoreIdentityLearn.Views.Administration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreIdentityLearn.Controllers;

public class AdministrationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AdministrationController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult LoadPartialCreateRole()
    {
        return PartialView("AddRoleForm", new CreateRoleViewModel());
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpGet]
    public IActionResult UsersList()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    [HttpGet]
    public IActionResult RolesList()
    {
        var roles = _roleManager.Roles.ToList();
        var model = new VerticallyEditableCardsView<ApplicationRole>()
        {
            Items = roles,
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadPartialEditRole(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return NotFound();
        }

        var model = new CreateRoleViewModel()
        {
            RoleName = role.Name!,
            Description = role.Description
        };
        return PartialView("EditRoleForm", model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadPartialUserInfo(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var model = new UserViewModel()
        {
            Id = user.Id,
            Birthday = user.DateOfBirth.ToShortDateString(),
            Email = user.Email!,
            Username = user.FirstName + " " + user.LastName,
            Roles = roles.ToList()
        };
        return PartialView("UserInfo", model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadPartialRoleUsersList(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return NotFound();
        }

        var users = await _userManager.GetUsersInRoleAsync(role.Name!);
        var userViewModels = users.Select(user => new UserViewModel()
        {
            Id = user.Id,
            Username = user.FirstName + " " + user.LastName,
            Birthday = user.DateOfBirth.ToShortDateString(),
            Email = user.Email!,
            Roles = []
        });
        return PartialView("RoleUsersList", userViewModels);
    }

    [HttpGet]
    public IActionResult TeamsList()
    {
        var teams = new List<TeamViewModel>()
        {
            new TeamViewModel()
            {
                Name = "Team1",
                Users = new List<UserViewModel>()
                {
                    new UserViewModel()
                    {
                        Username = "Hatsune Miku",
                        Birthday = DateTime.UtcNow.ToShortDateString(),
                        Email = "mikumikubeam@email.com",
                        Id = Guid.Empty.ToString()
                    },
                    new UserViewModel()
                    {
                        Username = "Luka Migurine",
                        Birthday = DateTime.UtcNow.ToShortDateString(),
                        Email = "wowteto@email.com",
                        Id = Guid.Empty.ToString()
                    },
                    new UserViewModel()
                    {
                        Username = "Kasane Teto",
                        Birthday = DateTime.UtcNow.ToShortDateString(),
                        Email = "fancymail@email.com",
                        Id = Guid.Empty.ToString()
                    }
                }
            },
            new TeamViewModel()
            {
                Name = "Team2",
                Users = new List<UserViewModel>()
                {
                    new UserViewModel()
                    {
                        Username = "Maria",
                        Birthday = DateTime.UtcNow.ToShortDateString(),
                        Email = "sweetDevil@email.com",
                        Id = Guid.Empty.ToString()
                    }
                }, Permissions = new PermissionViewModel()
                {
                    TeamId = Guid.Empty.ToString(),
                    TeamName = "WowBanana"
                }.MapFlagsByLevel(ApplicationPermissionAccessConstants.Read)
            }
        };

        return View(teams);
    }

    [HttpGet]
    public IActionResult LoadEditPermissionsForm(
        string teamId,
        ApplicationPermissionAccessConstants level)
    {
        var permissionsViewModel = new PermissionViewModel()
        {
            CanRead = true, TeamName = "Cats", TeamId = ""
        };

        if (level != ApplicationPermissionAccessConstants.Custom)
        {
            permissionsViewModel = new PermissionViewModel()
            {
                TeamId = teamId,
                TeamName = "Administrators"
            }.MapFlagsByLevel(level);
        }

        return PartialView("PermissionsEditForm", permissionsViewModel);
    }


    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return PartialView("AddRoleForm", model);
        //Check if Role already exists
        var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
        if (roleExists)
        {
            ModelState.AddModelError(string.Empty, "Role already exists");
        }
        else
        {
            // Create the role
            var newRole = new ApplicationRole
            {
                Name = model.RoleName,
                Description = model.Description
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return Json(new { success = true, redirectUrl = Url.Action("RolesList", "Administration") });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return PartialView("AddRoleForm", model);
    }
}