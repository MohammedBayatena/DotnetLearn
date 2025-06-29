using System.ComponentModel.DataAnnotations;

namespace AuthLearn.Entities;

public class SessionBlackListEntity
{
    [Required] [Key] public required string SessionId { get; set; }
}