using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests;

[TestClass]
public class RandomnessTests
{
    [TestMethod]
    public void Get_Next_Int()
    {
        var number = Randomness.Int();

        var number2 = Randomness.Int();

        Assert.IsTrue(number > 0 && number2 > 0);

        Assert.IsTrue(number - number2 > 1 || number - number2 < -1);

        var number3 = Randomness.Int(2);
        var number4 = Randomness.Int(2);
        var number5 = Randomness.Int(2);
        Assert.IsTrue(number3 < 3 && number4 < 3 && number5 < 3);
    }

    [TestMethod]
    public void Get_Next_String()
    {
        var txt = Randomness.String();
        var txt2 = Randomness.String();

        Assert.IsTrue(txt.Length == 6 && txt2.Length == 6);

        Assert.IsTrue(txt != txt2);
    }

    [TestMethod]
    public void Get_Next_Bytes()
    {
        var bytes = Randomness.Bytes();

        Assert.IsTrue(bytes.Length == 16);
        Assert.IsTrue(bytes[3] > 0 || bytes[0] > 0);
        Assert.IsTrue(bytes[15] > 0 || bytes[14] > 0);
    }

}