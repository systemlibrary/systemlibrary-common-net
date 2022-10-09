using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class ByteArrayExtensionTests
{
    [TestMethod]
    public void Convert_Bytes_To_Base64_String()
    {
        var data = "hello world";

        var bytes = data.GetBytes();

        var result = bytes.ToBase64();
        Assert.IsTrue(result.Length >= data.Length);
        Assert.IsTrue(result.EndsWith("="));
    }
}
