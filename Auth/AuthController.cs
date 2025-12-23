using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.Auth;


[ApiController]
[Route("api/auth")]
public class AuthController
{
    // Сначала подтверждение почты, потом регистрация
    [HttpPost("register")]
    public async Task Register()
    {
        
    }

    [HttpPost("email_confirmation")]
    public async Task EmailConfirmation()
    {
        
    }
    
    [HttpPost("login")]
    public async Task Login()
    {
        
    }
}