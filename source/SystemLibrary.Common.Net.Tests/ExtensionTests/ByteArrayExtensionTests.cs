using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class ByteArrayExtensionTests
{
    [TestMethod]
    public void GetBytes_Test_Success()
    {
        string text = null;
        byte[] bytes = null;

        bytes = text.GetBytes();

        Assert.IsTrue(bytes == null);

        text = "";
        bytes = text.GetBytes();
        Assert.IsTrue(bytes != null);
        Assert.IsTrue(bytes.Length == 0);

        text = "Hello world";
        bytes = text.GetBytes();

        Assert.IsTrue(text.Length == bytes.Length);
    }
}
