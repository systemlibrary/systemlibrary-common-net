using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests;

[TestClass]
public class JsonSerializationDefaultOptionsTests
{
    [TestMethod]
    public void Dump_Dummy_Numbers_Obfuscated()
    {
        //Dump.Write(12345.ToString().Obfuscate(77).ToBase64());
        //Dump.Write("0123457890abcdefGHI!?".Obfuscate(77).ToBase64());
        //Dump.Write("0123457890abcdefGHI!?".Obfuscate(77).ToBase64().FromBase64().Deobfuscate(77));
    }

    [TestMethod]
    public void Read_Json_With_Obfuscated_Variables_Decrypts_Success()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "DataWithObfuscateVars.json");

        var d = data.Json<DataWithObfuscateVars>();

        Assert.IsTrue(d.Id == 777, "id1");
        Assert.IsTrue(d.Id2 == 777, "id2");
        Assert.IsTrue(d.ID3 == 12345, "id3");
        Assert.IsTrue(d.id4 == 12345, "id4 " + d.id4);
        Assert.IsTrue(d.id5 == 12345, "id5");
        Assert.IsTrue(d.id6 == 0, "id6");
        Assert.IsTrue(d.id7 == "0123457890abcdefGHI!?", "id7");
        Assert.IsTrue(d.ID8 == "12345", "id8");
        Assert.IsTrue(d.Id9 == null, "id9 " + d.Id9);
        Assert.IsTrue(d.id11 == 12345, "id10 " + d.id11);

        var compressedRulesJson = d.Json();

        Assert.IsTrue(compressedRulesJson.Contains("\"Id\": 777,"), "json: id");
        Assert.IsTrue(compressedRulesJson.Contains("\"id4\": \"fn/CgMKBwoI=\""), "json: id4 " + d.id4 + " " + d.id4.ToString().Obfuscate(77));
        Assert.IsTrue(compressedRulesJson.Contains("\"ID8\": \"fn/CgMKBwoI=\""), "json: id8");
        Assert.IsTrue(compressedRulesJson.Contains("\"id11\": \"fn/CgMKBwoI=\""), "json: id11");
    }

    [TestMethod]
    public void Dump_Dummy_Numbers_Encrypted()
    {
        //Dump.Write(0.ToString().Encrypt());
        //Dump.Write(123.ToString().Encrypt());
        //Dump.Write(4560000.ToString().Encrypt());
        //Dump.Write("-678".Encrypt());
        //Dump.Write(12345678901234567890.ToString().Encrypt());
    }

    [TestMethod]
    public void Read_Json_With_Compressed_Variables_Decompresses_Success()
    {
        var expected = new DataWithCompressedVars();
        expected.Id = 1;
        expected.Id2 = 777;
        expected.ID3 = 3;
        expected.id4 = 4;
        expected.id7 = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBB?AAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBB?AAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBB?AAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBB?AAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBB?AAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBB?AAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBB?";
        expected.id8 = "abcdefghijklmnopqrstuvwXYZ0123?!__.:.__!";
        expected.id9 = 123456789123456;

        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "DataWithCompressedVars.json");

        var d = data.Json<DataWithCompressedVars>();

        Assert.IsTrue(d.Id == expected.Id, "id");
        Assert.IsTrue(d.Id2 == expected.Id2, "id2 " + d.Id2 + " " + expected.Id2);
        Assert.IsTrue(d.ID3 == expected.ID3, "id3");
        Assert.IsTrue(d.id4 == expected.id4, "id4");
        Assert.IsTrue(d.id7 == expected.id7, "id7");
        Assert.IsTrue(d.id8 == expected.id8, "id8");
        Assert.IsTrue(d.id9 == expected.id9, "id9");

        Assert.IsTrue(d.id9 == 123456789123456);

        var json = d.Json();

        Assert.IsTrue(!json.Contains("123456789123456"), "Invalid, the long exists in the string json");
    }

    [TestMethod]
    public void Read_Json_With_Encrypted_Variables_Decrypts_Success()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "DataWithEncryptedVars.json");

        var d = data.Json<DataWithEncryptedVars>();

        Assert.IsTrue(d.Id == 0, "id");
        Assert.IsTrue(d.Id2 == 777, "id2");
        Assert.IsTrue(d.ID3 != 777, "id3");
        Assert.IsTrue(d.id4 != 777, "id4");
        Assert.IsTrue(d.id5 != 777, "id5");
        Assert.IsTrue(d.id6 == 777, "id6");
        Assert.IsTrue(d.id7 == "-678", "id7 " + d.id7);
        Assert.IsTrue(d.id8 == "12345678901234567890", "id8");
        Assert.IsTrue(d.id9 == 123, "id9");

        var jsonEncrypted = d.Json();

        Assert.IsTrue(!jsonEncrypted.Contains("\"Id\": \"0"), "json id");
        Assert.IsTrue(!jsonEncrypted.Contains("\"ID3\": \"123"), "json id3");
        Assert.IsTrue(!jsonEncrypted.Contains("\"id4\": \"4560000"), "json id4");
        Assert.IsTrue(!jsonEncrypted.Contains("\"id5\": \"-678"), "json id5");
        Assert.IsTrue(!jsonEncrypted.Contains("12345678901234567890"), "json id8");
    }

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
        var html = "{ \"name\":\"Red\", \"xhtml\":\"<h1>hello</h1>\"}";

        var car = html.Json<Car>(new XhtmlJsonConverter());

        Assert.IsTrue(car != null);
        Assert.IsTrue(car.Name == "Red", "Car name " + car.Name);
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

    [TestMethod]
    public void Json_With_Various_DateTimeFormats_Success()
    {
        var text = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "DataWithVariousDateTimeFormats.json");

        var data = text.Json<DataWithVariousDateTimeFormatsModels>();

        Assert.IsTrue(data != null, "Null");

        ValidateDateTimeMinutePrecision(data.nor1, nameof(data.nor1));
        ValidateDateTimeMinutePrecision(data.nor2, nameof(data.nor2));
        ValidateDateTimeMinutePrecision(data.nor3, nameof(data.nor3));
        ValidateDateTimeDayPrecision(data.nor4, nameof(data.nor4));

        ValidateDateTimeMinutePrecision(data.nor5, nameof(data.nor5));
        ValidateDateTimeMinutePrecision(data.nor6, nameof(data.nor6));
        ValidateDateTimeMinutePrecision(data.nor7, nameof(data.nor7));
        ValidateDateTimeDayPrecision(data.nor8, nameof(data.nor8));

        ValidateDateTimeMinutePrecision(data.eng1, nameof(data.eng1));
        ValidateDateTimeMinutePrecision(data.eng2, nameof(data.eng2));
        ValidateDateTimeMinutePrecision(data.eng3, nameof(data.eng3));
        ValidateDateTimeDayPrecision(data.eng4, nameof(data.eng4));

        ValidateDateTimeMinutePrecisionWithTimezone(data.timezoneprecision, nameof(data.timezoneprecision));
        ValidateDateTimeMinutePrecisionWithTimezone(data.timezoneprecision2, nameof(data.timezoneprecision2));

        ValidateDateTimeMinutePrecisionUTCZ(data.fullprecision, nameof(data.fullprecision));
        ValidateDateTimeMinutePrecisionUTCZ(data.fullprecision2, nameof(data.fullprecision2));

        ValidateDateTimeMinutePrecisionUTCZ(data.msprecision, nameof(data.msprecision));
        ValidateDateTimeMinutePrecisionUTCZ(data.msprecision2, nameof(data.msprecision2));

        ValidateDateTimeMinutePrecisionUTCZ(data.secprecision, nameof(data.secprecision));
        ValidateDateTimeMinutePrecisionUTCZ(data.secprecision2, nameof(data.secprecision2));

        ValidateDateTimeMinutePrecisionUTCZ(data.noprecision, nameof(data.noprecision));
        ValidateDateTimeMinutePrecisionUTCZ(data.noprecision2, nameof(data.noprecision2));

        ValidateDateTimeDayPrecision(data.common1, nameof(data.common1));
        ValidateDateTimeDayPrecision(data.common2, nameof(data.common2));
        ValidateDateTimeDayPrecision(data.common3, nameof(data.common3));

        ValidateDateTimeMinutePrecisionUTCZ(data.rfc1123, nameof(data.rfc1123));
        ValidateDateTimeMinutePrecisionWithTimezone(data.rfc1123zzz, nameof(data.rfc1123zzz));
        ValidateDateTimeMinutePrecisionWithTimezone(data.rfc1123timezone, nameof(data.rfc1123timezone));

        ValidateDateTimeMinutePrecisionUTCZ(data.rfc3339mstimezone, nameof(data.rfc3339mstimezone));
        ValidateDateTimeMinutePrecisionUTCZ(data.rfc3339fullmstimezone, nameof(data.rfc3339fullmstimezone));
        ValidateDateTimeMinutePrecisionUTCZ(data.rfc3339timezonewithoutseconds, nameof(data.rfc3339timezonewithoutseconds));

        ValidateDateTimeDayPrecision(data.basic1, nameof(data.basic1));
        ValidateDateTimeMinutePrecision(data.basic2, nameof(data.basic2));
        ValidateDateTimeMinutePrecisionUTCZ(data.basic3, nameof(data.basic3));
        ValidateDateTimeMinutePrecisionWithTimezone(data.basic4, nameof(data.basic4));

        ValidateDateTimeMinutePrecisionWithTimezone(data.iso8601timezone, nameof(data.iso8601timezone));
        ValidateDateTimeMinutePrecision(data.usampm, nameof(data.usampm));
        ValidateDateTimeMinutePrecision(data.europeanweekday, nameof(data.europeanweekday));
        ValidateDateTimeMinutePrecision(data.unixtimestamp, nameof(data.unixtimestamp));

        ValidateDateTimeMonthPrecision(data.dateString1, nameof(data.dateString1));
        ValidateDateTimeMonthPrecision(data.dateString2, nameof(data.dateString2));
        ValidateDateTimeMonthPrecision(data.dateString3, nameof(data.dateString3));
        ValidateDateTimeMonthPrecision(data.dateString4, nameof(data.dateString4));
        ValidateDateTimeMonthPrecision(data.dateString5, nameof(data.dateString5));
    }

    static void ValidateDateTimeMonthPrecision(DateTime d, string msg)
    {
        Assert.IsTrue(d.Day == 24 && d.Month == 12 && d.Year == 2001, "Error: " + msg + ": " + d.ToString("yyyy-MM-dd HH:mm"));
    }

    static void ValidateDateTimeMinutePrecisionUTCZ(DateTime d, string msg)
    {
        Assert.IsTrue(d.Month == 12 && d.Day == 25 && d.Year == 2001 && d.Hour == 00 && d.Minute == 22, "Error: " + msg + ": " + d.ToString("yyyy-MM-dd HH:mm"));
    }

    static void ValidateDateTimeMinutePrecisionWithTimezone(DateTime d, string msg)
    {
        Assert.IsTrue(d.Month == 12 && d.Day == 24 && d.Year == 2001 && d.Hour == 20 && d.Minute == 22, "Error: " + msg + ": " + d.ToString("yyyy-MM-dd HH:mm"));
    }

    static void ValidateDateTimeMinutePrecision(DateTime d, string msg)
    {
        Assert.IsTrue(d.Month == 12 && d.Day == 24 && d.Year == 2001 && d.Hour == 23 && d.Minute == 22, "Error: " + msg + ": " + d.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    static void ValidateDateTimeDayPrecision(DateTime d, string msg)
    {
        Assert.IsTrue(d.Year == 2001 && d.Day == 24 && d.Month == 12, "Error: " + msg + ": " + d.ToString());
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