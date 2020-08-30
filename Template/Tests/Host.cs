using Funq;
using MongoWebApiStarter.Auth;
using ServiceStack;
using ServiceStack.Validation;

namespace MongoWebApiStarter.Tests
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("MongoWebApiStarterTests", typeof(MongoWebApiStarter.AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            container.AddSingleton(new Settings());

            Authentication.Initialize(container.Resolve<Settings>());

            Plugins.Add(new AuthFeature(
                () => new UserSession(),
                new[] { Authentication.JWTProvider })
            { HtmlRedirect = null });

            Plugins.Add(new ValidationFeature());
        }
    }
}
