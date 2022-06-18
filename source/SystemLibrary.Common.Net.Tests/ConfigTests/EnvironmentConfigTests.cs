using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.ConfigTests
{
    [TestClass]
    public class EnvironmentConfigTests
    {
        /// <summary>
        /// Environment.Name is now equal to, in preceding order:
        /// 
        /// if: environmentConfig.json exists:
        /// - if: it has transformation files:
        ///     - if: a transformation file exists equal to value of 'ASPNETCORE_ENVIRONMENT', then that transformation is ran
        ///     - else if: a transformation file exists equal to value of 'Configuration Mode' in Visual Studio, then that transformation is ran
        ///     
        /// - if: environmentConfig.json exists and has 'name' (transformations are ran before this step, hence name is 'transformed')
        ///     - return 'name' from environmentConfig.json
        /// 
        /// if: 'ASPNETCORE_ENVIRONMENT' exists:
        ///     - return value of 'ASPNETCORE_ENVIRONMENT'
        /// 
        /// else:
        /// - returns "", a blank string, never null
        /// </summary>
        [TestMethod] 
        public void Read_EnvironmentConfig_Current_Instance()
        {
            var conf = EnvironmentConfig.Current;

            Assert.IsTrue(conf != null && conf.Name.Is());
        }

        [TestMethod]
        public void Read_EnvironmentConfig_Name_From_EnvironmentConfigJson_WhenNoAspNetCoreEnvironmentIsSpecified()
        {
            var conf = EnvironmentConfig.Current;

            Assert.IsTrue(conf != null && conf.Name.Is() && conf.Name == "Untransformed", "Name of env is not Untransformed, it is: " + conf.Name);
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
