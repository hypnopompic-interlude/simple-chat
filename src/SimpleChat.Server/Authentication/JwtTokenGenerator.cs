using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleChat.Shared.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimpleChat.Server.Authentication;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly JwtBearerSettings _settings;

    public JwtTokenGenerator(IOptions<JwtBearerSettings> options) => _settings = options.Value;

    public string GenerateToken(ChatUser user)
    {
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_settings.SecretKey));
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        
        return jwtTokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor()
        {
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name)                
            }),
            //Expires = DateTime.UtcNow.AddSeconds( _settings.ExpiresSec),
        });
    }
}