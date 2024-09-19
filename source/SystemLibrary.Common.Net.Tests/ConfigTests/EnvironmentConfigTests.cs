using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Configs;

namespace SystemLibrary.Common.Net.Tests.ConfigTests;

[TestClass]
public class EnvironmentConfigTests
{
    [TestMethod]
    public void Read_Environment_Config_Current_Instance()
    {
        var conf = EnvironmentConfig.Current;

        Assert.IsTrue(conf != null && conf.Name.Is(), "Name invalid");

        if (conf.Name.Contains("rod"))
            Assert.IsTrue(!Configs.EnvironmentConfig.IsLocal, "Local is true");
        else
            Assert.IsTrue(Configs.EnvironmentConfig.IsLocal, "Not local");
    }

    [TestMethod]
    public void Read_Environment_Config_Name_From_Environment_Config_Json_When_No_AspNetCore_Environment_Is_Specified()
    {
        var conf = EnvironmentConfig.Current;

        Assert.IsTrue(conf != null);

        Assert.IsTrue(conf.Name.Is());

        if (conf.Name == "Release" || conf.Name == "Debug" || conf.Name.Contains("rod") || conf.Name.Contains("dev"))
        {
            //Do nothing...
        }
        else
        {
            Assert.IsTrue(conf.Name == "Untransformed", "Unknown configuration mode, should not transform any file - as no such transformation file exists, name: " + conf.Name);
        }
    }

    [TestMethod]
    public void Read_Environment()
    {
        var env = EnvironmentConfig.Current.Name.ToLower();

        if (env == "qa" || env == "stage" || env == "test" || env == "at")
        {
            Assert.IsTrue(EnvironmentConfig.IsTest, "Is test failed for environment startup: " + env);
        }
        else if (env == "production" || env == "prod" || env == "preprod" || env == "preproduction")
        {
            Assert.IsTrue(EnvironmentConfig.IsProd, "Is test failed for environment startup: " + env);
        }
        else if (env == "dev" || env == "development" || env == "local")
        {
            Assert.IsTrue(EnvironmentConfig.IsLocal, "Is test failed for environment startup: " + env);
        }
        else if (env == "unknown")
        {
            Assert.IsTrue(EnvironmentConfig.Current.Name.Is(), "env is unknown, but environment name is null");
        }
        else
        {
            Assert.IsTrue(EnvironmentConfig.IsLocal, env + " is not IsLocal");
        }
    }

    [TestMethod]
    public void Use_EnvironmentConfig_AsIs_From_Library()
    {
        var env = EnvironmentConfig.Current.Name.ToLower();

        if (env == "qa" || env == "stage" || env == "test" || env == "at")
        {
            Assert.IsTrue(EnvironmentConfig.IsTest, "Is test failed for environment startup: " + env);
        }
        else if (env == "production" || env == "prod" || env == "preprod" || env == "preproduction")
        {
            Assert.IsTrue(EnvironmentConfig.IsProd, "Is test failed for environment startup: " + env);
        }
        else if (env == "dev" || env == "development" || env == "local")
        {
            Assert.IsTrue(EnvironmentConfig.IsLocal, "Is test failed for environment startup: " + env);
        }
        else if (env == "unknown")
        {
            Assert.IsTrue(EnvironmentConfig.Current.Name.Is(), "env is unknown, but environment name is null");
        }
        else
        {
            Assert.IsTrue(EnvironmentConfig.IsLocal, env + " is not IsLocal");
        }
    }
}