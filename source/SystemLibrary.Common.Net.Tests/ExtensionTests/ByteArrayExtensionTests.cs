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
    public void Convert_String_To_Bytes_And_Back_To_Text()
    {
        string data = null;
        var bytes = data.GetBytes();
        var result = bytes.ToText();
        Assert.IsTrue(result == null);

        data = "";
        bytes = data.GetBytes();
        result = bytes.ToText();
        Assert.IsTrue(result == "");

        data = "hello world";
        bytes = data.GetBytes();
        result = bytes.ToText();
        Assert.IsTrue(result == data);
    }

    [TestMethod]
    public void Convert_Bytes_To_Md5Hash_String()
    {
        var bytes = "".GetBytes();
        var hash = bytes.ToMD5Hash();
        Assert.IsTrue(hash == "", "Empty");

        bytes = "Hello World".GetBytes();
        hash = bytes.ToMD5Hash();
        Assert.IsTrue(hash.Length == 47, "Length " + hash.Length);

        Assert.IsTrue(hash.Replace("-", "").Length == 32);
    }

    [TestMethod]
    public void Convert_Bytes_To_Sha1Hash_String()
    {
        var bytes = "".GetBytes();
        var hash = bytes.ToSha1Hash();
        Assert.IsTrue(hash == "", "Empty");

        bytes = "Hello World".GetBytes();
        hash = bytes.ToSha1Hash();
        Assert.IsTrue(hash.Length == 59, "Length " + hash.Length);
    }

    [TestMethod]
    public void Convert_Bytes_To_Sha256Hash_String()
    {
        var bytes = "".GetBytes();
        var hash = bytes.ToSha256Hash();
        Assert.IsTrue(hash == "", "Empty");

        bytes = "Hello World".GetBytes();
        hash = bytes.ToSha256Hash();
        Assert.IsTrue(hash.Length >= 95, "Length " + hash.Length);
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
