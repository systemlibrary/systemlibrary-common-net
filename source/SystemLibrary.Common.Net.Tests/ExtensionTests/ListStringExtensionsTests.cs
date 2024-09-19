using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Attributes;
using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class ListStringExtensionsTests
{
    [TestMethod]
    public void List_To_Enum_List()
    {
        List<string> data = null;
        var result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 0, "Count is not 0 from null list");

        data = new List<string>();
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 0, "Count is not 0 from empty list");

        data = new List<string> { "Car2", "Car3", "cAR4" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 3);
        Assert.IsTrue(result[0] == Cars.Car2);
        Assert.IsTrue(result[1] == Cars.Car3);
        Assert.IsTrue(result[2] == Cars.Car4);

        data = new List<string> { "CAR4", "car3", "cAR2" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 3, "Count is not 3");
        Assert.IsTrue(result[0] == Cars.Car4, "First is not car4");
        Assert.IsTrue(result[1] == Cars.Car3);
        Assert.IsTrue(result[2] == Cars.Car2);

        data = new List<string> { null, null, null, "Car3", "Hello" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 5);
        Assert.IsTrue(result[0] == Cars.Car1);
        Assert.IsTrue(result[1] == Cars.Car1);
        Assert.IsTrue(result[2] == Cars.Car1);
        Assert.IsTrue(result[3] == Cars.Car3);
        Assert.IsTrue(result[4] == Cars.Car4);

        data = new List<string> { "Yes", "yes", null, "YES" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 4, "Count is not 4");
        Assert.IsTrue(result[0] == Cars.Car2, "First is not car2");
        Assert.IsTrue(result[1] == Cars.Car2, "Second is not car1");
        Assert.IsTrue(result[2] == Cars.Car1);
        Assert.IsTrue(result[3] == Cars.Car2);
    }

    [TestMethod]
    public void IList_To_Enum_List()
    {
        IList<string> data = null;
        var result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 0, "Count is not 0 from null list");

        data = new List<string>();
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 0, "Count is not 0 from empty list");

        data = new List<string> { "Car2", "Car3", "cAR4" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 3);
        Assert.IsTrue(result[0] == Cars.Car2);
        Assert.IsTrue(result[1] == Cars.Car3);
        Assert.IsTrue(result[2] == Cars.Car4);


        data = new List<string> { "CAR4", "car3", "cAR2" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 3, "Count is not 3");
        Assert.IsTrue(result[0] == Cars.Car4, "First is not car4");
        Assert.IsTrue(result[1] == Cars.Car3);
        Assert.IsTrue(result[2] == Cars.Car2);


        data = new List<string> { null, null, null, "Car3", "Hello" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 5);
        Assert.IsTrue(result[0] == Cars.Car1);
        Assert.IsTrue(result[1] == Cars.Car1);
        Assert.IsTrue(result[2] == Cars.Car1);
        Assert.IsTrue(result[3] == Cars.Car3);
        Assert.IsTrue(result[4] == Cars.Car4);

        data = new List<string> { "Yes", "yes", null, "YES" };
        result = data.ToEnumList<Cars>();
        Assert.IsTrue(result.Count == 4, "Count is not 4");
        Assert.IsTrue(result[0] == Cars.Car2, "First is not car2");
        Assert.IsTrue(result[1] == Cars.Car2, "Second is not car1");
        Assert.IsTrue(result[2] == Cars.Car1);
        Assert.IsTrue(result[3] == Cars.Car2);
    }


    enum Cars
    {
        Car1,
        [EnumValue("Yes")]
        Car2,
        [EnumText("World")]
        Car3,
        [EnumText("Hello")]
        Car4
    }
}
