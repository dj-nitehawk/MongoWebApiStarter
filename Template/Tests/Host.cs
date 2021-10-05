using Funq;
using ServiceStack;
using ServiceStack.Validation;

namespace MongoWebApiStarter.Tests
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("MongoWebApiStarterTests", typeof(MongoWebApiStarter.AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            var settings = new Settings();
            container.AddSingleton(settings);

            Plugins.Add(Authentication.Feature(settings.Auth));

            Plugins.Add(new ValidationFeature());
        }
    }
}
