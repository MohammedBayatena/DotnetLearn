using System.ComponentModel.DataAnnotations;

namespace DotNetCoreIdentityLearn.Models;

public class CreateRoleViewModel
{
    [Required] [Display(Name = "Role")] public string RoleName { get; set; }
    [Display(Name = "Description")] public string Description { get; set; }
}