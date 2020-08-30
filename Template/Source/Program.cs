using Funq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Entities;
using MongoWebApiStarter.Auth;
using MongoWebApiStarter.Middleware;
using MongoWebApiStarter.Services;
using ServiceStack;
using ServiceStack.Text;
using ServiceStack.Validation;
using System.Threading.Tasks;

namespace MongoWebApiStarter
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseModularStartup<Startup>()
                   .Build();
    }

    public class Startup : ModularStartup
    {
        private static Settings settings;

        public new void ConfigureServices(IServiceCollection services)
        {
            settings = new Settings();
            Configuration.Bind(nameof(Settings), settings);
            services.AddSingleton(settings);
            services.AddHostedService<EmailService>();
            services.AddHostedService<FileCleanerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            app.UseMaintenanceModeMiddleware();
            app.UseServiceStack(new AppHost());
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("MongoWebApiStarter", typeof(AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            var settings = container.Resolve<Settings>();
            var isDevelopment = container.Resolve<IWebHostEnvironment>().IsDevelopment();

            container.AddSingleton<CloudFlareService>(); container.Resolve<CloudFlareService>();

            SetConfig(new HostConfig
            {
                UseCamelCase = false,
                EnableFeatures = isDevelopment ? Feature.All.Remove(Feature.Html) : Feature.All.Remove(Feature.All).Add(Feature.Json),
            });

            Config.GlobalResponseHeaders.Remove("X-Powered-By");
            JsConfig.IncludeNullValues = true;

            Authentication.Initialize(settings);
            Plugins.Add(Authentication.AuthFeature);
            Plugins.Add(new ValidationFeature());

            if (isDevelopment)
                Plugins.Add(new CorsFeature(allowedHeaders: "*"));

            ServiceExceptionHandlers.Add((_, __, x) =>
            {
                if (x is ValidationError ex)
                    return Validation.GetErrorResponse(ex);

                return null;
            });

            Task.Run(async () =>
            {
                await DB.InitAsync(settings.Database.Name, settings.Database.Host);
                await DB.InitAsync(Constants.FileBucketDB, settings.FileBucket.Host);
                await DB.MigrateAsync();
            })
            .GetAwaiter()
            .GetResult();
        }


    }
}
