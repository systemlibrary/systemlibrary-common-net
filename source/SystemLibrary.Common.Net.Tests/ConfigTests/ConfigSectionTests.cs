using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Models;

namespace SystemLibrary.Common.Net.Tests.Tests
{
    [TestClass]
    public partial class ConfigTests
    {
        [TestMethod]
        public void Read_Test_Config()
        {
            var config = TestConfig.Current;

            Assert.IsTrue(config != null, "TestConfig is null, your configuration is wrong or mismatch with the className in web/app-config");
            Assert.IsTrue(config.List.Count > 1, "List does not exist in your config file, or it is empty, or it is too short.");
            Assert.IsTrue(config.Name == "Hello world");

            foreach(TestCollectionItem item in config.List)
            {
                Assert.IsTrue(item.Number > 0);
            }
        }
    }
}
