using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class ReadOnlySpanExtensionTests
{
    [TestMethod]
    public void StringBase64_Matches_ReadOnlySpanChar_And_Substring_Base64()
    {
        var data = "hello world";
        var datab64 = data.ToBase64();

        var span = data.AsSpan();
        var span64 = span.ToBase64();

        var sub = data.Substring(0);
        var sub64 = sub.ToBase64();

        Assert.IsTrue(datab64 == sub64 && sub64 == span64);
    }

    [TestMethod]
    public void String_To_Span_GetBytes()
    {
        var data = "hello world";
        var bytes = data.GetBytes();

        Assert.IsTrue(bytes.Length == 11);
        Assert.IsTrue(bytes[0] > 0);
    }
}
