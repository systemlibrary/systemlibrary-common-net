using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public partial class StringExtensionsDPApiTests
{
    [TestInitialize]
    public void TestInitialize()
    {
        var typeKey = Type.GetType("SystemLibrary.Common.Net.CryptationKey, SystemLibrary.Common.Net");

        typeKey.SetStaticMember("_Key", null);
    }

    [TestMethod]
    public void Encrypt_And_Decrypt_Using_DataProtection_API_Success()
    {
        var serviceCollection = Services.Configure();

        serviceCollection.AddDataProtection();

        Services.Configure(serviceCollection.BuildServiceProvider());

        var data = "Hello world";

        var enc = data.EncryptUsingKeyRing();

        Assert.IsTrue(enc != data && enc.Length > data.Length + 10);

        var dec = enc.DecryptUsingKeyRing();

        Assert.IsTrue(dec == data, "Wrong decryption");
    }
}

