using System;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;
using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class ObjectExtensionsTests
{
    [TestMethod]
    public void Convert_User_To_String_CamelCasing()
    {
        User user = new User();
        user.Age = 10;
        user.FirstName = "Hello";
        user.Birth = DateTime.Now;
        user.Married = DateTimeOffset.Now;
        user.Money = 9696;

        var options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = user.ToJson(options);

        Assert.IsTrue(!json.Contains("Age"));
        Assert.IsTrue(json.Contains("firstName"));
    }

    [TestMethod]
    public void Convert_User_To_String_NotCamelCasing()
    {
        User user = new User();
        user.Age = 10;
        user.FirstName = "Hello";

        var json = user.ToJson();

        Assert.IsTrue(!json.Contains("age"));
        Assert.IsTrue(json.Contains("FirstName"));
    }

    [TestMethod]
    public void Convert_User_To_String_WithDifferentFormats()
    {
        User user = new User();
        user.Age = 10;
        user.FirstName = "Hello";
        user.Birth = DateTime.Now;
        user.Married = DateTimeOffset.Now.AddYears(10);
        user.Money = 9696;

        var datetimeNowString = DateTime.Now.Year + "-" +
            (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month) + "-" +
            (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day);

        var json = user.ToJson();

        Assert.IsTrue(!json.Contains("age"));
        Assert.IsTrue(json.Contains("FirstName"));
        Assert.IsTrue(json.Contains("9696"));
        Assert.IsTrue(json.Contains("Birth"));
        Assert.IsTrue(json.Contains(datetimeNowString));
    }

    [TestMethod]
    public void Convert_String_To_User_WithDifferentFormats()
    {
        var data1 = Assemblies.GetEmbeddedResource("_Files", "json-serialization-data.json");
        var data2 = Assemblies.GetEmbeddedResource("_Files", "json-serialization-data-short-format.json");

        var user1 = data1.ToJson<User>();
        var user2 = data2.ToJson<User>();

        Assert.IsTrue(user1 != null);
        Assert.IsTrue(user2 != null);

        Assert.IsTrue(user1.Birth > DateTime.MinValue, "Birth 1");
        Assert.IsTrue(user2.Birth > DateTime.MinValue, "Birth 2");

        Assert.IsTrue(user1.Married > DateTimeOffset.MinValue, "Married 1");
        Assert.IsTrue(user2.Married > DateTimeOffset.MinValue, "Married 2");
        Assert.IsTrue(user2.Married.Year > DateTime.Now.Year, "Married Year");

        Assert.IsTrue(user1.Death > DateTime.MinValue, "Death 1");
        Assert.IsTrue(user1.Death.Year < DateTime.Now.Year, "Death 1 Year");
        Assert.IsTrue(user2.Death == DateTime.MinValue, "Death 2");

        Assert.IsTrue(user1.Money > 100);
        Assert.IsTrue(user2.Money == 0);

        Assert.IsTrue(user2.Age == 0);
    }
}
