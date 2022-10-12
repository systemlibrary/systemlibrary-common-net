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

    [TestMethod]
    public void Convert_Bytes_To_Md5Hash_String()
    {
        var bytes = "".GetBytes();
        var hash = bytes.ToMD5Hash();
        Assert.IsTrue(hash == "", "Empty");

        bytes = "Hello World".GetBytes();
        hash = bytes.ToMD5Hash();
        Assert.IsTrue(hash.Length >= 32, "Length");
    }

    [TestMethod]
    public void Convert_Bytes_To_Sha1Hash_String()
    {
        var bytes = "".GetBytes();
        var hash = bytes.ToSha1Hash();
        Assert.IsTrue(hash == "", "Empty");

        bytes = "Hello World".GetBytes();
        hash = bytes.ToSha1Hash();
        Assert.IsTrue(hash.Length >= 59, "Length");
    }

    [TestMethod]
    public void Convert_Bytes_To_Obfuscate_String()
    {
        var bytes = "".GetBytes();
        var hash = bytes.ToSha1Hash();
        Assert.IsTrue(hash == "", "Empty");

        bytes = "Hello World".GetBytes();
        hash = bytes.ToSha1Hash();
        Assert.IsTrue(hash.Length >= 59, "Length");
    }
}
