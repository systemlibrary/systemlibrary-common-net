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

            Assert.IsTrue(conf.NewPropertyValue == "12345");

            Assert.IsTrue(conf.Name.Length > 1);

            Assert.IsTrue(conf.IsLocal || conf.IsProd || conf.IsTest);
        }
    }
}
