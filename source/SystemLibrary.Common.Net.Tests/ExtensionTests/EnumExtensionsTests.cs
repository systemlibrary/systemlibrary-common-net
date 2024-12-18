﻿using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Attributes;
using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class EnumExtensionsTests
{
    public enum Colors
    {
        Black,
        [EnumValue("white")]
        White,
        Red,
        [EnumValue("b")]
        Blue
    }

    [TestMethod]
    public void Enum_Is_Any()
    {
        var red = Colors.Red;

        var result = red.IsAny(Colors.Black, Colors.Blue);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Enum_GetValues()
    {
        var values = EnumExtensions<Colors>.GetValues();

        Assert.IsTrue(values.Count() == 4);

        Assert.IsTrue(values.First() == "Black");

        Assert.IsTrue(values.Skip(1).First() == "white");

        Assert.IsTrue(values.Last() == "b");
    }

    [TestMethod]
    public void Enum_GetKeys()
    {
        var all = EnumExtensions<Colors>.GetKeys();

        Assert.IsTrue(all.Count() == 4);

        var keys = EnumExtensions<Colors>.GetKeys();
        Assert.IsTrue(keys.Count() == 4);

        var values = EnumExtensions<Colors>.GetValues();
        Assert.IsTrue(keys.Count() == 4);
    }

    [TestMethod]
    public void Enum_To_Array()
    {
        var integers = new object[] { 1, 2, 3, 4 };

        var colors = integers.AsEnumArray<Colors>();

        Assert.IsTrue(colors.Length == integers.Length);
        Assert.IsTrue(colors[2] == Colors.Blue);
    }


    [TestMethod]
    public void Enum_To_Array_With_Strings_As_Object_Array()
    {
        var texts = new object[] { "Black", "White", "Blue" };

        var colors = texts.AsEnumArray<Colors>();

        Assert.IsTrue(colors.Length == texts.Length);
        Assert.IsTrue(colors[1] == Colors.White);
    }

    [TestMethod]
    public void Enum_To_Array_With_Strings_As_String_Array()
    {
        var texts = new string[] { "Black", "White", "Blue" };

        var colors = texts.AsEnumArray<Colors>();

        Assert.IsTrue(colors.Length == texts.Length);
        Assert.IsTrue(colors[1] == Colors.White);
    }
}
