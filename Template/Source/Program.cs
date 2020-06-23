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
using MongoWebApiStarter.Tools;
using ServiceStack;
using ServiceStack.Text;
using ServiceStack.Validation;

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
        public new void ConfigureServices(IServiceCollection services)
        {
            var settings = new Settings();
            Configuration.Bind("Settings", settings);
            services.AddSingleton(settings);
            services.AddHostedService<EmailService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMaintenanceModeMiddleware();

            app.UseServiceStack(new AppHost()
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }

    public class AppHost : AppHostBase
    {
        private readonly Settings settings = new Settings();

        public AppHost() : base("SCVault", typeof(Main.Account.Save.Service).Assembly) { }

        public override void Configure(Container container)
        {
            Configuration.Bind("Settings", settings);

            container.AddSingleton(settings);
            container.AddSingleton<CloudFlareService>(); container.Resolve<CloudFlareService>();
            container.AddSingleton(new DB(settings.Database.Name));

            SetConfig(new HostConfig
            {
                UseCamelCase = false,
                EnableFeatures = Feature.All.Remove(Feature.All).Add(Feature.Json)
            });

            Config.GlobalResponseHeaders.Remove("X-Powered-By");
            JsConfig.IncludeNullValues = true;

            Authentication.Initialize(settings);
            Plugins.Add(Authentication.AuthFeature);
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new CorsFeature(allowedHeaders: "*")); //todo: remove wildcard after deploymenet to production

            ServiceExceptionHandlers.Add((_, __, x) =>
            {
                if (x is ValidationError ex)
                    return Validation.GetErrorResponse(ex);

                return null;
            });

            DB.Migrate();
        }
    }
}
