using System.IdentityModel.Tokens.Jwt;

namespace SimpleChat.Server;

public static class HttpContextExtensions
{
    private static bool CheckAuthorization(HttpContext httpContext)
    {
        if (httpContext is null) return false;

        return httpContext.Request.Headers.Keys.Contains("Authorization")
            && !string.IsNullOrEmpty(httpContext.Request.Headers["Authorization"]);
    }

    public static JwtSecurityToken? GetSecurityToken(this HttpContext httpContext)
    {
        if (!CheckAuthorization(httpContext))
        {
            return null;
        }
        return new JwtSecurityToken(httpContext.Request.Headers["Authorization"].FirstOrDefault()?[6..].Trim() ?? string.Empty);
    }
}