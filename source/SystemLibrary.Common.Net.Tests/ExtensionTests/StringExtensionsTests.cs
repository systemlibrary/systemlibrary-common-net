using System;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;
using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public partial class StringExtensionsTests
{
    [TestInitialize]
    public void TestInitialize()
    {
        var typeKey = Type.GetType("SystemLibrary.Common.Net.CryptationKey, SystemLibrary.Common.Net");

        typeKey.SetStaticMember("_Key", null);
    }

    [TestMethod]
    public void Is_Json_Success()
    {
        var data = "{ \"firstName\": \"Hello\", \"age\": 10 }";

        Assert.IsTrue(data.IsJson(), "Invalid json 1");

        data = Assemblies.GetEmbeddedResource("_Files", "data.json");

        Assert.IsTrue(data.IsJson(), "Invalid json 2");

        data = Assemblies.GetEmbeddedResource("_Files", "data-array-space-and-newline.json");

        Assert.IsTrue(data.IsJson(), "Invalid json 3");

        data = Assemblies.GetEmbeddedResource("_Files", "data-new-line.json");

        Assert.IsTrue(data.IsJson(), "Invalid json 4");
    }

    [TestMethod]
    public void Encrypt_Hello_World()
    {
        var serviceCollection = Services.Configure();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceProvider);

        var data = "Hello world";

        var enc = data.Encrypt();
        var prev = "1hGSKl2xIGoboY7NmkctiEZCS52o6+C2MeGTBe5YAYQ=";
        Assert.IsTrue(prev.Length == enc.Length && enc.EndsWith("="), "Enc with default 32 char key changed: " + enc);

        var dec = enc.Decrypt();

        Assert.IsTrue(dec == data, "Decrypt has changed: " + dec);
    }

    [TestMethod]
    public void Encrypt_And_Decrypt_With_Key_And_Iv_Success()
    {
        var userdata = "123456789-1234-4567-9000-abdef12346789;12345678;email@dummy.com;SpecialChars@|1,-.:;##End";
        var key = "aaaaa.CCCDDeeeFF-GGGG.HHHiiiii";
        var iv = "1122334455666-";

        for (var i = 10; i < 100; i++)
        {
            var tempKey = key + i;
            var tempIv = iv + i;
            var encrypted = userdata.Encrypt(tempKey.GetBytes(), tempIv.GetBytes());
            var decrypted = encrypted.Decrypt(tempKey.GetBytes(), tempIv.GetBytes());
            Assert.IsTrue(userdata == decrypted, "Index failed " + i);
        }

        key = "aaaaa.CCCDDeee";
        iv = "1122334455666-";

        for (var i = 10; i < 100; i++)
        {
            var tempKey = key + i;
            var tempIv = iv + i;
            var encrypted = userdata.Encrypt(tempKey.GetBytes(), tempIv.GetBytes());
            var decrypted = encrypted.Decrypt(tempKey.GetBytes(), tempIv.GetBytes());
            Assert.IsTrue(userdata == decrypted, "Index failed " + i);
        }
    }

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
        result = data.Obfuscate(int.MaxValue);
        Assert.IsTrue(result.Length == data.Length);
        Assert.IsTrue(result != data);
        result = result.Deobfuscate(int.MaxValue);
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
        result = result.FromBase64(Encoding.UTF8);
        Assert.IsTrue(result == data);

        data = "";
        result = data.ToBase64();
        Assert.IsTrue(result == "");
        result = result.FromBase64(Encoding.UTF8);
        Assert.IsTrue(result == data);

        data = "hello world";
        result = data.ToBase64();
        Assert.IsTrue(result.Length > 10 && result.EndsWith("="));
        result = result.FromBase64(Encoding.UTF8);
        Assert.IsTrue(result == data);

        data = "A lot of various characters ÆØÅæøå ABCDEFGHIJKLMONPQRSTUVXYZ: 1234567890,.-_?\"*|!#¤%&/()=?…±òü±²³";
        result = data.ToBase64();
        Assert.IsTrue(result != null);
        result = result.FromBase64(Encoding.UTF8);
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

        Assert.AreEqual(a.GetEnumValue(), null, "A");
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
        Assert.AreEqual("A", a00.ToEnum<EnumTest>().ToString(), "a00 !!!");

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
    public void Int_To_Enum_Matching_Name_Prefixed_With_Underscore()
    {
        var i997 = 997;
        var i998 = 998;
        var i999 = 999;

        var e997 = i997.ToString().ToEnum<EnumTest>();
        var e998 = i998.ToString().ToEnum<EnumTest>();
        var e999 = i999.ToString().ToEnum<EnumTest>();

        Assert.IsTrue(e997 == EnumTest._997, "e997 is not _997, " + e997.ToString());
        Assert.IsTrue(e998 == EnumTest._999, "e998 is not _999, " + e998.ToString());
        Assert.IsTrue(e999 == EnumTest._999, "e999 is not _999, " + e999.ToString());
    }

    [TestMethod]
    public void Enum_To_Value_With_Underscore_Name_Returns_The_Int()
    {
        EnumTest _997 = EnumTest._997;
        EnumTest _999 = EnumTest._999;

        var value7 = _997.ToValue();
        var value9 = _999.ToValue();

        Assert.IsTrue(value7 == "997", "997? " + value7);
        Assert.IsTrue(value9 == "998", "998? " + value9);
    }

    [TestMethod]
    public void ToEnum_Invalid_Text_Value_And_Name_But_Matches_CaseInsensitiveName()
    {
        var find = "c";
        var res = find.ToEnum<EnumTest>();
        Assert.IsTrue(res == EnumTest.C);

        find = "D";
        res = find.ToEnum<EnumTest>();
        Assert.IsTrue(res == EnumTest.d);

        find = "E";
        res = find.ToEnum<EnumTest>();
        Assert.IsTrue(res == EnumTest.e);
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
        var result = data.EndsWithAny(StringComparison.OrdinalIgnoreCase, "helloworld", "something", "another one", "whatever", "world123!aa");
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
        Assert.IsTrue(result == true, "Values is false when Empty");

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

    [TestMethod]
    public void Convert_ToBOM_Success()
    {
        var bomBytes = new byte[] {
            239,
            187,
            191
        };
        var bomChar = Encoding.UTF8.GetString(bomBytes);

        var test = "".ToUtf8BOM();
        var expected = "";

        Assert.IsTrue(test == expected, "Err1: " + test);

        test = "ÆØÅæøå?".ToUtf8BOM();
        expected = "ÆØÅæøå?";

        Assert.IsTrue(test == bomChar + expected, "Err2: " + test);

        test = "ÆØÅæøå?".ToUtf8BOM().ToUtf8BOM().ToUtf8BOM().ToUtf8BOM().ToUtf8BOM();
        expected = "ÆØÅæøå?";

        Assert.IsTrue(test == bomChar + expected, "Err3: " + test);

        test = "ÆØÅæøå?".ToUtf8BOM();
        expected = "ÆØÅæøå?".ToUtf8BOM();

        Assert.IsTrue(test == expected, "Err4: " + test);
    }

    [TestMethod]
    public void To_App_Path()
    {
        string text = null;
        string result = text.ToPhysicalPath();
        Assert.IsTrue(result == null);

        text = "";
        result = text.ToPhysicalPath();
        
        var root = "C:/syslib/systemlibrary-common-net/source/SystemLibrary.Common.Net.Tests/bin/Release/net7.0/";

        Assert.IsTrue(result == root, "1 " + result);

        text = "a";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "a", "2 " + result);

        text = "a/b/c/d/e/12345/";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "a/b/c/d/e/12345/", "3 " + result);

        text = "https://www.systemlibrary.com/hello/world/";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "hello/world/", "4 " + result);

        text = "https://www.sub.sub.subdomain.com/hello/world/";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "hello/world/", "5 " + result);

        text = "https://www.sub.sub.subdomain.com/hello1/world2/?hello=world&hello=/world/";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "hello1/world2/", "6 " + result);

        text = "https://www.sub.sub.subdomain.com";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root, "7 " + result);

        text = "/a/b/c";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "a/b/c", "8 " + result);

        text = "/a/b/c/";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "a/b/c/", "9 " + result);

        text = "\\a\\b\\";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "a/b/", "10 " + result);

        text = "a\\b\\";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "a/b/", "11 " + result);

        text = "a\\b";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == root + "a/b", "12 " + result);

        text = "C:/syslib/systemlibrary-common-net/source/SystemLibrary.Common.Net.Tests/a/";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == text, "13 " + result);

        text = "C:/syslib/systemlibrary-common-net/source/SystemLibrary.Common.Net.Tests/a";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == text, "14 " + result);

        text = "C:\\syslib\\systemlibrary-common-net\\source\\SystemLibrary.Common.Net.Tests\\a";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == "C:/syslib/systemlibrary-common-net/source/SystemLibrary.Common.Net.Tests/a", "15 " + result);

        text = "C:\\hello\\world";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == "C:/hello/world", "16 " + result);

        text = "C:\\hello\\world\\";
        result = text.ToPhysicalPath();
        Assert.IsTrue(result == "C:/hello/world/", "17 " + result);
    }

    [TestMethod]
    public void Encrypt_And_Decrypt_Is_Success()
    {
        string data = null;
        string result = data;

        Assert.IsTrue(result == data.Encrypt().Decrypt(), "Data null failed");

        data = "";
        result = data;
        Assert.IsTrue(result == data.Encrypt().Decrypt(), "Blank: " + data.Encrypt());

        data = "abcdef";
        result = data;
        Assert.IsTrue(result == data.Encrypt().Decrypt(), "abcdef: " + data.Encrypt());

        data = "@£$$€{[]}abcdefghijklmno \n\n\n\t\tpqr stuvwxyzæø åABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ^^*'?=)(/&%¤#\"!|`1234567890 <>;:,.-_ /*-+";
        result = data;
        Assert.IsTrue(result == data.Encrypt().Decrypt(), "long: " + data.Encrypt());
    }

    [TestMethod]
    public void Encrypt_And_Decrypt_With_Custom_Key()
    {
        string data = "Hello";
        string result = data;
        var key = "1234567890123456";

        Assert.IsTrue(result == data.Encrypt(key).Decrypt(key), "Data null failed");

        var enc1 = data.Encrypt(key, addIV: true);
        var enc2 = data.Encrypt(key, addIV: true);
        var enc3 = data.Encrypt(key, addIV: true);

        Assert.IsFalse(enc1 == enc2 && enc2 == enc3, "Is deterministic with null as IV, it shouldve used random IV");

        var dec1 = enc1.Decrypt(key, addedIV: true);
        var dec2 = enc2.Decrypt(key, addedIV: true);
        var dec3 = enc3.Decrypt(key, addedIV: true);

        Assert.IsTrue(dec1 == dec2 && dec2 == dec3 && dec3 == data, "Error: " + dec1);

        var iv = "1234567890123456";

        // ENCRYPT WITH KEY,IV, but ADD IV to output, just need Key to decrypt properly
        var enc4 = data.Encrypt(key, iv, addIV: true);
        var enc5 = data.Encrypt(key, iv, addIV: true);
        var enc6 = data.Encrypt(key, iv, addIV: true);

        Assert.IsTrue(enc4 == enc5 && enc5 == enc6, "Is NOT deterministic with null as IV, it shouldve used random IV");

        var dec4 = enc4.Decrypt(key, addedIV: true);
        var dec5 = enc5.Decrypt(key, addedIV: true);
        var dec6 = enc6.Decrypt(key, addedIV: true);

        Assert.IsTrue(dec4 == dec5 && dec5 == dec6 && dec6 == data, "Error: " + dec1);

        // ENCRYPT WITH KEY,IV, not adding IV to output
        var enc7 = data.Encrypt(key, iv);
        var enc8 = data.Encrypt(key, iv);
        var enc9 = data.Encrypt(key, iv);

        Assert.IsTrue(enc7 == enc8 && enc8 == enc9, "Is NOT deterministic with null as IV, it shouldve used random IV");

        var dec7 = enc7.Decrypt(key, iv);
        var dec8 = enc8.Decrypt(key, iv);
        var dec9 = enc9.Decrypt(key, iv);

        Assert.IsTrue(dec7 == dec8 && dec8 == dec9 && dec9 == data, "Error: " + dec1);
    }

    [TestMethod]
    public void Encrypt_And_Decrypt_With_Salt16_Is_Success()
    {
        string data = null;
        string result = data;

        var key = "ABCDEF1234567890".GetBytes();

        Assert.IsTrue(result == data.Encrypt(key).Decrypt(key), "Data null failed");

        data = "";
        result = data;
        Assert.IsTrue(result == data.Encrypt(key).Decrypt(key), "Blank: " + data.Encrypt(key));

        data = "abcdef";
        result = data;
        Assert.IsTrue(result == data.Encrypt(key).Decrypt(key), "abcdef: " + data.Encrypt(key) + " VS " + data.Encrypt(key).Decrypt(key));

        data = "@£$$€{[]}abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ^^*'?=)(/&%¤#\"!|`1234567890 <>;:,.-_ /*-+";
        result = data;
        Assert.IsTrue(result == data.Encrypt(key).Decrypt(key), "long: " + data.Encrypt(key));
    }

    [TestMethod]
    public void Encrypt_Decrypt_When_Key16_and_IV16_Are_Strings()
    {
        string data = null;
        string result = data;

        var salt16 = "ABCDEF1234567890";

        var key = "1234567890123456";

        Assert.IsTrue(result == data.Encrypt(key, salt16).Decrypt(key, salt16), "Data null failed");

        data = "";
        result = data;
        Assert.IsTrue(result == data.Encrypt(key, salt16).Decrypt(key, salt16), "Blank: " + data.Encrypt(salt16));

        data = "abcdef";
        result = data;
        Assert.IsTrue(result == data.Encrypt(key, salt16).Decrypt(key, salt16), "abcdef: " + data.Encrypt(salt16));

        data = "@£$$€{[]}abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ^^*'?=)(/&%¤#\"!|`1234567890 <>;:,.-_ /*-+";
        result = data;
        Assert.IsTrue(result == data.Encrypt(key, salt16).Decrypt(key, salt16), "long: " + data.Encrypt(salt16));
    }

    [TestMethod]
    public void Decrypt_Encrypted_String_MultipleTimes_In_A_Row_Success()
    {
        for (var i = 0; i < 2000; i++)
        {
            var value = "abcdef";

            var encrypted = value.Encrypt();

            var result = encrypted.Decrypt();

            Assert.IsTrue(encrypted.Is() && encrypted != value);
            Assert.IsTrue(result == value);
        }
    }

    static int Decrypt_In_Async_Startup_Success_Counter = 0;
    static void Decrypt_In_Async_Startup_Success_Counter_Increment()
    {
        Decrypt_In_Async_Startup_Success_Counter++;
    }

    [TestMethod]
    public void Decrypt_In_Async_Startup_Success()
    {
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceCollection);
        Services.Configure(serviceProvider);

        var r = new Random(DateTime.Now.Millisecond);

        Async.FireAndForget(() => Call(0));
        Async.FireAndForget(() => Call(0));
        Async.FireAndForget(() => Call(0));
        Async.FireAndForget(() => Call(0));
        Async.FireAndForget(() => Call(0));

        Async.FireAndForget(() => Call(r.Next(10, 450)));
        Async.FireAndForget(() => Call(r.Next(10, 450)));
        Async.FireAndForget(() => Call(r.Next(10, 450)));
        Async.FireAndForget(() => Call(r.Next(10, 450)));
        Async.FireAndForget(() => Call(r.Next(10, 400)));
        Async.FireAndForget(() => Call(r.Next(10, 400)));
        Async.FireAndForget(() => Call(r.Next(10, 400)));
        Async.FireAndForget(() => Call(r.Next(10, 400)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));
        Async.FireAndForget(() => Call(r.Next(10, 350)));

        System.Threading.Thread.Sleep(625);

        Assert.IsTrue(Decrypt_In_Async_Startup_Success_Counter == 0, "Exception counter was: " + Decrypt_In_Async_Startup_Success_Counter);
    }

    static void Call(int sleep)
    {
        System.Threading.Thread.Sleep(sleep);
        try
        {
            var data = "1hGSKl2xIGoboY7NmkctiEZCS52o6+C2MeGTBe5YAYQ=";
            var result = data.Decrypt();

            if (result != "Hello world")
                Decrypt_In_Async_Startup_Success_Counter_Increment();
        }
        catch (Exception ex)
        {
            Dump.Write(ex.Message + " Encrypted data was: " + "Hello world".Encrypt());

            Decrypt_In_Async_Startup_Success_Counter_Increment();
        }
    }
}