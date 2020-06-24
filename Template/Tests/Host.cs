using Funq;
using MongoDB.Entities;
using MongoWebApiStarter.Auth;
using ServiceStack;
using ServiceStack.Validation;

namespace MongoWebApiStarter.Tests
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("MongoWebApiStarterTests", typeof(Main.Account.Save.Service).Assembly) { }

        public override void Configure(Container container)
        {
            var settings = new Settings();

            container.AddSingleton(settings);
            container.AddSingleton(new DB("MongoWebApiStarterTEST"));

            Authentication.Initialize(settings);
            Plugins.Add(new AuthFeature(
                () => new UserSession(),
                new[] { Authentication.JWTProvider })
            { HtmlRedirect = null });

            Plugins.Add(new ValidationFeature());

            DB.Migrate();
        }
    }
}
