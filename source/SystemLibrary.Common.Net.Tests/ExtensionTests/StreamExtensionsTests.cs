using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class StreamExtensionsTests
{
    [TestMethod]
    public void Stream_To_Md5_Hash_String()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        using (MemoryStream memory = new MemoryStream())
        {
            using (var writer = new StreamWriter(memory))
            {
                writer.Write(data);
                memory.Seek(0, SeekOrigin.Begin);

                var result = memory.ToMD5Hash();
                Assert.IsTrue(result != null);
                Assert.IsTrue(result.Length == 47, "Md5Hash Length: " + result.Length);
            }
        }
    }

    [TestMethod]
    public void Stream_To_Md5_Hash_Async_String()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        using (MemoryStream memory = new MemoryStream())
        {
            using (var writer = new StreamWriter(memory))
            {
                writer.Write(data);
                memory.Seek(0, SeekOrigin.Begin);

                var result = memory.ToMD5HashAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                Assert.IsTrue(result != null);
                Assert.IsTrue(result.Length == 47, "Md5Hash Length: " + result.Length);
            }
        }
    }

    [TestMethod]
    public void Stream_To_Sha1_Hash_String()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        using (MemoryStream memory = new MemoryStream())
        {
            using (var writer = new StreamWriter(memory))
            {
                writer.Write(data);
                memory.Seek(0, SeekOrigin.Begin);

                var result = memory.ToSha1Hash();
                Assert.IsTrue(result != null);
                Assert.IsTrue(result.Length == 59, "Sha1 Length: " + result.Length);
            }
        }
    }

    [TestMethod]
    public void Stream_To_Sha1_Hash_Async_String()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        using (MemoryStream memory = new MemoryStream())
        {
            using (var writer = new StreamWriter(memory))
            {
                writer.Write(data);
                memory.Seek(0, SeekOrigin.Begin);

                var result = memory.ToSha1HashAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                Assert.IsTrue(result != null);
                Assert.IsTrue(result.Length == 59, "Sha1 Length: " + result.Length);
            }
        }
    }

    [TestMethod]
    public void Stream_To_Sha256_Hash_String()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        using (MemoryStream memory = new MemoryStream())
        {
            using (var writer = new StreamWriter(memory))
            {
                writer.Write(data);
                memory.Seek(0, SeekOrigin.Begin);

                var result = memory.ToSha256Hash();
                Assert.IsTrue(result != null);
                Assert.IsTrue(result.Length == 95, "Sha256 Length: " + result.Length);
            }
        }
    }

    [TestMethod]
    public void Stream_To_Sha256_Hash_Async_String()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        using (MemoryStream memory = new MemoryStream())
        {
            using (var writer = new StreamWriter(memory))
            {
                writer.Write(data);
                memory.Seek(0, SeekOrigin.Begin);

                var result = memory.ToSha256HashAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                Assert.IsTrue(result != null);
                Assert.IsTrue(result.Length == 95, "Sha256 Length: " + result.Length);
            }
        }
    }

    [TestMethod]
    public void Reading_Stream_To_JsonAsync_Success()
    {
        var data = Assemblies.GetEmbeddedResource("JsonSerializationDefaultOptionsTests", "Data.json");

        using (MemoryStream memory = new MemoryStream())
        {
            using (var writer = new StreamWriter(memory))
            {
                writer.Write(data);
                writer.Flush();
                memory.Seek(0, SeekOrigin.Begin);

                var response = memory.JsonAsync<Data>().GetAwaiter().GetResult();

                Assert.IsTrue(data != null);

                Assert.IsTrue(response.IsSuccess, "!IsSuccess");

                Assert.IsTrue(response.SubClass != null, "Subclass");
                Assert.IsTrue(response.SubClass.CarEnumAsText == Product.Car3, "CarEnumAsText");

                Assert.IsTrue(response.IntAsStringProperty.Length > 4, "IntAsStringProperty.Length");
                Assert.IsTrue(response.StringProperty == "stringProp", "StringProperty");
                Assert.IsTrue(response.CarEnumAsText == Product.Car3, "CarEnumAsText");
                Assert.IsTrue(response.CarEnumAsNumber == Product.Car3, "CarEnumAsNumber");
                Assert.IsTrue(response.ListOfTextEnums.Count > 0, "ListOfTextEnums.Count");
                Assert.IsTrue(response.ListOfTextEnums[1] == Product.Car4, "ListOfTextEnums[1]");
            }
        }
    }
}
