using SimpleChat.Server.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddCommunication(builder.Configuration);

builder.Services.AddBackgroundJobs(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapMagicOnionService();

app.Run();

public partial class Program { }