using Amazon;
using Amazon.SimpleEmailV2;
using Dom;
using FastEndpoints.Swagger;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoWebApiStarter;
using MongoWebApiStarter.Services;

var bld = WebApplication.CreateBuilder();
var sec = bld.Configuration.GetSection(nameof(Settings));
var cfg = sec.Get<Settings>()!;

bld.Services
   .Configure<Settings>(sec)
   .AddHostedService<FileCleanerService>()
   .AddSingleton<CloudFlareService>()
   .AddSingleton(new AmazonSimpleEmailServiceV2Client(
       awsAccessKeyId: cfg.Email.ApiKey,
       awsSecretAccessKey: cfg.Email.ApiSecret,
       region: RegionEndpoint.USEast1))
   .AddResponseCaching()
   .AddFastEndpoints()
   .AddJobQueues<JobRecord, JobStorageProvider>()
   .AddJWTBearerAuth(cfg.Auth.SigningKey);

if (!bld.Environment.IsProduction())
{
    bld.Services.AddCors();
    bld.Services.SwaggerDocument();
}

BsonSerializer.RegisterSerializer(
    new ObjectSerializer(type =>
        ObjectSerializer.DefaultAllowedTypes(type) ||
        type.Name!.EndsWith("Message")));
await DB.InitAsync(cfg.Database.Name, cfg.Database.Host);
await DB.InitAsync(cfg.JobDatabase.Name, cfg.JobDatabase.Host);
await DB.InitAsync(cfg.FileBucket.Name, cfg.FileBucket.Host);
DB.DatabaseFor<JobRecord>(cfg.JobDatabase.Name);
DB.DatabaseFor<Dom.Image>(cfg.FileBucket.Name);
await DB.MigrateAsync();
await Notification.Initialize();

var app = bld.Build();
app.UseAuthentication()
   .UseAuthorization()
   .UseResponseCaching()
   .UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = null)
   .UseJobQueues(o =>
   {
       o.MaxConcurrency = 4;
       o.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
   });

if (!bld.Environment.IsProduction())
{
    app.UseSwaggerGen();
}

app.Run();