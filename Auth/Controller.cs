using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.Auth;


[ApiController]
[Route("auth")]
public class Controller : ControllerBase
{
    // Сначала подтверждение почты, потом регистрация
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        User.IRepository userRepository,
        RegisterRequest bodyData)
    {
        // TODO хеширование
        await userRepository.Create(bodyData.Username, bodyData.Password, bodyData.Email);
        return Ok();
    }
    public record RegisterRequest(
        [Required] [property: JsonPropertyName("username")] string Username,
        [Required] [property: JsonPropertyName("password")] string Password,
        [Required] [property: JsonPropertyName("email")] string Email,
        [Required] [property: JsonPropertyName("email_code")] string EmailCode // Код подтверждения почты
        );
    

    
    // Выслать код на почту
    [HttpPost("email_confirmation/send_code")]
    public async Task<IActionResult> EmailConfirmationSend(
        EmailConfirmationSendRequest bodyData)
    {
        // TODO определение 
        return Ok();
    }
    
    public record EmailConfirmationSendRequest(
        [Required] [property: JsonPropertyName("email")] string Email);
    
    
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(IConfiguration config, LoginRequest loginData)
    {
        if (!true)
        {
            return Unauthorized();
        }
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(
            JwtHandler.Instance.Create(config.GetSection("JwtSettings").Get<JwtSettings>()!,
            new JwtTokenData(Email: loginData.Email, 3)));

        return new LoginResponse(tokenString);
    }

    public record LoginRequest(
        [Required] [property: JsonPropertyName("email")] string Email, 
        [Required] [property: JsonPropertyName("password")] string Password);
    public record LoginResponse(string Token);


}