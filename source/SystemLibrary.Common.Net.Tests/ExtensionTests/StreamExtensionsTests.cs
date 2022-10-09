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
                Assert.IsTrue(result.Length == 47, "Md5Hash Length");
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

                var response = memory.ToJsonAsync<Data>().GetAwaiter().GetResult();

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
