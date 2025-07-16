using Microsoft.AspNetCore.Identity;

namespace DotNetCoreIdentityLearn.DataBase.Entities;

public class ApplicationRole : IdentityRole
{
    public required string Description { get; set; }
    
    public bool IsSystemRole { get; set; }
}