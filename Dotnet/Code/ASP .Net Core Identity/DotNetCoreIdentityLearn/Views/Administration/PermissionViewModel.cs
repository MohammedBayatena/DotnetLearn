using DotNetCoreIdentityLearn.Helpers.Attributes;
using DotNetCoreIdentityLearn.Helpers.Constants;

namespace DotNetCoreIdentityLearn.Views.Administration;

public class PermissionViewModel
{
    public required string TeamId { get; set; }

    public required string TeamName { get; set; }

    private string _level;

    public string Level // "Read", "Limited", "Full" , "Custom"
    {
        get => _level;
        init
        {
            _level = value;
            _level = (CanRead, CanWrite, CanDelete, CanUpdate) switch
            {
                (true, false, false, false) => ApplicationPermissionAccessConstants.Read.GetName(),
                (true, true, false, false) => ApplicationPermissionAccessConstants.Limited.GetName(),
                (true, true, true, true) => ApplicationPermissionAccessConstants.Full.GetName(),
                _ => ApplicationPermissionAccessConstants.Custom.GetName()
            };
        }
    }

    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool CanDelete { get; set; }
    public bool CanUpdate { get; set; }
}