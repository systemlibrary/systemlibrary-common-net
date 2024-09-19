using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class EnumExtensionsTests
{
    public enum Colors
    {
        Black, White, Red, Blue
    }

    [TestMethod]
    public void Enum_Is_Any()
    {
        var red = Colors.Red;

        var result = red.IsAny(Colors.Black, Colors.Blue);

        Assert.IsFalse(result);
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
