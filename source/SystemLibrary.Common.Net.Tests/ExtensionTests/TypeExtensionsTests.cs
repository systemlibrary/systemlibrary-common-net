using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class TypeExtensionsTests
{
    [TestMethod]
    public void Type_Inherits()
    {
        var stringType = typeof(string);
        var objectType = typeof(object);
        var listType = typeof(List<int>);
        var enumerableType = typeof(IEnumerable<int>);

        var res = stringType.Inherits(objectType);
        Assert.IsTrue(res);

        res = objectType.Inherits(objectType);
        Assert.IsTrue(!res);

        res = objectType.Inherits(stringType);
        Assert.IsTrue(!res);

        res = listType.Inherits(objectType);
        Assert.IsTrue(res);

        res = listType.Inherits(enumerableType);
        Assert.IsTrue(res);
    }

    [TestMethod]
    public void Type_Get_First_Generic_Type()
    {
        var type = typeof(IList<string>);
        var expected = typeof(string);
        var res = type.GetFirstGenericType();

        Assert.IsTrue(res == expected);

        type = typeof(string);
        expected = null;
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected);

        type = null;
        expected = null;
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected);

        type = typeof(int);
        expected = null;
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected, "int");

        type = typeof(List<int>);
        expected = typeof(int);
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected, "List int");

        type = typeof(Dictionary<string, object>);
        expected = typeof(KeyValuePair<string,object>);
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected, "Dictionary keyvaluepair");

        type = typeof(Dictionary<int, List<int>>);
        expected = typeof(KeyValuePair<int, List<int>>);
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected, "Dictionary keyvaluepair list of int");
    }
}
