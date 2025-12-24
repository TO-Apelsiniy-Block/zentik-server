using System.Security.Claims;

namespace ZenticServer.Auth;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    { // Сделать нормальную обработку на все случаи долбаебов
        return int.Parse(user.FindFirst(JwtClaimFields.UserId)?.Value);
    }
    
    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst(JwtClaimFields.Email)?.Value;
    }
    
    public static string GetRole(this ClaimsPrincipal user)
    {
        return user.FindFirst(JwtClaimFields.Role)?.Value;
    }
}
