using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreIdentityLearn.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "First Name is Required")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    [DataType(DataType.Text)]
    public string? LastName { get; set; }

    [Required]
    [Display(Name = "Birthday")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }


    [Remote(action: "IsEmailAvailable", controller: "Account")]
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = "";
}