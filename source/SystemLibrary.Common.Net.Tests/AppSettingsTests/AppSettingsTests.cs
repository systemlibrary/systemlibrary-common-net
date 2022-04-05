using System;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.AppSettingsTests
{
    [TestClass]
    public class AppSettingsTests
    {
        [TestMethod]
        public void Read_Dump_Configurations()
        {
            object configuration = GetConfigurationByName("dump");

            var properties = configuration.GetType().GetProperties();

            ValidateProperties(properties, "Dump contains no properties or is null", "folder");

            foreach (var property in properties)
            {
                var value = property.GetValue(configuration)?.ToString();
                if (property.Name.ToLower() == "folder")
                    Assert.IsTrue(value.Contains("Logs"), "Folder is invalid: " + property.GetValue(configuration).ToString());

                if (property.Name.ToLower() == "filename")
                    Assert.IsTrue(value.Contains(".html"), "FileName does not contain html");
            }
        }

        [TestMethod]
        public void Read_Json_Configurations()
        {
            object configuration = GetConfigurationByName("json");

            var properties = configuration.GetType().GetProperties();

            ValidateProperties(properties, "Json contains no properties or is null", "MaxDepth");

            foreach (var property in properties)
            {
                var value = property.GetValue(configuration)?.ToString();
                if (property.Name == "MaxDepth")
                    Assert.IsTrue(value == "64", "Max Depth invalid: " + value);

                if (property.Name == "AllowTrailingCommas")
                    Assert.IsTrue(value == "True", "AllowTrailingCommas not True");

                if (property.Name == "WriteIndented")
                    Assert.IsTrue(value == "True", "WriteIndented not True");

                if (property.Name == "ReadCommentHandling")
                    Assert.IsTrue(value == "Allow", "ReadCommentHandling is not Allow: " + value);
            }
        }

        static object GetConfigurationByName(string systemLibraryCommonNetName)
        {
            var config = GetLibraryAppSettingsConfig();
            var configurationProperty = GetLibraryAppSettingsConfigPropertyInfo();

            var configuration = configurationProperty.GetValue(config);

            var jsonProperty = configuration.GetType().GetProperties()
                .Where(x => x.Name.ToLower() == systemLibraryCommonNetName.ToLower())
                .FirstOrDefault();

            return jsonProperty.GetValue(configuration);
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

        static void ValidateProperties(PropertyInfo[] properties, string errorMessage, string propertyName)
        {
            if (properties == null || properties.Length == 0)
                throw new Exception(errorMessage);

            var names = properties.Select(x => x.Name.ToLower()).ToArray();

            if (!names.Has(propertyName.ToLower()))
                throw new Exception(errorMessage + " Does not contain property: " + propertyName + ". Contains: " + string.Join(" ", names));
        }
    }
}
