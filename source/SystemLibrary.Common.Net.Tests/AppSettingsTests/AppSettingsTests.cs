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
        public void Read_Deeply_Nested_Configurations()
        {
            var parent = Configs.AppSettingsTests.Current.Parent;
            Assert.IsTrue(parent != null, "Parent is null");
            Assert.IsTrue(parent.Color == "orange", "Color: " + parent.Color);
            Assert.IsTrue(parent.Nested != null, "nested is null");
            Assert.IsTrue(parent.Nested.NestedAgain != null, "nestedagain is null");
            Assert.IsTrue(parent.Nested.NestedAgain.Leaf != null, "leaf is null");
            Assert.IsTrue(parent.Nested.NestedAgain.Leaf.Color == "red", "leaf color");
            Assert.IsTrue(parent.Nested.NestedAgain.Color == "green", "NestedAgain color");
            Assert.IsTrue(parent.Nested.Color == "blue", "Nested color");
        }

        [TestMethod]
        public void Read_UserName_Variable_Customized()
        {
            var conf = Configs.AppSettingsTests.Current;

            Assert.IsTrue(conf != null, "!conf");

            Assert.IsTrue(conf.username?.StartsWith("Hello") == true, "username?.StartsWith1 " + conf.username);

            //Multiple reads to test is does not vary
            Assert.IsTrue(conf.userName?.StartsWith("Hello") == true, "username?.StartsWith2 " + conf.userName);
            Assert.IsTrue(conf.Username?.StartsWith("Hello") == true, "username?.StartsWith3 " + conf.Username);
        }

        [TestMethod]
        public void Read_Dump_Configurations()
        {
            object configuration = GetConfigurationByName("dump");

            var properties = configuration.GetType().GetProperties();

            ValidateProperties(properties, "No properties or null", "folder");

            var count = 0;
            foreach (var property in properties)
            {
                var value = property.GetValue(configuration)?.ToString();
                if (property.Name.ToLower() == "folder")
                {
                    count++;
                    Assert.IsTrue(value.Contains("Logs"), "Folder is invalid: " + property.GetValue(configuration).ToString());
                }

                if (property.Name.ToLower() == "filename")
                {
                    count++;
                    Assert.IsTrue(value.Contains(".log"), "FileName does not contain .log");
                }
            }
            Assert.IsTrue(count == 2, "Too few properties found: " + count + "/3");
        }

        [TestMethod]
        public void Read_Json_Configurations()
        {
            object configuration = GetConfigurationByName("json");

            var properties = configuration.GetType().GetProperties();

            ValidateProperties(properties, "No properties or null", "MaxDepth");

            foreach (var property in properties)
            {
                var value = property.GetValue(configuration)?.ToString();
                if (property.Name == "MaxDepth")
                    Assert.IsTrue(value == "64", "MaxDepth: " + value);

                if (property.Name == "AllowTrailingCommas")
                    Assert.IsTrue(value == "True", "!AllowTrailingCommas");

                if (property.Name == "WriteIndented")
                    Assert.IsTrue(value == "True", "!WriteIndented");

                if (property.Name == "ReadCommentHandling")
                    Assert.IsTrue(value == "Skip", "!ReadCommentHandling");

                if (property.Name == "JsonIgnoreCondition")
                    Assert.IsTrue(value == "WhenWritingNull", "!JsonIgnoreCondition");
            }
        }


        static object GetConfigurationByName(string systemLibraryCommonNetName)
        {
            var config = GetAppSettingsConfig();
            var configurationProperty = GetLibraryAppSettingsConfigPropertyInfo();

            var configuration = configurationProperty.GetValue(config);

            var jsonProperty = configuration.GetType().GetProperties()
                .Where(x => x.Name.ToLower() == systemLibraryCommonNetName.ToLower())
                .FirstOrDefault();

            return jsonProperty.GetValue(configuration);
        }

        static PropertyInfo GetLibraryAppSettingsConfigPropertyInfo()
        {
            object config = GetAppSettingsConfig();

            return config.GetType()
               .GetProperties()
               .Where(x => x.Name == "SystemLibraryCommonNet")
               .FirstOrDefault();
        }

        static object GetAppSettingsConfig()
        {
            var type = Type.GetType("SystemLibrary.Common.Net.AppSettings, SystemLibrary.Common.Net");

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
                throw new Exception(errorMessage + "!No property " + propertyName + ". Contains: " + string.Join(" ", names));
        }
    }
}
