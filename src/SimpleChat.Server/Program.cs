var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddCommunication(builder.Configuration);

builder.Services.AddBackgroundJobs(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapMagicOnionService();

app.Run();

public partial class Program { }