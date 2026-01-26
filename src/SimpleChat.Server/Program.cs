using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMagicOnion()
    .UseRedisGroup(options =>
    {
        //options.ConnectionString = "localhost:6379";
        //options.ConnectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
    });

var app = builder.Build();

app.MapMagicOnionService();

app.Run();
