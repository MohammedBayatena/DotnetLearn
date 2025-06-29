using System.ComponentModel.DataAnnotations;

namespace AuthLearn.Entities;

public class TokenBlackListEntity
{
    [Required] [Key] public required string TokenHash { get; set; }
}