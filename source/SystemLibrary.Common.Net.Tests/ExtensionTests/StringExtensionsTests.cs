using System.Collections.Generic;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;
using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void Convert_String_To_User()
        {
            var data = "{ \"firstName\": \"Hello\", \"age\": 10 }";

            var user = data.ToJson<User>();

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
            var user = data.ToJson<User>(options);

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
        public void TrimEnd()
        {
            var data = "/";

            Assert.IsTrue(!data.TrimEnd("/").Contains("/"));
            Assert.IsTrue(!data.TrimEnd("\\").Contains("\\"));
        }
     
        [TestMethod]
        public void Test_GetPrimaryDomain()
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
    }
}
