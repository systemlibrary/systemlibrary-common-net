using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class TypeExtensionsTests 
{
    [TestMethod]
    public void Inherits_Test()
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
}
