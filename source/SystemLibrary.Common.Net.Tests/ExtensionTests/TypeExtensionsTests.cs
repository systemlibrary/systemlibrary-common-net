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
    public void IsKeyValuePairType()
    {
        var type = typeof(string);

        Assert.IsTrue(!type.IsKeyValuePair());

        type = typeof(object);
        Assert.IsTrue(!type.IsKeyValuePair());

        type = typeof(List<int>);
        Assert.IsTrue(!type.IsKeyValuePair());
        type = typeof(IEnumerable<int>);
        Assert.IsTrue(!(type.IsKeyValuePair()));

        type = typeof(IEnumerable<object>);
        Assert.IsTrue(!(type.IsKeyValuePair()));

        type = typeof(KeyValuePair<int, int>);
        Assert.IsTrue(type.IsKeyValuePair());

        type = typeof(KeyValuePair<int, object>);
        Assert.IsTrue(type.IsKeyValuePair());

        type = typeof(KeyValuePair<string, int>);
        Assert.IsTrue(type.IsKeyValuePair());

        type = typeof(KeyValuePair<int, string>);
        Assert.IsTrue(type.IsKeyValuePair());
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
        expected = typeof(KeyValuePair<string, object>);
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected, "Dictionary keyvaluepair");

        type = typeof(Dictionary<int, List<int>>);
        expected = typeof(KeyValuePair<int, List<int>>);
        res = type.GetFirstGenericType();
        Assert.IsTrue(res == expected, "Dictionary keyvaluepair list of int");
    }

    [TestMethod]
    public void IsList_Or_Array_Success()
    {
        var list = new List<int>();

        var res = list.GetType().IsListOrArray();

        Assert.IsTrue(res, "List");

        var a = new List<int>();
        var b = (object)a;
        var ilist = (IList<int>)b;

        res = ilist.GetType().IsListOrArray();

        Assert.IsTrue(res, "IList");

        var arr = new int[1];

        res = arr.GetType().IsListOrArray();
        Assert.IsTrue(res, "Array");
    }
}
