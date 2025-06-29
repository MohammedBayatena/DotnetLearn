using System.ComponentModel.DataAnnotations;

namespace AuthLearn.Entities;

public class RefreshTokenInfoEntity
{
    [Required] public required string UserId { get; set; }
    [Required] [Key] public required string Token { get; set; }
    [Required] public required DateTime Expires { get; set; }
    public required bool Valid { get; set; }
}