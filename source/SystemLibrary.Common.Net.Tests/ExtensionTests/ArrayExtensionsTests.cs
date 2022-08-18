using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class ArrayExtensionsTests
{
    [TestMethod]
    public void Array_Add_Strings_To_Empty()
    {
        var names = new string[] { };

        var res = names.Add(new string[] { "World", "world", "w0rld" });

        Assert.IsTrue(res.Length == 3);
    }

    [TestMethod]
    public void Array_Add_Empty_To_Empty()
    {
        var names = new string[] { };

        var res = names.Add(new string[] { });

        Assert.IsTrue(res.Length == 0);
    }

    [TestMethod]
    public void Array_Add_Empty_To_Null()
    {
        string[] names = null;

        var res = names.Add(new string[] { });

        Assert.IsTrue(res.Length == 0);
    }

    [TestMethod]
    public void Array_Add_Null_To_Null()
    {
        string[] names = null;

        string[] add = null;
        var res = names.Add(add);

        Assert.IsTrue(res == null);
    }

    [TestMethod]
    public void Array_Add_Strings_To_Strings()
    {
        var names = new string[] { "Hello", "hello", "hell0" };

        var res = names.Add(new string[] { "World", "world", "w0rld" });

        Assert.IsTrue(res.Length == 6);
    }

    [TestMethod]
    public void Array_Add_Ints_To_Ints()
    {
        var names = new int[] { 1, 2, 3, 4 };

        var res = names.Add(new int[] { 1, 2, 3, 4, 5 });

        Assert.IsTrue(res.Length == 9);
    }

    static bool Array_Add_ints_Predicate(int number)
    {
        return number > 2;
    }

    [TestMethod]
    public void Array_Add_Int_Array_To_Int_Array_With_Predicate()
    {
        var numbers = new int[] { 1, 2, 3, 4 };

        var res = numbers.Add(Array_Add_ints_Predicate, new int[] { 1, 2, 3, 4, 5 });

        Assert.IsTrue(res.Length == 5);
    }

    [TestMethod]
    public void Array_Add_Multiple_Int_Arrays_To_Int_Array_With_Predicate()
    {
        var numbers = new int[] { 1, 2, 3, 4 };
        var add1 = new int[] { 1, 2, 3, 4, 5 };
        var add2 = new int[] { 5, 6, 7, 8 };
        var add3 = new int[] { 0, 1, -1, -2, 9 };

        var res = numbers.Add(Array_Add_ints_Predicate, add1, add2, add3);

        Assert.IsTrue(res.Length == 10);
    }
}
