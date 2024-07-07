using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests;

[TestClass]
public class ServicesTests
{
    [TestMethod]
    public void Test()
    {
        var serviceCollection = new ServiceCollection();

        var appName = "AppName" + 
            Assembly.GetEntryAssembly()?
            .GetName()?
            .Name?
            .ToLower()?
            .ReplaceAllWith("-", ",", ".", " ", "=", "/", "\\")?
            .MaxLength(16);

        serviceCollection.AddDataProtection()
              .DisableAutomaticKeyGeneration()
              .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Temp\"))
              .SetDefaultKeyLifetime(TimeSpan.FromDays(365 * 1000))
              .SetApplicationName(appName);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        Services.Configure(serviceCollection);
        Services.Configure(serviceProvider);

        var data = "Hello world";
        var enc = data.Encrypt();

        Assert.IsTrue("uZSuoc+BRpaATu9LmkJa+g==" == enc, "Enc with key file as Key changed: " + enc);

        var dec = enc.Decrypt();

        Assert.IsTrue(dec == data, "Decrypt has changed");

    }
}
