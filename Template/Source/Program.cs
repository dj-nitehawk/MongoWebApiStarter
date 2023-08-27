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
var stn = sec.Get<Settings>()!;

bld.Services
   .Configure<Settings>(sec)
   .AddHostedService<FileCleanerService>()
   .AddSingleton<CloudFlareService>()
   .AddSingleton(new AmazonSimpleEmailServiceV2Client(
       awsAccessKeyId: stn.Email.ApiKey,
       awsSecretAccessKey: stn.Email.ApiSecret,
       region: RegionEndpoint.USEast1))
   .AddResponseCaching()
   .AddFastEndpoints()
   .AddJobQueues<JobRecord, JobStorageProvider>()
   .AddJWTBearerAuth(stn.Auth.SigningKey);

if (!bld.Environment.IsProduction())
{
    bld.Services.AddCors();
    bld.Services.SwaggerDocument();
}

var app = bld.Build();
app.UseAuthentication()
   .UseAuthorization()
   .UseResponseCaching()
   .UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = null);

await InitDatabase();

app.UseJobQueues(o =>
{
    o.MaxConcurrency = 4;
    o.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
});

if (!bld.Environment.IsProduction())
{
    app.UseSwaggerGen();
}

app.Run();

async Task InitDatabase()
{
    BsonSerializer.RegisterSerializer(new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) || type.Name!.EndsWith("Message")));
    await DB.InitAsync(stn.Database.Name, stn.Database.Host);
    await DB.InitAsync(stn.JobDatabase.Name, stn.JobDatabase.Host);
    await DB.InitAsync(stn.FileBucket.Name, stn.FileBucket.Host);
    DB.DatabaseFor<JobRecord>(stn.JobDatabase.Name);
    DB.DatabaseFor<Dom.Image>(stn.FileBucket.Name);
    await DB.MigrateAsync();
    await Notification.Initialize();
}

public partial class Program { };