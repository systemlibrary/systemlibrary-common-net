using System;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests
{
    [TestClass]
    public class StringBuilderExtensionsTests
    {
        [TestMethod]
        public void Test_Remove_EndsWith()
        {
            StringBuilder sb = null;
            bool result = false;
            bool expected = false;

            result = sb.TrimEnd("abc");
            Assert.IsTrue(result == expected, "abc 1 errored");

            sb = new StringBuilder();
            result = sb.TrimEnd("abc");
            Assert.IsTrue(result == expected, "abc 2 errored");

            sb = new StringBuilder("");
            result = sb.TrimEnd("abc");
            Assert.IsTrue(result == expected, "abc 3 errored");

            sb = new StringBuilder("ab");
            result = sb.TrimEnd("abc");
            Assert.IsTrue(result == expected, "abc 4 errored");

            sb = new StringBuilder("abc");
            result = sb.TrimEnd("abc");
            expected = true;
            Assert.IsTrue(result == expected, "abc 5 errored");
            Assert.IsTrue(sb.ToString() == "", "abc was not removed");

            sb = new StringBuilder("Hello World !234567890-.-,_:_");
            result = sb.TrimEnd("Hello World !234567890-.-,_:_");
            expected = true;
            Assert.IsTrue(result == expected, "Hello world errored");
            Assert.IsTrue(sb.ToString() == "", "Hell oworld was not removed");

            sb = new StringBuilder("ABC");
            result = sb.TrimEnd("abC");
            expected = false;
            Assert.IsTrue(result == expected, "abC 1 errored");
            Assert.IsTrue(sb.ToString() == "ABC", "abC was not removed");

            sb = new StringBuilder("ABC");
            result = sb.TrimEnd("ABC ");
            expected = false;
            Assert.IsTrue(result == expected, "ABC 2 errored");
            Assert.IsTrue(sb.ToString() == "ABC", "ABC was not removed");

            sb = new StringBuilder("1234567890?ÆØÅÆØÅ");
            result = sb.TrimEnd("ÆØÅ");
            expected = true;
            Assert.IsTrue(result == expected, "ÆØÅ 3 errored");
            Assert.IsTrue(sb.ToString() == "1234567890?ÆØÅ", "Only one ÆØÅ was removed");

            sb = new StringBuilder("ceo@systemlibrary.com");
            result = sb.TrimEnd("ceo@SYstemlibRary.COM");
            expected = false;
            Assert.IsTrue(result == expected, "Email errored");
            Assert.IsTrue(sb.ToString() == "ceo@systemlibrary.com", "Email was not removed");

            sb = new StringBuilder("ceo@SYstemlibRary.COM");
            result = sb.TrimEnd("ceo@SYstemlibRary.COM");
            expected = true;
            Assert.IsTrue(result == expected, "Email errored");
            Assert.IsTrue(sb.ToString() == "", "Email was not removed");

            sb = new StringBuilder("hello world this is the end this is the end this is the end");
            result = sb.TrimEnd("hello", "world", "or", "this is the end");
            sb.TrimEnd(" ");
            expected = true;
            Assert.IsTrue(result == expected, "Hello world 2 multiple errored");
            Assert.IsTrue(sb.ToString() == "hello world this is the end this is the end", "Hello world 2 multiple was not removed");
        }

        [TestMethod]
        public void Test_EndsWith()
        {
            StringBuilder sb = null;
            bool result = false;
            bool expected = false;

            result = sb.EndsWith("abc");
            Assert.IsTrue(result == expected, "abc 1 errored");

            sb = new StringBuilder();
            result = sb.EndsWith("abc");
            Assert.IsTrue(result == expected, "abc 2 errored");

            sb = new StringBuilder("");
            result = sb.EndsWith("abc");
            Assert.IsTrue(result == expected, "abc 3 errored");

            sb = new StringBuilder("ab");
            result = sb.EndsWith("abc");
            Assert.IsTrue(result == expected, "abc 4 errored");

            sb = new StringBuilder("abc");
            result = sb.EndsWith("abc");
            expected = true;
            Assert.IsTrue(result == expected, "abc 5 errored");

            sb = new StringBuilder("ABCD");
            result = sb.EndsWith("abc");
            expected = false;
            Assert.IsTrue(result == expected, "abc 6 errored");

            sb = new StringBuilder("abcdef 12354-.-1-51.64+06913+0|13125?!?!:__;:");
            result = sb.EndsWith("abc");
            expected = false;
            Assert.IsTrue(result == expected, "abc 7 errored");

            sb = new StringBuilder("abcdef 12354-.-1-51.64+06913+0|13125?!?!:__;:abc");
            result = sb.EndsWith("abc");
            expected = true;
            Assert.IsTrue(result == expected, "abc 8 errored");

            sb = new StringBuilder("abcdef 12354-.-1-51.64+06913+0|13125?!?!:__;:abc");
            result = sb.EndsWith("ABC");
            expected = false;
            Assert.IsTrue(result == expected, "abc 9 errored");

            sb = new StringBuilder("abc");
            result = sb.EndsWith("ABC", true);
            expected = true;
            Assert.IsTrue(result == expected, "abc 10 errored");


            sb = new StringBuilder("Hello World æøå");
            result = sb.EndsWith("Å", true);
            expected = true;
            Assert.IsTrue(result == expected, "ÆØÅ 1 errored");

            sb = new StringBuilder("Hello World æøå");
            result = sb.EndsWith("æøå", true);
            expected = true;
            Assert.IsTrue(result == expected, "ÆØÅ 2 errored");

            sb = new StringBuilder("Hello World ÆØÅ");
            result = sb.EndsWith("æøå", true);
            expected = true;
            Assert.IsTrue(result == expected, "ÆØÅ 3 errored");

            sb = new StringBuilder("Hello World Æ Ø Å");
            result = sb.EndsWith(" Æ Ø å", true);
            expected = true;
            Assert.IsTrue(result == expected, "ÆØÅ 4 errored");

            sb = new StringBuilder("Hello World æøå");
            result = sb.EndsWith("Å", false);
            expected = false;
            Assert.IsTrue(result == expected, "ÆØÅ 5 errored");

            sb = new StringBuilder("Hello World æøå");
            result = sb.EndsWith("æøå");
            expected = true;
            Assert.IsTrue(result == expected, "ÆØÅ 6 errored");

            sb = new StringBuilder("Hello World ÆØÅ");
            result = sb.EndsWith("æøå");
            expected = false;
            Assert.IsTrue(result == expected, "ÆØÅ 7 errored");

            sb = new StringBuilder("Hello World Æ Ø Å");
            result = sb.EndsWith(" Æ Ø å");
            expected = false;
            Assert.IsTrue(result == expected, "ÆØÅ 8 errored");
        }
    }
}
