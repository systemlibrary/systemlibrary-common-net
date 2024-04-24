using System.Collections.Concurrent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests;

[TestClass]
public partial class ConcurrentDictionaryExtensionsTests
{
    [TestMethod]
    public void TryGet_TypeKey_With_Null_Dictionary_Returns_Item()
    {
        var type = typeof(string);

        ConcurrentDictionary<int, string> cache = null;

        var result = cache.TryGet(type, () => "Hello world");

        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void TryGet_StringKey_With_Null_Dictionary_Returns_Item()
    {
        var type = typeof(string);

        ConcurrentDictionary<string, string> cache = null;

        var result = cache.TryGet(type.Name, () => "Hello world");

        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void TryGet_TypeKey_Returns_Item()
    {
        var type = typeof(string);

        ConcurrentDictionary<int, string> cache = new ConcurrentDictionary<int, string>();

        var result = cache.TryGet(type, () => "Hello world");

        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void TryGet_StringKey_Returns_Item()
    {
        var type = typeof(string);

        ConcurrentDictionary<string, string> cache = new ConcurrentDictionary<string, string>();

        var result = cache.TryGet(type.Name, () => "Hello world");

        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void TryGet_TypeKey_Multiple_Invocations_Returns_Item()
    {
        var type = typeof(string);

        ConcurrentDictionary<int, string> cache = new ConcurrentDictionary<int, string>();

        var result = cache.TryGet(type, () => "Hello world");
        Assert.IsTrue(result == "Hello world");

        result = cache.TryGet(type, () => "Not returned");
        Assert.IsTrue(result == "Hello world");

        result = cache.TryGet(type, () => "Not returned");
        Assert.IsTrue(result == "Hello world");

        result = cache.TryGet(type, () => "Not returned");
        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void TryGet_StringKey_Multiple_Invocations_Returns_Item()
    {
        var type = typeof(string);

        ConcurrentDictionary<string, string> cache = new ConcurrentDictionary<string, string>();

        var result = cache.TryGet(type.Name, () => "Hello world");
        Assert.IsTrue(result == "Hello world");

        result = cache.TryGet(type.Name, () => "Not returned");
        Assert.IsTrue(result == "Hello world");

        result = cache.TryGet(type.Name, () => "Not returned");
        Assert.IsTrue(result == "Hello world");

        result = cache.TryGet(type.Name, () => "Not returned");
        Assert.IsTrue(result == "Hello world");
    }
}
