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
builder.Services.AddSingleton<CloudFlareService>();
builder.Services.AddHostedService<EmailService>();
builder.Services.AddHostedService<FileCleanerService>();
builder.Services.AddCors();
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(settings.Auth.SigningKey);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
if (app.Environment.IsDevelopment()) app.UseCors();

settings = app.Services.GetRequiredService<Settings>(); //resolve here to pickup test settings
await DB.InitAsync(settings.Database.Name, settings.Database.Host);
await DB.InitAsync(settings.FileBucket.Name, settings.FileBucket.Host);
DB.DatabaseFor<Dom.Image>(settings.FileBucket.Name);
await DB.MigrateAsync();
await Notification.Initialize();

app.Run();

