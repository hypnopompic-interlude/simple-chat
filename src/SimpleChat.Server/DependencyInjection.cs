using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using SimpleChat.Server.Authentication;
using SimpleChat.Server.Chats;
using SimpleChat.Server.Services;
using SimpleChat.Server.Users;
using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Services;
using SimpleChat.Shared.Users;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCommunication(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(ChatHubSettings)).Get<ChatHubSettings>();

        services.AddMagicOnion()
            .UseRedisGroup(options =>
            {
                options.ConnectionString = settings?.RedisConnectionString ?? string.Empty;
                //options.ConnectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
            });

        services.AddSingleton<IGroupService, GroupService>();

        services.AddTransient<INotificationSender<ChatTextMessageModel>, ChatNotificationSender>();

        services.AddSingleton<ISystemMessageProvider, ChatSystemMessageProvider>();

        return services;
    }

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

                options.RequireHttpsMetadata = true;
            });

        services.Configure<JwtBearerSettings>(configuration.GetSection(JwtBearerSettings.Section));
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(configure =>
        {
            var settings = configuration.GetSection(nameof(BackgroundSettings)).Get<BackgroundSettings>() ?? new();
            var jobKey = new JobKey(nameof(ServerNotificationProcessingJob));

            configure
                .AddJob<ServerNotificationProcessingJob>(jobKey, default(Action<IJobConfigurator>))
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(settings.PeriodicNotificationTimeSpanSec).RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}

