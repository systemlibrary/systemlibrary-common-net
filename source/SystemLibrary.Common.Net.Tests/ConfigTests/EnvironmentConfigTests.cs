using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.ConfigTests
{
    [TestClass]
    public class EnvironmentConfigTests
    {
        [TestMethod] 
        public void Read_EnvironmentConfig_Current_Instance()
        {
            var conf = EnvironmentConfig.Current;

            Dump.Write(conf.Name);
            Assert.IsTrue(conf != null && conf.Name.Is());
        }

        [TestMethod]
        public void Read_EnvironmentConfig_Name_From_EnvironmentConfigJson_WhenNoAspNetCoreEnvironmentIsSpecified()
        {
            var conf = EnvironmentConfig.Current;

            Assert.IsTrue(conf != null);
            Assert.IsTrue(conf.Name.Is());

            if (conf.Name.Is())
            {
                if(conf.Name == "Release" || conf.Name == "Debug")
                {
                    //Do nothing...
                }
                else
                {
                    Assert.IsTrue(conf.Name == "Untransformed", "Unknown configuration mode, should not transform any file - as no such transformation file exists, name: " + conf.Name);
                }
            }
        }

        [TestMethod]
        public void Read_Environment()
        {
            var env = EnvironmentConfig.Current.Name.ToLower();

            if (env == "qa" || env == "stage" || env == "test" || env == "at")
            {
                Assert.IsTrue(EnvironmentConfig.Current.IsTest, "Is test failed for environment startup: " + env);
            }
            else if (env == "production" || env == "prod" || env == "preprod" || env == "preproduction")
            {
                Assert.IsTrue(EnvironmentConfig.Current.IsProd, "Is test failed for environment startup: " + env);
            }
            else if(env == "dev" || env == "development" || env == "local")
            {
                Assert.IsTrue(EnvironmentConfig.Current.IsLocal, "Is test failed for environment startup: " + env);
            }
            else if(env == "unknown")
            {
                Assert.IsTrue(EnvironmentConfig.Current.Name.Is(), "env is unknown, but environment name is null");
            }
            else
            {
                Assert.IsTrue(EnvironmentConfig.Current.IsLocal, env + " is not IsLocal");
            }
        }
    }
}
