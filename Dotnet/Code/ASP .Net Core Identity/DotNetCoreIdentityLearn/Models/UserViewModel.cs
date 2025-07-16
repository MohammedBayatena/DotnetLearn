namespace DotNetCoreIdentityLearn.Models;

public class UserViewModel
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Birthday { get; set; }
    public List<string> Roles { get; set; } = [];
}