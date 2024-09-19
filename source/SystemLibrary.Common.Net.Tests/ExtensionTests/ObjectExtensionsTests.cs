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
    public void Convert_CamelCase_Props()
    {
        var user = new User();

        user.FirstName = "Hello";
        user.LastName = "World";

        var json = user.Json(true);

        Assert.IsTrue(json.Contains("birth"), "birth");
        Assert.IsTrue(!json.Contains("FirstName"), "First");
        Assert.IsTrue(json.Contains("firstName"), "first");
        Assert.IsTrue(json.Contains("lastName"), "last");
    }

    [TestMethod]
    public void Convert_Class_Does_Not_Print_Null_Values()
    {
        var user = new User();

        var json = user.Json();

        Assert.IsTrue(json.Contains("Birth"));
        Assert.IsTrue(json.Contains("EnumTestProp"));
        Assert.IsTrue(json.Contains("number"));
        Assert.IsTrue(json.Contains("IsEnabled"));
        Assert.IsTrue(!json.Contains("FirstName"));
        Assert.IsTrue(!json.Contains("Owner"));
        Assert.IsTrue(!json.Contains("LastName"));
        Assert.IsTrue(!json.Contains("NullableAgeProperty"));
        Assert.IsTrue(!json.Contains("Names"));
        Assert.IsTrue(!json.Contains("LastNames"));
    }

    [TestMethod]
    public void Convert_Class_Does_Print_Enum_First_Value()
    {
        var user = new User();

        user.FirstName = "Hello world";

        var json = user.Json();

        Assert.IsTrue(json.Contains("FirstName"));
        Assert.IsTrue(!json.Contains("LastName"), "LastName is null, yet part of output");
        Assert.IsTrue(json.Contains("EnumTestProp"));
        Assert.IsTrue(json.Contains(EnumTest.A.ToValue()));
    }

    [TestMethod]
    public void Convert_Class_With_Custom_Date_Time_Converter()
    {
        var date = DateTime.Now;
        var user = new User();
        user.Death = date;
        user.Birth = date.AddDays(1);
        user.CurrentDate = date;

        var json = user.Json(new DateTimeJsonConverter("yyyy-MM-dd"));

        var datetext = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Assert.IsTrue(!json.Contains(datetext), "Custom json converter did not preced built-in");

        Assert.IsTrue(json.Contains(date.Year.ToString()), "Year is missing");

        Assert.IsTrue(json.Contains("CurrentDate\": \"" + date.ToString("yyyy-MM-dd")), "DateJsonConverter did not properly convert to date only");
    }

    [TestMethod]
    public void Convert_Class_With_Enum_Uses_ToValue()
    {
        var user = new User();

        user.EnumTestProp = EnumTest.B;

        var json = user.Json();

        Assert.IsTrue(json.Contains("hello123"));
    }

    [TestMethod]
    public void Convert_User_To_String_Camel_Casing()
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

        var json = user.Json(options);

        Assert.IsTrue(!json.Contains("Age\""), "Age, camelCase");
        Assert.IsTrue(json.Contains("9696"), "9696");
        Assert.IsTrue(json.Contains("firstName"), "firstName, camelCase");
    }

    [TestMethod]
    public void Convert_User_To_String_Not_Camel_Casing()
    {
        User user = new User();
        user.Age = 10;
        user.FirstName = "Hello";

        var json = user.Json();

        Assert.IsTrue(!json.Contains("age"));
        Assert.IsTrue(json.Contains("FirstName"));
    }

    [TestMethod]
    public void Convert_User_With_Enum_As_0_Selected_Outputs_The_Property()
    {
        User user = new User();

        user.EnumTestProp = EnumTest.A;

        var json = user.Json();
        Dump.Write(json);
    }

    [TestMethod]
    public void Convert_User_With_Enum_As_Underscore_Int_Returns_The_Enum_Name_As_Is()
    {
        User user = new User();

        user.EnumTestProp = EnumTest._997;

        var json = user.Json();

        Assert.IsTrue(json.Contains("997"), "997: " + json);

        user.EnumTestProp = EnumTest._999;

        json = user.Json();

        Assert.IsTrue(json.Contains("998"), "999: " + json);
    }

    [TestMethod]
    public void Convert_Json_Int_To_Enum_With_Underscore()
    {
        User user = new User();

        user.EnumTestProp = EnumTest._997;

        var json = user.Json();

        json = json.Replace("997", "990");

        var user2 = json.Json<User>();

        Assert.IsTrue(user2.EnumTestProp.ToValue() == "990", "990");

        json = json.Replace("990", "998");

        var user3 = json.Json<User>();

        Assert.IsTrue(user3.EnumTestProp == EnumTest._999, "Could not convert 998 to 999");

        Assert.IsTrue(user3.EnumTestProp.ToValue() == "998", "It is 998 still, its invalid Enum [or removed key], it shouldve matched _999");
    }

    [TestMethod]
    public void Convert_User_WithNorwegian_Characters_To_String_Not_Camel_Casing()
    {
        User user = new User();
        user.Age = 10;
        user.FirstName = "ÆØÅ æøå";

        var json = user.Json();

        Assert.IsTrue(json.Contains("ÆØÅ æøå"));
        Assert.IsTrue(json.Contains("FirstName"));
    }

    [TestMethod]
    public void Convert_User_To_String_With_Custom_Converter()
    {
        User user = new User();
        user.Age = 10;
        user.FirstName = "Hello";

        var json = user.Json(null);

        Assert.IsTrue(!json.Contains("age"));
        Assert.IsTrue(json.Contains("FirstName"));
    }

    [TestMethod]
    public void Convert_User_To_String_With_Different_Formats()
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

        var json = user.Json();

        Assert.IsTrue(!json.Contains("age"));
        Assert.IsTrue(json.Contains("FirstName"));
        Assert.IsTrue(json.Contains("9696"));
        Assert.IsTrue(json.Contains("Birth"));
        Assert.IsTrue(json.Contains(datetimeNowString));
    }

    [TestMethod]
    public void Convert_String_To_User_With_Different_Formats()
    {
        var data1 = Assemblies.GetEmbeddedResource("_Files", "json-serialization-data.json");
        var data2 = Assemblies.GetEmbeddedResource("_Files", "json-serialization-data-short-format.json");

        var user1 = data1.Json<User>();
        var user2 = data2.Json<User>();

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
