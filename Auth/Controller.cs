using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        User.IRepository userRepository)
    {
        await userRepository.Create("qwe", "Qwe");
        return Ok();
        
    }

    
    // Выслать код на почту
    [HttpPost("email_confirmation/send_code")]
    public async Task<IActionResult> EmailConfirmationSend(
        EmailConfirmationSendRequest confirmationRequest)
    {
        return Ok();
    }
    
    public record EmailConfirmationSendRequest(
        [Required] [property: JsonPropertyName("email")] string Email);
    
    
    // Проверить верность кода
    [HttpPost("email_confirmation/get_code")]
    public async Task<IActionResult> EmailConfirmationEnter(
        EmailConfirmationEnterRequest confirmationRequest)
    {
        return Ok();
    }

    public record EmailConfirmationEnterRequest(
        [Required] [property: JsonPropertyName("email")] string Email,
        [Required] [property: JsonPropertyName("code")] int Code);
    
    
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