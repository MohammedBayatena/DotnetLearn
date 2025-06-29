using Microsoft.AspNetCore.DataProtection;

namespace AuthLearn.Services;

public interface ILoginService
{
    public void Login();
}

public class LoginService : ILoginService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public LoginService(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataProtectionProvider = dataProtectionProvider;
    }

    public void Login()
    {
        var protector = _dataProtectionProvider.CreateProtector("Auth-Cookie");
        var encryptedPayload = $"UserName={protector.Protect("Bruh")}";
        _httpContextAccessor.HttpContext!.Response.Headers.SetCookie = encryptedPayload;
    }
    
}