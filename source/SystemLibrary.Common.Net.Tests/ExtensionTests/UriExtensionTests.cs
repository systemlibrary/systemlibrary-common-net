using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests;

[TestClass]
public class UriExtensionTests
{
    [TestMethod]
    public void Uri_Get_Primary_Domain()
    {
        Uri uri = null;

        var primaryDomain = uri.GetPrimaryDomain();

        Assert.IsTrue(primaryDomain == "");
    }

    [TestMethod]
    public void Uri_Get_Localhost_As_Primary_Domain()
    {
        var uri = new Uri("http://localhost.no");

        var primaryDomain = uri.GetPrimaryDomain();

        Assert.IsTrue(primaryDomain == "localhost.no");
    }

    [TestMethod]
    public void Uri_Get_Localhost_As_Primray_Domain_When_Subdomain()
    {
        var uri = new Uri("http://www.domain1.domain2.localhost.no");

        var primaryDomain = uri.GetPrimaryDomain();

        Assert.IsTrue(primaryDomain == "localhost.no");
    }

    [TestMethod]
    public void Uri_Get_Localhost_As_Primary_Domain_When_No_Language_Part()
    {
        var uri = new Uri("http://localhost");

        var primaryDomain = uri.GetPrimaryDomain();

        Assert.IsTrue(primaryDomain == "localhost.com");
    }

    [TestMethod]
    public void Uri_Get_Sub_Domains_Without_Protocol()
    {
        var uri = new Uri("system.library", UriKind.Relative);

        var primaryDomain = uri.GetPrimaryDomain();

        Assert.IsTrue(primaryDomain == "library.com");
    }

    [TestMethod]
    public void Uri_Get_Domain_With_Valid_Domainl()
    {
        var uri = new Uri("system.demo", UriKind.Relative);

        var primaryDomain = uri.GetPrimaryDomain();

        Assert.IsTrue(primaryDomain == "system.demo");
    }

    [TestMethod]
    public void Uri_Get_Sub_Domains_With_Valid_Domainl()
    {
        var uri = new Uri("system.library.no", UriKind.Relative);

        var primaryDomain = uri.GetPrimaryDomain();

        Assert.IsTrue(primaryDomain == "library.no");
    }
}
