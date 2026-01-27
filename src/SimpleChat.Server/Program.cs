using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMagicOnion()
    .UseRedisGroup(options =>
    {
        options.ConnectionString = "localhost:6379";
        //options.ConnectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
    });


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapMagicOnionService();

app.Run();
