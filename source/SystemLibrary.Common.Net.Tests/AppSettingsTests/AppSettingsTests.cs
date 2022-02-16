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
            var type = Type.GetType("SystemLibrary.Common.Net.AppSettingsConfig, SystemLibrary.Common.Net");

            var config = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.Name == "Current").FirstOrDefault()
                .GetValue(null);

            var configurationProperty = config.GetType()
                .GetProperties()
                .Where(x => x.Name == "SystemLibraryCommonNet")
                .FirstOrDefault();

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
    }
}
