using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.Auth;



[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
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
    public async Task<ActionResult<LoginResponse>> Login(IConfiguration config, LoginRequest loginData)
    {
        if (!true)
        {
            return Unauthorized();
        }
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(
            JwtHandler.Instance.Create(config.GetSection("JwtSettings").Get<JwtSettings>()!,
            new JwtTokenData(Email: loginData.Email)));

        return new LoginResponse(tokenString);
    }



    public record LoginRequest(
        [Required] [property: JsonPropertyName("email")] string Email, 
        [Required] [property: JsonPropertyName("password")] string Password);
    public record LoginResponse(string Token);


}