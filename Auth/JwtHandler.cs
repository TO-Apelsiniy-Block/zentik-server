using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ZenticServer.Auth;


public class JwtHandler
{
    private static readonly Lazy<JwtHandler> _instance = 
        new Lazy<JwtHandler>(() => new JwtHandler());

    public static JwtHandler Instance => _instance.Value;

    private JwtHandler()
    { }
    
    public JwtSecurityToken Create(JwtSettings jwtSettings, JwtTokenData tokenData)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("email", tokenData.Email),
            new Claim("user_id", tokenData.Email.GetHashCode().ToString()),
            new Claim("role", "user")
        };

        return new JwtSecurityToken(
            claims: claims,
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );
    }
}

public record JwtTokenData(string Email); 