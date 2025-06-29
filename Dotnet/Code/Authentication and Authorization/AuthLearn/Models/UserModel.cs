using System.ComponentModel.DataAnnotations;

namespace AuthLearn.Models;

public class UserModel
{
    [Required] [MaxLength(100)] public string Name { get; set; }
    [Required] public string Password { get; set; }
    [Required] [MaxLength(100)] public string Email { get; set; }

    [Required] public DateOnly DateOfBirth { get; set; }
}