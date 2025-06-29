using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLearn.Entities;

public class UserPermissionEntity
{
    [Key] public int Id { get; set; }
    [ForeignKey("UserId")] public int UserId { get; set; }
    public User? User { get; set; }
    public required List<string> Permissions { get; set; }
}