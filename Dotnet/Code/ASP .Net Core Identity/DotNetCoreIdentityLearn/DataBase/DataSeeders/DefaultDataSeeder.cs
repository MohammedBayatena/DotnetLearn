using DotNetCoreIdentityLearn.DataBase.Entities;
using DotNetCoreIdentityLearn.Helpers.Attributes;
using DotNetCoreIdentityLearn.Helpers.Constants;
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreIdentityLearn.DataBase.DataSeeders;

public class DefaultDataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<DefaultDataSeeder> _logger;


    private static readonly ApplicationRole[] Roles =
    [
        new()
        {
            Name = ApplicationRolesConstants.Administrator.GetName(),
            Description = ApplicationRolesConstants.Administrator.GetDescription(),
            IsSystemRole = true
        },
        new()
        {
            Name = ApplicationRolesConstants.Manager.GetName(),
            Description = ApplicationRolesConstants.Manager.GetDescription(),
            IsSystemRole = true
        },
        new()
        {
            Name = ApplicationRolesConstants.User.GetName(),
            Description = ApplicationRolesConstants.User.GetDescription(),
            IsSystemRole = true
        }
    ];

    private static readonly ApplicationUser[] Users =
    [
        new()
        {
            FirstName = "Hatsune",
            LastName = "Miku",
            DateOfBirth = new DateOnly(2007, 8, 31),
            Email = "MikuHatsune@WorldIsMine.com",
            UserName = "MikuHatsune@WorldIsMine.com"
        }
    ];

    private static readonly (ApplicationUser user, string roleName)[] UserRoleRelationShips =
    [
        (Users[0], ApplicationRolesConstants.Administrator.GetName())
    ];

    private const string DefaultUserPassword = "Admin@1234"; // Not A Good Practice but whatever

    public DefaultDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
        ILogger<DefaultDataSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }


    public async Task SeedAsync()
    {
        if (!await TrySeedAsync(SeedRolesAsync, "Seeding Default Roles failed")) return;
        if (!await TrySeedAsync(SeedUsersAsync, "Seeding Default Users failed")) return;
        await TrySeedAsync(SeedRolesUsersRelationShips, "Seeding Default Users & Roles RelationShips failed");
    }

    private async Task<bool> SeedRolesUsersRelationShips()
    {
        foreach (var relation in UserRoleRelationShips)
        {
            var user = await _userManager.FindByEmailAsync(relation.user.Email!);
            if (user == null)
            {
                _logger.LogError(
                    "DataBase Seeding Error {Error}",
                    $"Seeding RelationShip Did Not succeed: User {relation.user.UserName} Not Found");
                return false;
            }

            if ((await _userManager.AddToRoleAsync(user, relation.roleName)).Succeeded) continue;
            _logger.LogError(
                "DataBase Seeding Error {Error}",
                $"Seeding Role {relation.roleName} Into User: {relation.user.UserName} Failed");
            return false;
        }

        return true;
    }

    private async Task<bool> SeedUsersAsync()
    {
        foreach (var applicationUser in Users)
        {
            if ((await _userManager.CreateAsync(applicationUser, DefaultUserPassword)).Succeeded) continue;
            _logger.LogError("DataBase Seeding Error {Error}", $"Seeding User: {applicationUser.UserName}  Failed");
            return false;
        }

        return true;
    }

    private async Task<bool> SeedRolesAsync()
    {
        foreach (var applicationRole in Roles)
        {
            if ((await _roleManager.CreateAsync(applicationRole)).Succeeded) continue;
            _logger.LogError("DataBase Seeding Error {Error}", $"Seeding Role: {applicationRole.Name}  Failed");
            return false;
        }

        return true;
    }

    private async Task<bool> TrySeedAsync(Func<Task<bool>> seedFunction, string errorMessage)
    {
        var isSuccess = await seedFunction();
        if (!isSuccess)
        {
            _logger.LogError("DataBase Seeding Error {Error}", errorMessage);
        }

        return isSuccess;
    }
}