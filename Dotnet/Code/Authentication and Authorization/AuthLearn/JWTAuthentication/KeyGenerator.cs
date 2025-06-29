using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace AuthLearn.JWTAuthentication;

public static class KeyGenerator
{
    private static readonly RSA PrivateKey;

    static KeyGenerator()
    {
        PrivateKey = RSA.Create();
    }

    public static void GenerateKey()
    {
        var privateKey = PrivateKey.ExportRSAPrivateKey();
        File.WriteAllBytes("PKey", privateKey);
    }

    public static RSA GetRsaKey()
    {
        return PrivateKey;
    }

    public static RsaSecurityKey GetPrivateSecurityKey()
    {
        var rsaKey = RSA.Create();
        rsaKey.ImportRSAPrivateKey(File.ReadAllBytes("PKey"), out _);
        return new RsaSecurityKey(rsaKey);
    }
}