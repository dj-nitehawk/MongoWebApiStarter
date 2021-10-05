using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using ServiceStack;

namespace MongoWebApiStarter.Tests
{
    [TestClass]
    public static class Context
    {
        private static ServiceStackHost appHost;

        public static string BaseURL { get; set; } = "http://localhost:12345/";

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            appHost = new AppHost()
                          .Init()
                          .Start(BaseURL);

            Task.Run(async () =>
            {
                await DB.InitAsync("MongoWebApiStarterTEST");
                await DB.MigrateAsync();
                await Notification.Initialize();
            })
            .GetAwaiter()
            .GetResult();
        }

        [AssemblyCleanup]
        public static void DeInitialize() => appHost.Dispose();
    }
}
