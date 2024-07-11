using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.AppSettingsTests
{
    [TestClass]
    public class AppSettingsConfigTests
    {
        [TestMethod]
        public void Read_Config_With_Nested_Class_And_With_Encrypted_Decrypted_Values()
        {
            var config = Configs.AppSettingsConfigTests.Current;
            Assert.IsTrue(config != null, "config is null: ");
            Assert.IsTrue(config.DummyUser?.Contains("Hello"), "DummyUser: " + config.DummyUser);
            Assert.IsTrue(config.Parent != null, "Parent is null");
            Assert.IsTrue(config.Parent.Color == "orange", "Parent color");

            Assert.IsTrue(config.PasswordDecrypt == "Hello world");
            Assert.IsTrue(config.PasswordDecrypted == "Hello world");
            Assert.IsTrue(config.HelloWorld == "Hello world", config.HelloWorld);
            Assert.IsTrue(config.HelloWorld2 == "Hello world", config.HelloWorld2);
        }
    }
}

