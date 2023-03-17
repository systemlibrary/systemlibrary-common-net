using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;
using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void String_Is()
    {
        string data = null;
        var result = false;

        result = data.Is();
        Assert.IsTrue(result == false, "Is null");

        data = "";
        result = data.Is();
        Assert.IsTrue(result == false, "Is empty");

        data = " ";
        result = data.Is();
        Assert.IsTrue(result == false, "Is space");

        data = "  ";
        result = data.Is();
        Assert.IsTrue(result, "Is 2 spaces");

        data = "A";
        result = data.Is();
        Assert.IsTrue(result, "A");

        data = ".";
        result = data.Is();
        Assert.IsTrue(result, "A");
    }

    [TestMethod]
    public void String_Is_Not()
    {
        string data = null;
        var result = false;

        result = data.IsNot();
        Assert.IsTrue(result, "Result not null");

        data = "";
        result = data.IsNot();
        Assert.IsTrue(result, "Result not empty");

        data = " ";
        result = data.IsNot();
        Assert.IsTrue(result, "Result not space");

        data = "  ";
        result = data.IsNot();
        Assert.IsTrue(result == false, "Result not 2 spaces");

        data = "Hello world";
        result = data.IsNot("Hello", "Hello woRld", "Hello world");
        Assert.IsTrue(result, "Result not 'Hello world'");

        data = "abcdHello world12345";
        result = data.IsNot("Hello", "Hello woRld", "Hello world");
        Assert.IsTrue(result == false, "Result not 'Hello world'");
    }

    [TestMethod]
    public void String_To_Md5_Hash_String()
    {
        string data = null;
        string result = null;

        result = data.ToMD5Hash();
        Assert.IsTrue(result == null);

        data = "";
        result = data.ToMD5Hash();
        Assert.IsTrue(result == "");

        data = "Hello world";
        result = data.ToMD5Hash();
        Assert.IsTrue(result.Length == 47, "length");

        var data2 = "Hello world";
        var result2 = data2.ToMD5Hash();
        Assert.IsTrue(result == result2, "Md5 Not equal second time");
    }

    [TestMethod]
    public void String_To_Sha1_Hash_String()
    {
        string data = null;
        string result = null;

        result = data.ToSha1Hash();
        Assert.IsTrue(result == null);

        data = "";
        result = data.ToSha1Hash();
        Assert.IsTrue(result == "");

        data = "Hello world";

        result = data.ToSha1Hash();
        Assert.IsTrue(result.Length == 59, "Sha1 length: " + result.Length);

        var data2 = "Hello world";
        var result2 = data2.ToSha1Hash();
        Assert.IsTrue(result == result2, "Sha1 Not equal second time");
    }

    [TestMethod]
    public void String_To_Sha256_Hash_String()
    {
        string data = null;
        string result = null;

        result = data.ToSha256Hash();
        Assert.IsTrue(result == null);

        data = "";
        result = data.ToSha256Hash();
        Assert.IsTrue(result == "");

        data = "Hello world";

        result = data.ToSha256Hash();
        Assert.IsTrue(result.Length == 95, "Sha256 length: " + result.Length);

        var data2 = "Hello world";
        var result2 = data2.ToSha256Hash();
        Assert.IsTrue(result == result2, "Sha256 Not equal second time");
    }

    [TestMethod]
    public void Obfuscate_String_And_Deobfuscate()
    {
        string data = null;
        string result;

        result = data.Obfuscate();
        Assert.IsTrue(result == null);
        result = result.Deobfuscate();
        Assert.IsTrue(result == data, "null");

        data = "";
        result = data.Obfuscate();
        Assert.IsTrue(result == "");
        result = result.Deobfuscate();
        Assert.IsTrue(result == data, "blank");

        data = "Hello World!";
        result = data.Obfuscate();
        Assert.IsTrue(result.Length == data.Length);
        Assert.IsTrue(result != data);
        result = result.Deobfuscate();
        Assert.IsTrue(result == data, "hello world");

        data = "A lot of various characters ÆØÅæøå ABCDEFGHIJKLMONPQRSTUVXYZ: 1234567890,.-_?\"*|!#¤%&/()=?…±òü±²³";
        result = data.Obfuscate();
        Assert.IsTrue(result.Length == data.Length);
        Assert.IsTrue(result != data);
        result = result.Deobfuscate();
        Assert.IsTrue(result == data, "long text");

        data = "High Salt A lot of various with high salt ÆØÅæøå ABCDEFGHIJKLMONPQRSTUVXYZ: 1234567890,.-_?\"*|!#¤%&/()=?…±òü±²³";
        result = data.Obfuscate(10555);
        Assert.IsTrue(result.Length == data.Length);
        Assert.IsTrue(result != data);
        result = result.Deobfuscate(10555);
        Assert.IsTrue(result == data, "high salt");

        result = data.Obfuscate(10000);
        var result2 = data.Obfuscate(1);
        var result3 = data.Obfuscate(100);
        var result4 = data.Obfuscate(1000);

        Assert.IsTrue(result != result2 && result.Length == result2.Length, "Result 2");
        Assert.IsTrue(result != result3 && result.Length == result3.Length, "Result 3");
        Assert.IsTrue(result != result4 && result.Length == result4.Length, "Result 4");

        try
        {
            data = "hello";
            result = data.Obfuscate(-1);
            Assert.IsTrue(false, "Did not throw exception");
        }
        catch
        {
        }

        result = data.Obfuscate(char.MaxValue + 55);
        result2 = data.Obfuscate(55);
        Assert.IsTrue(result != result2, "MaxValue + 55 is equal 55");

        result = result.Deobfuscate(char.MaxValue + 55);
        Assert.IsTrue(data == result, "MaxValue salt");
    }

    [TestMethod]
    public void Convert_String_ToBase64()
    {
        string data = null;
        string result;

        result = data.ToBase64();
        Assert.IsTrue(result == null);
        result = result.FromBase64();
        Assert.IsTrue(result == data);

        data = "";
        result = data.ToBase64();
        Assert.IsTrue(result == "");
        result = result.FromBase64();
        Assert.IsTrue(result == data);

        data = "hello world";
        result = data.ToBase64();
        Assert.IsTrue(result.Length > 10 && result.EndsWith("="));
        result = result.FromBase64();
        Assert.IsTrue(result == data);

        data = "A lot of various characters ÆØÅæøå ABCDEFGHIJKLMONPQRSTUVXYZ: 1234567890,.-_?\"*|!#¤%&/()=?…±òü±²³";
        result = data.ToBase64();
        Assert.IsTrue(result != null);
        result = result.FromBase64();
        Assert.IsTrue(result == data && result.Length == data.Length);
    }

    [TestMethod]
    public void Convert_String_To_User()
    {
        var data = "{ \"firstName\": \"Hello\", \"age\": 10 }";

        var user = data.Json<User>();

        Assert.IsTrue(user.Age == 10 && user.FirstName == "Hello");
    }

    [TestMethod]
    public void Convert_String_To_User_Fails_For_Age_CaseSensitive()
    {
        var data = "{ \"FirstName\": \"Hello\", \"age\": 10 }";

        var options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = false
        };
        var user = data.Json<User>(options);

        Assert.IsTrue(user != null);
        Assert.IsTrue(user.Age == 0);
        Assert.IsTrue(user.FirstName == "Hello");
    }

    [TestMethod]
    public void Enum_To_Value()
    {
        EnumTest a = EnumTest.A;
        EnumTest b = EnumTest.B;
        EnumTest c = EnumTest.C;

        Assert.AreEqual(a.ToValue(), "A", "A");
        Assert.AreEqual(b.ToValue(), "hello123", "hello123");
        Assert.AreEqual(c.ToValue(), "100", "100");
    }

    [TestMethod]
    public void Enum_To_Object_Value()
    {
        EnumTest a = EnumTest.A;
        EnumTest b = EnumTest.B;
        EnumTest c = EnumTest.C;

        Assert.AreEqual(a.GetEnumValue(),null, "A");
        Assert.AreEqual(b.GetEnumValue(), "hello123", "hello123");
        Assert.AreEqual(c.GetEnumValue(), 100, "100");
    }

    [TestMethod]
    public void Enum_To_Text_Value()
    {
        EnumTest a = EnumTest.A;
        EnumTest b = EnumTest.B;
        EnumTest c = EnumTest.C;

        Assert.AreEqual(a.GetEnumText(), null, "A is not null");
        Assert.AreEqual(b.GetEnumText(), "HELLO", "HELLO");
        Assert.AreEqual(c.GetEnumText(), "Hello World", "Hello World");
    }


    [TestMethod]
    public void Enum_To_Text()
    {
        EnumTest a = EnumTest.A;
        EnumTest b = EnumTest.B;
        EnumTest c = EnumTest.C;

        Assert.AreEqual(a.ToText(), "A");
        Assert.AreEqual(b.ToText(), "HELLO");
        Assert.AreEqual(c.ToText(), "Hello World");
    }

    [TestMethod]
    public void String_To_Enum()
    {
        string a0 = null;
        string a00 = "";
        Assert.AreEqual("A", a0.ToEnum<EnumTest>().ToString(), "a0");
        Assert.AreEqual("A", a00.ToEnum<EnumTest>().ToString(), "a00");

        string a1 = "a";
        string a2 = "A";
        string a3 = "";
        Assert.AreEqual("A", a1.ToEnum<EnumTest>().ToString(), a1);
        Assert.AreEqual("A", a2.ToEnum<EnumTest>().ToString(), a2);
        Assert.AreEqual("A", a3.ToEnum<EnumTest>().ToString(), a3);

        string b1 = "b";
        string b2 = "B";
        string b3 = "HELLO";
        string b4 = "hello123";
        Assert.AreEqual("B", b1.ToEnum<EnumTest>().ToString(), b1);
        Assert.AreEqual("B", b2.ToEnum<EnumTest>().ToString(), b2);
        Assert.AreEqual("B", b3.ToEnum<EnumTest>().ToString(), b3);
        Assert.AreEqual("B", b4.ToEnum<EnumTest>().ToString(), b4);

        int c1 = 100;
        string c2 = "100";
        Assert.AreEqual("C", c1.ToString().ToEnum<EnumTest>().ToString(), "100 int");
        Assert.AreEqual("C", c2.ToEnum<EnumTest>().ToString(), "100 str");
    }

    [TestMethod]
    public void Max_Length()
    {
        var data = (string)null;
        var res = data.MaxLength(3);

        Assert.IsTrue(res == "", "Res it not null when passing in null");

        data = "";
        res = data.MaxLength(3);
        Assert.IsTrue(res == "", "Res it not blank when passing in blank");

        data = "1";
        res = data.MaxLength(3);
        Assert.IsTrue(res == "1", "Res is not 1 when passing in '1'");

        data = "123";
        res = data.MaxLength(3);
        Assert.IsTrue(res == "123", "Res is not 123 when passing in '123'");

        data = "1234";
        res = data.MaxLength(3);
        Assert.IsTrue(res == "123", "Res is not 123 when passing in '1234'");

        data = "1234 hello world, long text @.,.-!'æ¨¨æ¨ææ¨%#&=)#&#!(=!/?=/(?=";
        res = data.MaxLength(3);
        Assert.IsTrue(res == "123", "Res is not 123 when passing in a long text with many special chars");

        data = "1234 hello world, long text @.,.-!'æ¨¨æ¨ææ¨%#&=)#&#!(=!/?=/(?=";
        res = data.MaxLength(-1);
        Assert.IsTrue(res == "", "Res is not blank when passing in a long text with many special chars, but max length negative");
    }


    [TestMethod]
    public void Is_Any()
    {
        var data = "world";

        var any = new[] { "hello", "world" };

        Assert.IsTrue(data.IsAny(any));
    }

    [TestMethod]
    public void Contains_Any()
    {
        var data = "Hello world, hello world";

        var any = new[] { "abc", "def", "hello" };

        Assert.IsTrue(data.ContainsAny(any));
    }

    [TestMethod]
    public void Ends_With_Any()
    {
        var data = "Hello world";

        var any = new[] { "", "data", "world" };

        Assert.IsTrue(data.EndsWithAny(any));
    }

    [TestMethod]
    public void Ends_With_Any_Character()
    {
        var data = "Hello world";
        var any = "abcdef";

        Assert.IsTrue(data.EndsWithAnyCharacter(any));
    }

    [TestMethod]
    public void Ends_With_Any_Case_Insensitive()
    {
        var data = "hello WorLd123!aA";
        var result = data.EndsWithAnyCaseInsensitive("helloworld", "something", "another one", "whatever", "world123!aa");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Starts_With_Any()
    {
        var data = "Hello world 123456";
        var result = false;

        result = data.StartsWithAny("world", "1234", "abc", "ello", "Hello");
        Assert.IsTrue(result, "Does not start with any");

        result = data.StartsWithAny(null);
        Assert.IsTrue(result == false, "Values is null");

        result = data.StartsWithAny("");
        Assert.IsTrue(result == false, "Values is empty");

        result = data.StartsWithAny("H");
        Assert.IsTrue(result, "H");

    }

    [TestMethod]
    public void Trim_End()
    {
        var data = "/";

        Assert.IsTrue(!data.TrimEnd("/").Contains("/"));
        Assert.IsTrue(!data.TrimEnd("\\").Contains("\\"));
    }

    [TestMethod]
    public void Test_Get_Primary_Domain()
    {
        Assert.IsTrue(((string)null).GetPrimaryDomain() == "");
        Assert.IsTrue(" ".GetPrimaryDomain() == "");
        Assert.IsTrue("".GetPrimaryDomain() == "");
        Assert.IsTrue("a".GetPrimaryDomain() == "a.com");
        Assert.IsTrue("abc".GetPrimaryDomain() == "abc.com");
        Assert.IsTrue("abc.com".GetPrimaryDomain() == "abc.com");

        Assert.IsTrue("hello.world".GetPrimaryDomain() == "world.com", "hello.world");
        Assert.IsTrue("hello.world.n".GetPrimaryDomain() == "world.n", "n");

        Assert.IsTrue("hello.world.no".GetPrimaryDomain() == "world.no", ".no");
        Assert.IsTrue("hello.world.com".GetPrimaryDomain() == "world.com", ".com");
        Assert.IsTrue("hello.world.web".GetPrimaryDomain() == "world.web", ".web");
        Assert.IsTrue("hello.world.config".GetPrimaryDomain() == "config.com", "config");

        Assert.IsTrue("http://hello.world.n".GetPrimaryDomain() == "world.n");
        Assert.IsTrue("http://hello.world.no".GetPrimaryDomain() == "world.no");
        Assert.IsTrue("http://hello.world.com".GetPrimaryDomain() == "world.com");
        Assert.IsTrue("http://hello.world.web".GetPrimaryDomain() == "world.web");
        Assert.IsTrue("http://hello.world.config".GetPrimaryDomain() == "config.com");


        Assert.IsTrue("https://hello.world.n".GetPrimaryDomain() == "world.n");
        Assert.IsTrue("https://hello.world.no".GetPrimaryDomain() == "world.no");
        Assert.IsTrue("https://hello.world.com".GetPrimaryDomain() == "world.com");
        Assert.IsTrue("https://hello.world.web".GetPrimaryDomain() == "world.web");
        Assert.IsTrue("https://hello.world.config".GetPrimaryDomain() == "config.com");

        Assert.IsTrue("https://www.hello.world.n".GetPrimaryDomain() == "world.n");
        Assert.IsTrue("https://www.hello.world.no".GetPrimaryDomain() == "world.no");
        Assert.IsTrue("https://www.hello.world.com".GetPrimaryDomain() == "world.com");
        Assert.IsTrue("https://www.hello.world.web".GetPrimaryDomain() == "world.web");
        Assert.IsTrue("https://www.hello.world.config".GetPrimaryDomain() == "config.com");
    }

    [TestMethod]
    public void Hex_Darken_Or_Lighten_Tests()
    {
        var value = "";
        var expected = "";
        var result = "";
        result = value.HexDarkenOrLighten();
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#12";
        expected = "";
        try
        {
            result = value.HexDarkenOrLighten();

            Assert.IsTrue(false, "Exception was not thrown");
        }
        catch
        {
            Assert.IsTrue(true);
        }

        value = "#000000";
        expected = "#FFFFFF";
        result = value.HexDarkenOrLighten(auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#FFFFFF";
        expected = "#4F4F4F";
        result = value.HexDarkenOrLighten(auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#414141";
        expected = "#EBEBEB";
        result = value.HexDarkenOrLighten(auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#414141";
        expected = "#141414";
        result = value.HexDarkenOrLighten(auto: false);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#414141";
        expected = "#555555";
        result = value.HexDarkenOrLighten(factor: -0.31, auto: false);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#AAAAAA";
        expected = "#DFDFDF";
        result = value.HexDarkenOrLighten(factor: -0.31, auto: false);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#DFDFDF";
        expected = "#252525";
        result = value.HexDarkenOrLighten(factor: -0.31, auto: false);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#66FF00";
        expected = "#864F00";
        result = value.HexDarkenOrLighten(factor: -0.31, auto: false);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#8f8f33";
        expected = "#CACA48";
        result = value.HexDarkenOrLighten(factor: -0.41, auto: false);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#FFFFFF";
        expected = "#030303";
        result = value.HexDarkenOrLighten(factor: 0.99, auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#8f8f33";
        expected = "#0101CD";
        result = value.HexDarkenOrLighten(factor: 0.99, auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#030303";
        expected = "#FCFCFC";
        result = value.HexDarkenOrLighten(factor: 0.99, auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#030303";
        expected = "#FFFFFF";
        result = value.HexDarkenOrLighten(factor: 0.01, auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);

        value = "#FFFFFF";
        expected = "#4F4F4F";
        result = value.HexDarkenOrLighten(factor: 0.31, auto: true);
        Assert.IsTrue(result == expected, "Result was " + result + " when expecting " + expected);
    }

    [TestMethod]
    public void Get_Bytes_Of_Text()
    {
        string text = null;
        byte[] bytes = null;

        bytes = text.GetBytes();

        Assert.IsTrue(bytes == null);

        text = "";
        bytes = text.GetBytes();
        Assert.IsTrue(bytes != null);
        Assert.IsTrue(bytes.Length == 0);

        text = "Hello world";
        bytes = text.GetBytes();

        Assert.IsTrue(text.Length == bytes.Length);
    }

    [TestMethod]
    public void Get_Uri_Encoded_Text()
    {
        var plain = "Hello world + ?";

        var coded = plain.UriEncode();

        Assert.IsTrue(coded == "Hello%20world%20%2B%20%3F");

        plain = null;
        coded = plain.UriEncode();
        Assert.IsTrue(coded == null);
    }

    [TestMethod]
    public void Get_Uri_Decoded_Text()
    {
        var coded = "Hello%20world%20%2B%20%3F";

        var plain = coded.UriDecode();

        Assert.IsTrue(plain == "Hello world + ?", plain);

        plain = null;
        coded = plain.UriDecode();
        Assert.IsTrue(coded == null);
    }

    [TestMethod]
    public void ToPascalCase()
    {
        string text = null;
        string result = text.ToPascalCase();
        Assert.IsTrue(result == null);

        text = "";
        result = text.ToPascalCase();
        Assert.IsTrue(result == "");

        text = "1h";
        result = text.ToPascalCase();
        Assert.IsTrue(result == "1h");

        text = "a";
        result = text.ToPascalCase();
        Assert.IsTrue(result == "A");

        text = "HEllo world 1234this is nICE";
        result = text.ToPascalCase();
        Assert.IsTrue(result == "Hello World 1234this Is Nice", result);

        text = "HELLO WORLD@EMAIL.COM. This is it. this is just a sample? or is it?";
        result = text.ToPascalCase();
        Assert.IsTrue(result == "Hello World@email.com. This Is It. This Is Just A Sample? Or Is It?", result);

        text = "Hello-world-this WAS-NICE";
        result = text.ToPascalCase();
        Assert.IsTrue(result == "Hello-World-This Was-Nice");
    }

    [TestMethod]
    public void To_Camel_Case()
    {
        string text = null;
        string result = text.toCamelCase();
        Assert.IsTrue(result == null);

        text = "";
        result = text.toCamelCase();
        Assert.IsTrue(result == "");

        text = "1h";
        result = text.toCamelCase();
        Assert.IsTrue(result == "1h");

        text = "A";
        result = text.toCamelCase();
        Assert.IsTrue(result == "a");

        text = "HEllo world 1234this is nICE";
        result = text.toCamelCase();
        Assert.IsTrue(result == "hello World 1234this Is Nice", result);

        text = "HELLO WORLD@EMAIL.COM. This is it. this is just a sample? or is it?";
        result = text.toCamelCase();
        Assert.IsTrue(result == "hello World@email.com. This Is It. This Is Just A Sample? Or Is It?", result);

        text = "Hello-world-this WAS-NICE";
        result = text.toCamelCase();
        Assert.IsTrue(result == "hello-World-This Was-Nice");
    }
}
