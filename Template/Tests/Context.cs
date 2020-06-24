using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;

namespace MongoWebApiStarter.Tests
{
    /// <summary>
    /// The testing context ready to go with a client
    /// </summary>
    [TestClass]
    public static class CTX
    {
        private static ServiceStackHost appHost;

        /// <summary>
        /// The json client for making http requests to the service endpoints
        /// </summary>
        public static JsonServiceClient Client { get; private set; }

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            const string baseURL = "http://localhost:54321/";

            appHost = new AppHost()
                .Init()
                .Start(baseURL);

            Client = new JsonServiceClient(baseURL);
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            Client.Dispose();
            appHost.Dispose();
        }
    }
}
