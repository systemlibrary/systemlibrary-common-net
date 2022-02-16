using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Configs;
using SystemLibrary.Common.Net.Tests.Configurations;

namespace SystemLibrary.Common.Net.Tests.ConfigTests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod] 
        public void Read_NonExistingSettings_DoesNotThrow()
        {
            var conf = NonExistingSettings.Current;

            Assert.IsTrue(conf != null);
        }

        [TestMethod]
        public void Read_SubDirectorySetting_InConfigsFolder_WithTransformations()
        {
            var conf = IntegrationSettings.Current;

            //NOTE: Build mode is defined in /properties/mstest.runsettings
            //NOTE: The project must also contain the 'mode' else 'Release' is being used in transformations
            var mode = EnvironmentConfig.AspNetCoreEnvironment;

            Assert.IsTrue(conf != null, "A file 'CarSettings.xml' or 'CarSettings.json' must exist in either ~/Configs/ or ~/Configurations/ or root: ~/");
            if (mode == "Release")
            {
                Assert.IsTrue(conf.FirstName?.Contains("Release") == true, "firstname is invalid, it must be get; and set; property");
                Assert.IsTrue(conf.lastname?.Contains("Release") == true, "LastName is invalid, it must be get; and set; property");
                Assert.IsTrue(conf.age > 200, "Age is an invalid int, or not within the range");
            }
            else if(mode == "Debug")
            {
                //NOTE: We are building "Release" configuration, so the test setting "Debug" do not work, unless we also specify a 'Debug' transformation file  
                //Assert.IsTrue(conf.lastname?.Contains("Release") == false, "LastName contains Release: " + conf.lastname);
                Assert.IsTrue(conf.age > 0, "Age is an invalid int, or not within the range");
            }
            else
            {
                Assert.IsTrue(false, "Error: Mode should be either release or debug, it is: " + mode);
            }

            Assert.IsTrue(conf.IsEnabled);
        }

        [TestMethod]
        public void Read_Json_From_ConfigsFolder_With_Transformations_BasedOnBuildMode()
        {
            var conf = CarSettings.Current;

            //NOTE: Build mode is defined in /properties/mstest.runsettings
            var mode = EnvironmentConfig.AspNetCoreEnvironment;
            Assert.IsTrue(conf != null, "A file 'CarSettings.xml' or 'CarSettings.json' must exist in either ~/Configs/ or ~/Configurations/ or root: ~/");
            Assert.IsTrue(conf.Car != null, "A property 'Car' must exist, with type of class 'Car'");

            if (mode == "Release")
            {
                Assert.IsTrue(conf.FirstName?.Contains("Release") == true, "firstname is invalid, it must be get; and set; property");
                Assert.IsTrue(conf.lastname?.Contains("Release") == true, "LastName is invalid, it must be get; and set; property");
                Assert.IsTrue(conf.age > 200, "Age is an invalid int, or not within the range");
            }
            else if(mode == "Debug")
            {
                Assert.IsTrue(conf.FirstName?.Contains("Debug") == true, "firstname is invalid, it must be get; and set; property");
                Assert.IsTrue(conf.lastname?.Contains("Debug") == true, "LastName is invalid, it must be get; and set; property");
                Assert.IsTrue(conf.lastname?.Contains("Release") == false, "LastName is invalid, it must be get; and set; property");
                Assert.IsTrue(conf.age > 10 && conf.age < 100, "Age is an invalid int, or not within the range");
            }
            else
            {
                Assert.IsTrue(conf.age > 0, "Age is an invalid int, or not within the range");
            }
            Assert.IsTrue(conf.isEnabled, "IsEnabled is not true, must be a property with get; and set;");
        }

        [TestMethod]
        public void Read_Xml_Config()
        {
            var conf = HumanConfigs.Current;

            Assert.IsTrue(conf != null, "A file 'humanconfigs.xml' or 'humanconfigs.json' must exist in either ~/Configs/ or ~/Configurations/ or root: ~/");
            Assert.IsTrue(conf.firstname?.Contains("Users") == true, "firstname is invalid, it must be get; and set; property");
            Assert.IsTrue(conf.LastName?.Contains("Users") == true, "LastName is invalid, it must be get; and set; property");
            Assert.IsTrue(conf.Phone > 100, "Phone is an invalid int, or not within the range");
            Assert.IsTrue(conf.IsAlive, "IsAlive is not true in the config file, or is not a get; set; property");
            Assert.IsTrue(conf.IsAliveCapital, "IsAliveCapital is not true, or is not a get; set; property");

            var conf2 = HumanConfigs.Current;

            Assert.IsTrue(conf == conf2, "The static variable have changed - not a singleton");
        }
    }
}
