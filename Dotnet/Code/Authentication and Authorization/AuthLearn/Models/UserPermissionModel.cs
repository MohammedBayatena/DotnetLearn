namespace AuthLearn.Models;

public class UserPermissionModel
{
    public required int UserId { get; set; }
    public required List<string> Permissions { get; set; }
}