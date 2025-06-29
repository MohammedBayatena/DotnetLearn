using AuthLearn.Entities;

namespace AuthLearn.Resources;

public class PermissionInfoResource
{
    public int Id { get; set; }
    public List<string> Permissions { get; set; } = [];
    public UserResource? User { get; set; }
}

public static class PermissionInfoResourceExtensions
{
    public static PermissionInfoResource ToPermissionInfoResource(this UserPermissionEntity permissionEntity)
    {
        return new PermissionInfoResource()
        {
            Id = permissionEntity.Id,
            Permissions = permissionEntity.Permissions,
            User = permissionEntity.User?.ToUserResource()
        };
    }
}