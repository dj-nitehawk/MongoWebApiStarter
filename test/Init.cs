using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;

namespace App.Test
{
    [TestClass]
    public static class InitTest
    {
        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            new DB("MongoWebApiStarter-TEST");
        }
    }
}
