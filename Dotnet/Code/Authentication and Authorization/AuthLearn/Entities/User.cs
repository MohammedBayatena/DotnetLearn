using System.ComponentModel.DataAnnotations;

namespace AuthLearn.Entities;

public class User
{
    [Required] public int Id { get; set; }
    [Required] [MaxLength(100)] public string Name { get; set; } = "";
    [Required] [MaxLength(1000)] public string PasswordHash { get; set; } = "";
    [Required] [MaxLength(100)] public string Email { get; set; } = "";
    [Required] [MaxLength(10)] public string DateOfBirth { get; set; } = "1-1-1111";
    [Required] public bool IsAdmin { get; set; } = false;
    public UserPermissionEntity? Permissions { get; set; }
}