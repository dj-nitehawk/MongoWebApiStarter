global using FastEndpoints;
global using FastEndpoints.Security;
global using FastEndpoints.Validation;
global using MongoDB.Entities;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using MongoWebApiStarter;
using MongoWebApiStarter.Services;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder();
var configSection = builder.Configuration.GetSection(nameof(Settings));
builder.Services.Configure<Settings>(configSection);
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddHostedService<EmailService>();
builder.Services.AddHostedService<FileCleanerService>();
builder.Services.AddSingleton<CloudFlareService>();

builder.Services.AddCors();
builder.Services.AddResponseCaching();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(configSection.Get<Settings>().Auth.SigningKey);
builder.Services.AddSwagger();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseCors(b => b
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
}

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.UseFastEndpoints();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.DocExpansion(DocExpansion.None);
        o.DefaultModelExpandDepth(0);
    });
}

var settings = app.Services.GetRequiredService<IOptions<Settings>>().Value;
await DB.InitAsync(settings.Database.Name, settings.Database.Host);
await DB.InitAsync(settings.FileBucket.Name, settings.FileBucket.Host);
DB.DatabaseFor<Dom.Image>(settings.FileBucket.Name);
await DB.MigrateAsync();
await Notification.Initialize();

app.Run();