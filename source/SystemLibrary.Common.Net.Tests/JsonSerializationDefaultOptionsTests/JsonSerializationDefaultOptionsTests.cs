using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests;

[TestClass]
public class JsonSerializationDefaultOptionsTests
{
    [TestMethod]
    public void Read_Json_With_Default_Options_Success()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        var response = data.ToJson<Data>();

        Assert.IsNotNull(response);

        Assert.IsTrue(response.IsSuccess, "False");

        Assert.IsTrue(response.SubClass != null, "Subclass is null");
        Assert.IsTrue(response.SubClass.CarEnumPropertyAsNull == Product.Car1, "Subclass car1");
        Assert.IsTrue(response.SubClass.CarEnumAsNumber == Product.Car3, "Subclass Car3 number");
        Assert.IsTrue(response.SubClass.CarEnumAsText == Product.Car3, "Subclass car3");

        Assert.IsTrue(response.IntAsStringProperty.Length > 4, "Int is not converted to string");
        Assert.IsTrue(response.StringProperty == "stringProp", "StringProperty is not converted to string");
        Assert.IsTrue(response.CarEnumAsText == Product.Car3, "Car3");
        Assert.IsTrue(response.CarEnumAsNumber == Product.Car3, "Car3");
        Assert.IsTrue(response.ListOfTextEnums.Count > 0, "List is null");
        Assert.IsTrue(response.ListOfTextEnums[1] == Product.Car4, "List car4");
    }
}
