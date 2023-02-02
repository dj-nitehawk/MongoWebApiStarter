using FastEndpoints.Swagger;
using Microsoft.Extensions.Options;
using MongoWebApiStarter;
using MongoWebApiStarter.Services;

var builder = WebApplication.CreateBuilder();
var configSection = builder.Configuration.GetSection(nameof(Settings));
var notProduction = !builder.Environment.IsProduction();

builder.Services.Configure<Settings>(configSection);
builder.Services.AddHostedService<EmailService>();
builder.Services.AddHostedService<FileCleanerService>();
builder.Services.AddSingleton<CloudFlareService>();
builder.Services.AddResponseCaching();
builder.Services.AddFastEndpoints();
builder.Services.AddJWTBearerAuth(configSection.Get<Settings>().Auth.SigningKey);

if (notProduction)
{
    builder.Services.AddCors();
    builder.Services.AddSwaggerDoc();
}

var app = builder.Build();

if (notProduction)
{
    app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    app.UseSwaggerGen();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = null);

var settings = app.Services.GetRequiredService<IOptions<Settings>>().Value;
await DB.InitAsync(settings.Database.Name, settings.Database.Host);
await DB.InitAsync(settings.FileBucket.Name, settings.FileBucket.Host);
DB.DatabaseFor<Dom.Image>(settings.FileBucket.Name);
await DB.MigrateAsync();
await Notification.Initialize();

app.Run();