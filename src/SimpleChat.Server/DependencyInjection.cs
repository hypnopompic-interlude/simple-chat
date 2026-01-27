using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SimpleChat.Server.Authentication;
using SimpleChat.Server.Users;
using SimpleChat.Shared.Users;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtBearerSettings)).Get<JwtBearerSettings>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.SecretKey)),
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.FromSeconds(10),

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                };
#if DEBUG
                options.RequireHttpsMetadata = false;
#endif
            });

        services.Configure<JwtBearerSettings>(configuration.GetSection(JwtBearerSettings.Section));
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }
}

