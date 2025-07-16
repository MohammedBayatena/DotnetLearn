using DotNetCoreIdentityLearn.Views.Administration;

namespace DotNetCoreIdentityLearn.Models;

public class TeamViewModel
{
    public string Name { get; set; }
    public List<UserViewModel> Users { get; set; }
    
    public PermissionViewModel Permissions { get; set; } =  new PermissionViewModel()
    {
        TeamId = Guid.NewGuid().ToString(),
        Level = "Limited",
        TeamName = "Assa"
    };
}