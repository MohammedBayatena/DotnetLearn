using AuthLearn.Entities;

namespace AuthLearn.Resources;

public class UserResource
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
}

public static class UserResourceExtensions
{
    public static UserResource ToUserResource(this User user)
    {
        return new UserResource()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
        };
    }
}