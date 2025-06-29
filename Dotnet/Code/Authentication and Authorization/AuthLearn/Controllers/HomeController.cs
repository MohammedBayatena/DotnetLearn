using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLearn.Controllers;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    [HttpGet("home")]
    [Authorize("IsAuthenticated")]
    public IActionResult Home()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/html/index.html");
        return PhysicalFile(filePath, "text/html");
    }

    [HttpGet("accessdenied")]
    public IActionResult AccessDenied()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/html/accessdenied.html");
        return PhysicalFile(filePath, "text/html");
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/html/login.html");
        return PhysicalFile(filePath, "text/html");
    }
    
}