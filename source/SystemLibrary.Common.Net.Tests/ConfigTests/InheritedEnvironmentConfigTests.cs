using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.ConfigTests
{
    [TestClass]
    public class InheritedEnvironmentConfigTests
    {
        [TestMethod]
        public void Read_Config_Which_Inherits_Environment_Config_Success()
        {
            var conf = Configs.EnvironmentConfig.Current;

            if (Configs.EnvironmentConfig.Current.Name == "Release")
                Assert.IsTrue(conf.NewPropertyValue == "456");

            if (Configs.EnvironmentConfig.Current.Name == "Debug")
                Assert.IsTrue(conf.NewPropertyValue == "7890");

            if (Configs.EnvironmentConfig.Current.Name == "Untransformed")
                Assert.IsTrue(conf.NewPropertyValue == "12345");
        }
    }
}
