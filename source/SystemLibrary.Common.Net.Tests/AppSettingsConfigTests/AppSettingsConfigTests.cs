using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.AppSettingsTests
{
    [TestClass]
    public class AppSettingsConfigTests
    {
        [TestMethod]
        public void Read_Config_Extension()
        {
            var config = Configs.AppSettingsConfigTests.Current;
            Assert.IsTrue(config != null, "config is null: ");
            Assert.IsTrue(config.DummyUser?.Contains("Hello"), "DummyUser: " + config.DummyUser);
            Assert.IsTrue(config.Parent != null, "Parent is null");
            Assert.IsTrue(config.Parent.Color == "orange", "Parent color");
        }
    }
}

