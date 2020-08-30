using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Tests
{
    [TestClass]
    public static class Context
    {
        private static ServiceStackHost appHost;

        public static string BaseURL { get; set; } = "http://localhost:54321/";

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            appHost = new AppHost()
                          .Init()
                          .Start(BaseURL);

            Task.Run(async () => await DB.InitAsync("MongoWebApiStarterTEST"))
            .GetAwaiter()
            .GetResult();
        }

        [AssemblyCleanup]
        public static void DeInitialize() => appHost.Dispose();
    }
}
