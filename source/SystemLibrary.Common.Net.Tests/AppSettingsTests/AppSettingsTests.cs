using System;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.AppSettingsTests
{
    [TestClass]
    public class AppSettingsTests
    {
        [TestMethod]
        public void Read_Root_AppSettings_Config()
        {
            var config = GetLibraryAppSettingsConfig();
            var configurationProperty = GetLibraryAppSettingsConfigPropertyInfo();

            var configuration = configurationProperty.GetValue(config);

            var dumpProperty = configuration.GetType().GetProperties()
                .Where(x => x.Name == "Dump")
                .FirstOrDefault();

            var dumpConfiguration = dumpProperty.GetValue(configuration);

            var properties = dumpConfiguration.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "Folder")
                    Assert.IsTrue(property.GetValue(dumpConfiguration).ToString().Contains("logs"), "Folder is invalid");
            }
        }

        [TestMethod]
        public void Read_AppSettings_Configuration_And_Use_Those_Settings()
        {
            var config = GetLibraryAppSettingsConfig();
            var configurationProperty = GetLibraryAppSettingsConfigPropertyInfo();

            var configuration = configurationProperty.GetValue(config);
            var properties = configuration.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == "MaxDepth")
                    Assert.IsTrue(property.GetValue(configuration).ToString() == "64", "Max Depth invalid");

                if (property.Name == "AllowTrailingCommas")
                    Assert.IsTrue(property.GetValue(configuration).ToString() == "True", "AllowTrailingCommas not True");

                if (property.Name == "WriteIndented")
                    Assert.IsTrue(property.GetValue(configuration).ToString() == "True", "WriteIndented not True");
            }
        }

        static PropertyInfo GetLibraryAppSettingsConfigPropertyInfo()
        {
            object config = GetLibraryAppSettingsConfig();

            return config.GetType()
               .GetProperties()
               .Where(x => x.Name == "SystemLibraryCommonNet")
               .FirstOrDefault();
        }

        static object GetLibraryAppSettingsConfig()
        {
            var type = Type.GetType("SystemLibrary.Common.Net.AppSettingsConfig, SystemLibrary.Common.Net");

            var config = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.Name == "Current")
                .FirstOrDefault()
                .GetValue(null);
            return config;
        }
    }
}
