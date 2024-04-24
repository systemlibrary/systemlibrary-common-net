using System.Collections.Concurrent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests;

[TestClass]
public partial class DictionaryCacheTests
{
    [TestMethod]
    public void Get_Without_Dictionary_Returns_Success()
    {
        var type = typeof(DictionaryCache);

        var result = DictionaryCache.Get(type, null, () => "Hello world");

        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void Get_Dictionary_Returns_Success()
    {
        var type = typeof(DictionaryCache);

        var dictionary = new ConcurrentDictionary<int, string>();

        var result = DictionaryCache.Get(type, dictionary, () => "Hello world");

        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void Get_Dictionary_StringKey_Returns_Success()
    {
        var type = typeof(DictionaryCache);

        var dictionary = new ConcurrentDictionary<string, string>();

        var result = DictionaryCache.Get(type.Name, dictionary, () => "Hello world");

        Assert.IsTrue(result == "Hello world");
    }

    [TestMethod]
    public void Get_Dictionary_StringKey_Multiple_Invocations_Success()
    {
        var type = typeof(DictionaryCache);

        var dictionary = new ConcurrentDictionary<string, string>();

        var result = DictionaryCache.Get(type.Name, dictionary, () => "Hello world");
        Assert.IsTrue(result == "Hello world");

        result = DictionaryCache.Get(type.Name, dictionary, () => "Hello world1");
        Assert.IsTrue(result == "Hello world");

        result = DictionaryCache.Get(type.Name, dictionary, () => "Hello world2");
        Assert.IsTrue(result == "Hello world");

        result = DictionaryCache.Get(type.Name, dictionary, () => "Hello world3");
        Assert.IsTrue(result == "Hello world");
    }
}
