using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void Convert_User_To_String_CamelCasing()
        {
            User user = new User();
            user.Age = 10;
            user.FirstName = "Hello";

            var json = user.ToJson();

            Assert.IsTrue(!json.Contains("Age"));
            Assert.IsTrue(json.Contains("firstName"));
        }

        [TestMethod]
        public void Convert_User_To_String_NotCamelCasing()
        {
            var options = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = null
            };

            User user = new User();
            user.Age = 10;
            user.FirstName = "Hello";

            var json = user.ToJson(options);

            Assert.IsTrue(!json.Contains("age"));
            Assert.IsTrue(json.Contains("FirstName"));
        }
    }
}
