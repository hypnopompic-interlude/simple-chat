using SimpleChat.Shared.Users;
using System.IdentityModel.Tokens.Jwt;

namespace SimpleChat.Server.Users;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? string.Empty;
        _securityToken = _httpContextAccessor.HttpContext?.GetSecurityToken() ?? new();
    }

    private readonly JwtSecurityToken? _securityToken;
    public JwtSecurityToken? SecurityToken => _securityToken;

    private readonly string _userName;
    public string Name  => _userName;
}