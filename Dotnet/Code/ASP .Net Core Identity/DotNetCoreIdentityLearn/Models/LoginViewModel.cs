using System.ComponentModel.DataAnnotations;

namespace DotNetCoreIdentityLearn.Models;

public class LoginViewModel
{
    [Required] [DataType(DataType.EmailAddress)] public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
    
}