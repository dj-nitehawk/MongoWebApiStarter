using Funq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using SCVault.Auth;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Validation;
using System;

namespace SCVault.Tests
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
        public AppHost() : base("SCVaultTests", typeof(Main.Account.Save.Service).Assembly) { }

        public override void Configure(Container container)
        {
            var settings = new Settings();

            container.AddSingleton(settings);
            container.AddSingleton(new DB("SCVaultTEST"));

            Authentication.Initialize(settings);

            Plugins.Add(new AuthFeature(
                () => new UserSession(),
                new[]
                {
                    new JwtAuthProvider()
                    {
                        AuthKeyBase64 = settings.Auth.SigningKey,
                        ExpireTokensIn = TimeSpan.FromMinutes(10)
                    }
                })
            { HtmlRedirect = null });

            Plugins.Add(new ValidationFeature());

            DB.Migrate();
        }
    }
}
