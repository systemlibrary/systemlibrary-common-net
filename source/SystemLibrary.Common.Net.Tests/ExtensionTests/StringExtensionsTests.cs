using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;
using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void Enum_To_Value()
        {
            EnumTest a = EnumTest.A;
            EnumTest b = EnumTest.B;
            EnumTest c = EnumTest.C;

            //Assert.AreEqual(a.ToValue(), "A", "A");
            Assert.AreEqual(b.ToValue(), "hello123", "hello123");
            //Assert.AreEqual(c.ToValue(), "100", "100");
        }

        [TestMethodAttribute]
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
            //Assert.AreEqual("C", c1.ToString().ToEnum<EnumTest>().ToString(), "100 int");
            //Assert.AreEqual("C", c2.ToEnum<EnumTest>().ToString(), "100 str");
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
        public void Remove_If_Ends_With()
        {
            var data = "/";

            Assert.IsTrue(!data.RemoveIfEndsWith("/").Contains("/"));
            Assert.IsTrue(!data.RemoveIfEndsWith("\\").Contains("\\"));
        }
    }
}
