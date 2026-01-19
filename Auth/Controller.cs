using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.Auth;


[ApiController]
[Route("auth")]
public class Controller : ControllerBase
{
    // Сначала подтверждение почты, потом регистрация
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register(
        IConfiguration config, 
        User.Repository userRepository,
        RegisterRequest registerData,
        EmailConfirmation.Repository emailConfirmationRepository)
    {
        if (! await emailConfirmationRepository.CheckCode(
                registerData.Email, registerData.DeviceId, registerData.EmailCode))
            return BadRequest("Wrong email confirmation code");

        User.Model user;
        try // TODO хеширование
        {
            user = await userRepository.Create(
                registerData.Username, registerData.Password, registerData.Email);
        }
        catch (Exceptions.AlreadyExists ez)
        {
            return Conflict();
        }
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(
            JwtHandler.Create(config.GetSection("JwtSettings").Get<JwtSettings>()!,
                new JwtTokenData(Email: registerData.Email, user.UserId)));
        
        await emailConfirmationRepository.DeleteCode(registerData.Email, registerData.DeviceId);
        return Ok(new LoginResponse(tokenString));
    }
    public record RegisterRequest(
        [Required] [property: JsonPropertyName("username")] string Username,
        [Required] [property: JsonPropertyName("password")] string Password,
        [Required] [property: JsonPropertyName("email")] string Email, // TODO валидация имейла
        [Required] [property: JsonPropertyName("email_code")] int EmailCode, 
        // Код подтверждения почты
        [Required] [property: JsonPropertyName("device_id")] int DeviceId);
    
    
    // Выслать код на почту
    [HttpPost("email_confirmation")]
    public async Task<IActionResult> EmailConfirmationSend(
        EmailConfirmationSendRequest bodyData,
        EmailConfirmation.Repository emailConfirmationRepository,
        Email.Service emailService)
    {
        // TODO поставить ограничитель запросов
        var code = RandomNumberGenerator.GetInt32(100_000, 999_999);
        await emailConfirmationRepository.CreateOrUpdateCode(bodyData.Email, bodyData.DeviceId, code);
        await emailService.SendCode(bodyData.Email, code);
        return Ok();
    }
    
    public record EmailConfirmationSendRequest(
        [Required] [property: JsonPropertyName("email")] string Email,
        [Required] [property: JsonPropertyName("device_id")] int DeviceId);
    
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        IConfiguration config, 
        LoginRequest loginData,
        User.Repository userRepository,
        EmailConfirmation.Repository emailConfirmationRepository)
    {
        
        if (! await emailConfirmationRepository.CheckCode(
                loginData.Email, loginData.DeviceId, loginData.EmailCode))
            return BadRequest("Wrong email confirmation code");

        // Проверка пароля и почты
        User.Model user;
        try
        {
            user = await userRepository.Get(loginData.Email);
        }
        catch (Exceptions.NotFound e)
        {
            return Unauthorized("Wrong email or password");
        }
        if (user.Password != loginData.Password)
            return Unauthorized("Wrong email or password");
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(
            JwtHandler.Create(config.GetSection("JwtSettings").Get<JwtSettings>()!,
            new JwtTokenData(Email: loginData.Email, user.UserId)));

        await emailConfirmationRepository.DeleteCode(loginData.Email, loginData.DeviceId);
        
        return new LoginResponse(tokenString);
    }

    public record LoginRequest(
        [Required] [property: JsonPropertyName("email")] string Email, 
        [Required] [property: JsonPropertyName("password")] string Password,
        [Required] [property: JsonPropertyName("email_code")] int EmailCode, 
        // Код подтверждения почты
        [Required] [property: JsonPropertyName("device_id")] int DeviceId);
        
    public record LoginResponse(
        [Required] [property: JsonPropertyName("token")] string Token
        );


}