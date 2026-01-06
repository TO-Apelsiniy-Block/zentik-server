using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ZenticServer.PushEvents;

namespace ZenticServer;

public class ProgramUtils
{
    private static JwtSettings _jwtSettings;
    private static SseSettings _sseSettings;
    private static WebApplicationBuilder _builder;

    public static void BuilderSetup(WebApplicationBuilder builder)
    {
        new ProgramUtils(builder);
    }
    
    private void SetDependencyInjection()
    {
        _builder.Services.AddScoped<Chat.IRepository, Chat.Repository>();
        _builder.Services.AddScoped<Message.IRepository, Message.Repository>();
        _builder.Services.AddScoped<User.IRepository, User.Repository>();

        var sessionManager = new SseSessionManager(_sseSettings);
        _builder.Services.AddSingleton(sessionManager);
        _builder.Services.AddSingleton(new PushEvents.EventManager(sessionManager));
    }
    
    private void SetAuth()
    {

        var secretKey = _jwtSettings.Secret ?? throw 
            new InvalidOperationException("JWT Secret not configured");
        
        _builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        _builder.Services.AddAuthorization();
    }

    private ProgramUtils(WebApplicationBuilder builder)
    {
        _builder = builder;
        LoadSettings();
        SetAuth();
        SetDependencyInjection();
    }

    private static void LoadSettings()
    {
        _builder.Services.Configure<JwtSettings>(
            _builder.Configuration.GetSection("JwtSettings"));
        
        _jwtSettings = _builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

        _sseSettings = new SseSettings();
    }
    
    
}