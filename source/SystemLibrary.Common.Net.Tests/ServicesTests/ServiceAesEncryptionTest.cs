using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.XServiceTestsRunsLast;
internal class IsInternal
{
    public string Test;
}

[TestClass]
public class ServiceAesEncryptionTest
{
    [TestInitialize]
    public void TestInitialize()
    {
        var typeKey = Type.GetType("SystemLibrary.Common.Net.CryptationKey, SystemLibrary.Common.Net");

        typeKey.SetStaticMember("_Key", null);
    }

    [TestMethod]
    public void Test_Create_Default_Instance()
    {
        var type = typeof(string);
        var res = type.Default();

        Assert.IsTrue(res == null);

        type = typeof(int);
        res = type.Default();
        Assert.IsTrue((int)res == 0);

        type = typeof(bool);
        res = type.Default();
        Assert.IsTrue((bool)res == false);

        type = typeof(List<string>);
        res = type.Default();
        Assert.IsTrue(res == null);

        type = typeof(List<int>);
        res = type.Default();
        Assert.IsTrue(res == null);

        type = typeof(IList<string>);
        res = type.Default();
        Assert.IsTrue(res == null);

        type = typeof(DateTime);
        res = type.Default();
        Assert.IsTrue((DateTime)res == DateTime.MinValue);

        type = typeof(DateTimeOffset);
        res = type.Default();
        Assert.IsTrue((DateTimeOffset)res == DateTimeOffset.MinValue);
    }

    [TestMethod]
    public void Encrypting_Without_Data_Protection_Success()
    {
        var serviceCollection = new ServiceCollection();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceCollection);

        Services.Configure(serviceProvider);

        var data = "Hello world";
        var enc = data.Encrypt();
        var prev = "dsDc7602l6cz7P6iSrK+png/gfC8OItoL85WPenGNf8=";

        Assert.IsTrue(prev.Length == enc.Length, "Enc with key file as Key changed: " + enc);

        var dec = enc.Decrypt();

        Assert.IsTrue(dec == data, "Decrypt has changed");
    }

    [TestMethod]
    public void AddDataProtection_KeysToFileSystem_With_AppName_Uses_KeyFile()
    {
        var serviceCollection = new ServiceCollection();

        var appName = "AppName" +
               Assembly.GetEntryAssembly()?
               .GetName()?
               .Name?
               .ToLower()?
               .ReplaceAllWith("-", ",", ".", " ", "=", "/", "\\")?
               .MaxLength(32) +
               Assembly.GetCallingAssembly()?
               .GetName()?
               .Name?
               .ToLower()
               .ReplaceAllWith("-", ",", ".", " ", "=", "/", "\\")?
               .MaxLength(4);

        serviceCollection.AddDataProtection()
              .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Temp\"))
              .SetDefaultKeyLifetime(TimeSpan.FromDays(365 * 1000))
              .SetApplicationName(appName);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceCollection);
        Services.Configure(serviceProvider);
        var data = "Hello world";
        var enc = data.Encrypt();
        var prev = "dsDc7602l6cz7P6iSrK+png/gfC8OItoL85WPenGNf8=";
        Assert.IsTrue(prev.Length == enc.Length, "Enc with key file as Key changed: " + enc);

        var dec = enc.Decrypt();

        Assert.IsTrue(dec == data, "Decrypt has changed");
    }

    [TestMethod]
    public void AddDataProtection_KeysToFileSystem_Without_AppName_Uses_KeyFile()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDataProtection()
              .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Temp\"))
              .SetDefaultKeyLifetime(TimeSpan.FromDays(365 * 1000));

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceCollection);
        Services.Configure(serviceProvider);

        var data = "Hello world";
        var enc = data.Encrypt();
        var prev = "dsDc7602l6cz7P6iSrK+png/gfC8OItoL85WPenGNf8=";
        Assert.IsTrue(prev.Length == enc.Length, "Enc with key file as Key changed: " + enc);

        var dec = enc.Decrypt();

        Assert.IsTrue(dec == data, "Decrypt has changed");
    }

    [TestMethod]
    public void Add_Service_WithDataProtection_Encrypt_With_AppName()
    {
        var serviceCollection = new ServiceCollection();

        var appName = "CustomApplicationNameAsKey";

        serviceCollection.AddDataProtection()
            .SetApplicationName(appName);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceCollection);
        Services.Configure(serviceProvider);

        var data = "Hello world";
        var enc = data.Encrypt();
        var prev = "dsDc7602l6cz7P6iSrK+png/gfC8OItoL85WPenGNf8=";
        Assert.IsTrue(prev.Length == enc.Length, "Enc with key file as Key changed: " + enc);

        var dec = enc.Decrypt();

        Assert.IsTrue(dec == data, "Decrypt has changed");
    }

    [TestMethod]
    public void Add_Service_WithDataProtection_Encrypt_Using_BuiltIn_AsmName()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDataProtection();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceCollection);
        Services.Configure(serviceProvider);

        var data = "Hello world";
        var enc = data.Encrypt();
        var prev = "dsDc7602l6cz7P6iSrK+png/gfC8OItoL85WPenGNf8=";
        Assert.IsTrue(prev.Length == enc.Length, "Enc with key file as Key changed: " + enc);

        var dec = enc.Decrypt();

        Assert.IsTrue(dec == data, "Decrypt has changed");
    }


    [TestMethod]
    public void Encrypt_And_Decrypt_Using_DataProtection_Key_File_Success()
    {
        System.Threading.Thread.Sleep(2000);
        var serviceCollection = Services.Configure();

        serviceCollection.AddDataProtection()
            .DisableAutomaticKeyGeneration()
            .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Temp\"))
            .SetDefaultKeyLifetime(TimeSpan.FromDays(365 * 1000))
            .SetApplicationName("TestApp");

        Services.Configure(serviceCollection.BuildServiceProvider());

        var data = "Hello world";
        var enc = data.Encrypt();
        Assert.IsTrue(enc != data && enc.Length > data.Length + 10);

        var dec = enc.Decrypt();
        Assert.IsTrue(dec == data, "Wrong decryption");

        enc = data.EncryptUsingKeyRing();
        Assert.IsTrue(enc != data && enc.Length > data.Length + 10);

        dec = enc.DecryptUsingKeyRing();
        Assert.IsTrue(dec == data, "Wrong decryption");
    }

    [TestMethod]
    public void Encrypt_And_Decrypt_Using_AppName()
    {
        System.Threading.Thread.Sleep(1000);
        var serviceCollection = Services.Configure();

        serviceCollection.AddDataProtection()
            .DisableAutomaticKeyGeneration()
            .SetApplicationName("HelloWorld");

        Services.Configure(serviceCollection.BuildServiceProvider());

        var data = "Hello world";
        var enc = data.Encrypt();
        Assert.IsTrue(enc != data && enc.Length > data.Length + 10);

        var dec = enc.Decrypt();
        Assert.IsTrue(dec == data, "Wrong decryption");

        enc = data.EncryptUsingKeyRing();
        Assert.IsTrue(enc != data && enc.Length > data.Length + 10);

        dec = enc.DecryptUsingKeyRing();
        Assert.IsTrue(dec == data, "Wrong decryption");
    }

    [TestMethod]
    public void Encrypt_And_Decrypt_Using_Auto_Generated_AppName()
    {
        System.Threading.Thread.Sleep(1000);
        var serviceCollection = Services.Configure();

        serviceCollection.AddDataProtection()
            .DisableAutomaticKeyGeneration();

        Services.Configure(serviceCollection.BuildServiceProvider());

        var data = "Hello world";
        var enc = data.Encrypt();
        Assert.IsTrue(enc != data && enc.Length > data.Length + 10);

        var dec = enc.Decrypt();
        Assert.IsTrue(dec == data, "Wrong decryption");

        enc = data.EncryptUsingKeyRing();
        Assert.IsTrue(enc != data && enc.Length > data.Length + 10);

        dec = enc.DecryptUsingKeyRing();
        Assert.IsTrue(dec == data, "Wrong decryption");
    }
}

