using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;
using SystemLibrary.Common.Net.Tests.XServiceTestsRunsLast;

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

    [TestMethod]
    public void GetTypeName_Success()
    {
        var type = typeof(string);
        var name = type.GetTypeName();
        Assert.IsTrue("String" == name, name);

        type = typeof(int);
        name = type.GetTypeName();
        Assert.IsTrue("Int32" == name, name);

        type = typeof(DateTimeJsonConverter);
        name = type.GetTypeName();
        Assert.IsTrue("DateTimeJsonConverter" == name, name);

        type = typeof(List<>);
        name = type.GetTypeName();
        Assert.IsTrue("<>" == name, name);

        type = typeof(List<int>);
        name = type.GetTypeName();
        Assert.IsTrue("Int32" == name, name);

        type = typeof(List<string>);
        name = type.GetTypeName();
        Assert.IsTrue("String" == name, name);

        type = typeof(IList<DateTimeOffset>);
        name = type.GetTypeName();
        Assert.IsTrue("DateTimeOffset" == name, name);

        type = typeof(IDictionary<,>);
        name = type.GetTypeName();
        Assert.IsTrue("<,>" == name, name);

        type = typeof(IDictionary<object,int>);
        name = type.GetTypeName();
        Assert.IsTrue("Object, Int32" == name, name);
    }

    [TestMethod]
    public void IsDictionary()
    {
        var type = typeof(string);
        var res = type.IsDictionary();
        Assert.IsTrue(res == false);

        type = typeof(int[]);
        res = type.IsDictionary();
        Assert.IsTrue(res == false);

        type = typeof(List<string>);
        res = type.IsDictionary();
        Assert.IsTrue(res == false);

        type = typeof(IList<string>);
        res = type.IsDictionary();
        Assert.IsTrue(res == false);

        type = typeof(IDictionary<string, int>);
        res = type.IsDictionary();
        Assert.IsTrue(res == true);

        type = typeof(IDictionary<object, object>);
        res = type.IsDictionary();
        Assert.IsTrue(res == true);

        type = typeof(Dictionary<int, bool>);
        res = type.IsDictionary();
        Assert.IsTrue(res == true);
    }

    [TestMethod]
    public void Test_Is_Internal()
    {
        var type = typeof(string);
        var res = type.IsInternal();
        Assert.IsTrue(res == false, "Its true string");

        type = typeof(ServiceAesEncryptionTest);
        res = type.IsInternal();
        Assert.IsTrue(res == false, "Its true: Service...");

        type = typeof(IsInternal);
        res = type.IsInternal();
        Assert.IsTrue(res == true, "Internal is not internal");
    }
}
