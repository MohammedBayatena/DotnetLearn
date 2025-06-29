using System.ComponentModel.DataAnnotations;

namespace AuthLearn.Models;

public class RefreshRequestModel
{
    [Required] public string Token { get; set; }
}