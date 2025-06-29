using System.Security.Claims;
using AuthLearn.Entities;

namespace AuthLearn.Helpers;

public class UserHelper
{
    public static ClaimsPrincipal Convert(User user,string scheme)
    {
        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.DateOfBirth, user.DateOfBirth)
        ];
        
        var identity = new ClaimsIdentity(claims,scheme);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return claimsPrincipal;
    }
}