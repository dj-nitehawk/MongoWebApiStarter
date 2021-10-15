global using FastEndpoints;
global using FastEndpoints.Security;
global using MongoDB.Entities;
using MongoWebApiStarter;
using MongoWebApiStarter.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder();
var configSection = builder.Configuration.GetSection(nameof(Settings));

builder.Services.Configure<Settings>(configSection);
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddHostedService<EmailService>();
builder.Services.AddHostedService<FileCleanerService>();
builder.Services.AddSingleton<CloudFlareService>();
builder.Services.AddCors();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(configSection.Get<Settings>().Auth.SigningKey);

var app = builder.Build();
if (app.Environment.IsDevelopment()) app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();

var settings = app.Services.GetRequiredService<IOptions<Settings>>().Value;
await DB.InitAsync(settings.Database.Name, settings.Database.Host);
await DB.InitAsync(settings.FileBucket.Name, settings.FileBucket.Host);
DB.DatabaseFor<Dom.Image>(settings.FileBucket.Name);
await DB.MigrateAsync();
await Notification.Initialize();

app.Run();

