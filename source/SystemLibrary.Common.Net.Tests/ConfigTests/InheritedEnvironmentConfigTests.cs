using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.ConfigTests
{
    [TestClass]
    public class InheritedEnvironmentConfigTests
    {
        [TestMethod] 
        public void InheritedEnvironmentConfigTests_Current_Instance()
        {
            var conf = Configs.EnvironmentConfig.Current;

            Assert.IsNotNull(conf);
            Assert.IsTrue(conf.NewPropertyValue == "12345");
        }
    }
}
