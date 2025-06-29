using System.Security.Cryptography;
using System.Text;

namespace AuthLearn.Helpers;

public class TokenHasher
{
    public static string HashToken(string token)
    {
        var tokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
        return tokenHash;
    }
}