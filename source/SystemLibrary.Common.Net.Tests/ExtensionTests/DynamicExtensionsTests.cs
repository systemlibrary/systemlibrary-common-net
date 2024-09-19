using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class DynamicExtensionsTests
{
    static dynamic a = new
    {
        firstName = "Hello",
        age = 1
    };
    static dynamic b = new
    {
        firstName = "World",
        Age = 2
    };
    static dynamic c = new
    {
        age = 99,
        Age = 100
    };

    [TestMethod]
    public void Merge_Dynamic_Objects_As_Dynamic()
    {
        var result = DynamicExtensions.Merge(a, b);
        Assert.IsTrue(result.age == 1, "age");
        Assert.IsTrue(result.Age == 2, "Age");
        Assert.IsTrue(result.firstName == "World", "firstName");

        var dictionary = result as IDictionary<string, object>;
        Assert.IsTrue(dictionary.Keys.Count == 3, "Count");
    }

    [TestMethod]
    public void Merge_Multiple_Dynamic_Objects_As_Dynamic()
    {
        var result = DynamicExtensions.Merge(a, b, c);
        Assert.IsTrue(result.age == 99, "age");
        Assert.IsTrue(result.Age == 100, "Age");
        Assert.IsTrue(result.firstName == "World", "firstName");

        var dictionary = result as IDictionary<string, object>;
        Assert.IsTrue(dictionary.Keys.Count == 3, "Count");
    }
}
