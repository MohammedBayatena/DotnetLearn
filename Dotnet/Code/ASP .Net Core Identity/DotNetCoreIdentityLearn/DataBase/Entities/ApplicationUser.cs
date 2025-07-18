using Microsoft.AspNetCore.Identity;

namespace DotNetCoreIdentityLearn.DataBase.Entities;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
}