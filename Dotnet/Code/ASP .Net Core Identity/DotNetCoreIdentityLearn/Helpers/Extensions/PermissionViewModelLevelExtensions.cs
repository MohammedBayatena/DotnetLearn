using DotNetCoreIdentityLearn.Helpers.Constants;
using DotNetCoreIdentityLearn.Views.Administration;

namespace DotNetCoreIdentityLearn.Helpers.Extensions;

public static class PermissionViewModelLevelExtensions
{
    public static PermissionViewModel MapFlagsByLevel(this PermissionViewModel model,
        ApplicationPermissionAccessConstants level)
    {
        (bool CanRead, bool CanWrite, bool CanDelete, bool CanUpdate) permissions = (level) switch
        {
            ApplicationPermissionAccessConstants.Read => (true, false, false, false),
            ApplicationPermissionAccessConstants.Limited => (true, true, false, false),
            ApplicationPermissionAccessConstants.Full => (true, true, true, true),
            _ => (false, false, false, false)
        };

        model.CanRead = permissions.CanRead;
        model.CanWrite = permissions.CanWrite;
        model.CanDelete = permissions.CanDelete;
        model.CanUpdate = permissions.CanUpdate;

        return model;
    }
}