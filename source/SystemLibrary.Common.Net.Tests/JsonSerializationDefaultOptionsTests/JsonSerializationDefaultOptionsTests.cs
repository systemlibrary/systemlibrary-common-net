using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests;

[TestClass]
public class JsonSerializationDefaultOptionsTests
{
    [TestMethod]
    public void Read_Json_With_Default_Options_Success()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        var response = data.Json<Data>();

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

    [TestMethod]
    public void Json_To_Object_With_Custom_Converter()
    {
        var html = "{ \"name\":\"Ferrari\", \"xhtml\":\"<h1>hello</h1>\"}";

        var car = html.Json<Car>(new XhtmlJsonConverter());

        Assert.IsTrue(car != null);
        Assert.IsTrue(car.Name == "Ferrari", "Car name " + car.Name);
        Assert.IsTrue(car.Xhtml.Text.Contains("hello"));
    }

    [TestMethod]
    public void Json_With_NorwegianLetters_Converted_Success()
    {
        var text = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        var data = text.Json<Data>(null, true);

        var norwegianLetters = data.NorwegianLetters;
        var textWithUnicodeCodepoints = data.TextWithUnicodeCodepoints;

        Assert.IsTrue(textWithUnicodeCodepoints == "Strømmen Strømmen", textWithUnicodeCodepoints);
        Assert.IsTrue(norwegianLetters == "æøåÆØÅ", norwegianLetters);
    }

    [TestMethod]
    public void Json_DataWithAllNumberTypes_Converted_Success()
    {
        var text = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "DataWithAllNumberTypes.json");

        var data = text.Json<DataWithAllNumberTypesModels>();

        Assert.IsTrue(data != null);

        Assert.IsTrue(data.inner[0].attributes.id > 1, "First element, id is wrong");
        Assert.IsTrue(data.inner[0].attributes.deci > 1, "First element, deci is wrong");
        Assert.IsTrue(data.inner[0].attributes.name.Is(), "First element, name is wrong");
        Assert.IsTrue(data.inner[0].attributes.anotherId.ToString().EndsWith("2"), "First element, anotherid is wrong");

        Assert.IsTrue(data.inner[1].attributes.id > 285222112, "Second element, id is wrong");
        Assert.IsTrue(data.inner[1].attributes.deci > 285222112, "Second element, deci is wrong");
        Assert.IsTrue(data.inner[1].attributes.name.Is(), "Second element, name is wrong");
        Assert.IsTrue(data.inner[1].attributes.anotherId.ToString().EndsWith("2"), "Second element, anotherid is wrong");

        Assert.IsTrue(data.inner[2].attributes.deci == 123, "Third element, deci is wrong");
        Assert.IsTrue(data.inner[2].attributes.deci == 123.0, "Third element, deci is wrong");
    }
}

public class Car
{
    public string Name { get; set; }
    public Xhtml Xhtml { get; set; }
}

public class Xhtml
{
    public Xhtml(string text = null)
    {
        Text = text;
    }

    public string Text { get; set; }
}

public class XhtmlJsonConverter : JsonConverter<Xhtml>
{
    public override Xhtml Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new Xhtml(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, Xhtml value, JsonSerializerOptions options)
    {
        if (value != null)
            writer.WriteStringValue(value.Text);
    }
}