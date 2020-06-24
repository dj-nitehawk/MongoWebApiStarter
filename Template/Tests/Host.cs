using Funq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using MongoWebApiStarter.Auth;
using ServiceStack;
using ServiceStack.Validation;

namespace MongoWebApiStarter.Tests
{
    [TestClass]
    public static class Init
    {
        public const string BaseUrl = "http://localhost:5002/";
        private static ServiceStackHost appHost;

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            appHost = new AppHost()
                .Init()
                .Start(BaseUrl);
        }

        [AssemblyCleanup]
        public static void Cleanup() => appHost.Dispose();
    }

    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("MongoWebApiStarterTests", typeof(Main.Account.Save.Service).Assembly) { }

        public override void Configure(Container container)
        {
            var settings = new Settings();

            container.AddSingleton(settings);
            container.AddSingleton(new DB("MongoWebApiTEST"));

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
