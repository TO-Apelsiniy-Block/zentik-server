
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ZenticServer;

public static class ProgramUtils
{
    public static void SetDependencyInjection(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<Chat.IRepository, Chat.Repository>();
        builder.Services.AddScoped<Message.IRepository, Message.Repository>();
        builder.Services.AddScoped<User.IRepository, User.Repository>();
    }
    
    public static void SetAuth(WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("JwtSettings"));
        
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
        var secretKey = jwtSettings.Secret ?? throw 
            new InvalidOperationException("JWT Secret not configured");
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        builder.Services.AddAuthorization();
    }
    
    
}