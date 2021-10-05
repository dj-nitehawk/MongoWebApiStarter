using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Http.Json;
using MongoDB.Entities;
using MongoWebApiStarter;
using MongoWebApiStarter.Services;

var builder = WebApplication.CreateBuilder();

var settings = new Settings();
builder.Configuration.Bind(nameof(Settings), settings);
builder.Services.AddSingleton(settings);

await DB.InitAsync(settings.Database.Name, settings.Database.Host);
await DB.InitAsync(settings.FileBucket.Name, settings.FileBucket.Host);
DB.DatabaseFor<Dom.Image>(settings.FileBucket.Name);
await DB.MigrateAsync();
await Notification.Initialize();

builder.Services.AddSingleton<CloudFlareService>();
builder.Services.AddHostedService<EmailService>();
builder.Services.AddHostedService<FileCleanerService>();

builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(settings.Auth.SigningKey);

builder.Services.AddCors();
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.Run();

public partial class Program { }
