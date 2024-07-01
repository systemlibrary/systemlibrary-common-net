using System;
using System.Collections.Generic;

using Asm = System.Reflection.Assembly;

namespace SystemLibrary.Common.Net;

/// <summary>
/// Class to find types and embedded resources in loaded Assemblies
/// 
/// Built on System.Reflection.Assembly
/// </summary>
public static partial class Assemblies
{
    static string[] BlacklistedAssemblyNames => new string[]
    {
        "Microsoft",
        "Docfx",
        "Windows",
        "MSBuild",
        "System.",
        "Castle.",
        "Owin",
        "StructureMap",
        "EntityFramework",
        "EPiServer",
        "Umbraco",
        "Newtonsoft",
        "Swashbuckle",
        "RestSharp",
        "GraphQL",
        "log4net",
        "MSTest",
        "VSTest",
        "Serilog",
        "nlog",
        "ElasticSearch",
        "Elasticsearch",
        "Remotion",
        "YamlDotNet",
        "Antlr",
        "ClearScript",
        "nunit",
        "AWS",
        "SharpZipLib",
        "HtmlAgilityPack",
        "Azure.",
        "JavaScriptEngineSwitcher",
        "NuGet",
        "Salesforce",
        "React",
        "moq",
        "Moq",
        "automapper",
        "AutoMapper",
        "Autofac",
        "Dapper",
        "SystemLibrary.Common.Net",
        "SystemLibrary.Common.Web",
        "testhost",
        "netstandard",
        "Anonymously Hosted",
        "DynamicContentModelsAssembly",
        "nunit.",
        "xunit.",
        "Polly.",
        "runtime.win",
        "FluentValidation.",
        "FluentAssertions.",
        "StackExchange.",
        "AutoFixture.",
        "Modernizr.",
        "DocumentFormat.OpenXml",
        "NLog.",
        "IdentityModel.",
        "coverlet.",
        "MediatR.",
        "StyleCop.",
        "Hangfire.",
        "Pipelines.",
        "NUnit3TestAdapter.",
        "Npgsql.",
        "Humanizer.Core",
        "NSubstitute",
        "NJsonSchema",
        "bootstrap",
        "SendGrid",
        "Portable.BouncyCastle.",
        "RabbitMQ.",
        "SQLitePCLRaw.",
        "CsvHelper.",
        "Elasticsearch.",
        "MongoDB.",
        "WebGrease.",
        "Google.",
        "jQuery.",
        "SharpCompress.",
        "JetBrains.",
        "NodaTime.",
        "Selenium.",
        "CommandLineParser.",
        "SendGrid.",
        "WebActivatorEx.",
        "MessagePack",
        "MailKit",
        "protobuf-net.",
        "Unity.",
        "MySql.Data.",
        "Xamarin.",
        "CommonServiceLocator.",
        "NuGet.Packaging.",
        "IdentityServer4.",
        "FluentEmail",
        "Grpc."

    };

    static IEnumerable<Asm> WhiteListedAssemblies { get; }

    /// <summary>
    /// Find all types inheriting class T in all loaded assemblies
    /// 
    /// Skips searching in assemblies starting with common names like: Microsoft, System, Windows, EntityFramework, AWS, Serilog, MSTest, nunit, Newtonsoft, Xamarin, Dapper, Autofac, Automapper, Salesforce, ...
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// public class Car : IVehicle {
    /// }
    /// 
    /// var vehicles = Assemblies.FindAllTypesInheriting&lt;IVehicle&gt;
    /// // returns 'Car' and all other types that inherits/implements IVehicle
    /// </code>
    /// </example>
    /// <typeparam name="TClassType">Must be a class</typeparam>
    /// <returns>An IEnumerable of Type</returns>
    public static IEnumerable<Type> FindAllTypesInheriting<TClassType>() where TClassType : class
    {
        return FindTypesInheriting(typeof(TClassType));
    }

    /// <summary>
    /// Find all types inheriting class T with a certain attribute in all loaded assemblies
    /// 
    /// Skips searching in assemblies starting with common names like: Microsoft, System, Windows, EntityFramework, AWS, Serilog, MSTest, nunit, Newtonsoft, Dapper, Autofac, Automapper, Salesforce, ...
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// public class NameAttribute : Attribute { 
    /// }
    /// 
    /// [NameAttribute]
    /// public class Car : IVehicle {
    /// }
    /// 
    /// var vehicles = Assemblies.FindAllTypesInheriting&lt;IVehicle,NameAttribute&gt;
    /// // returns 'Car' and all other types that inherits/implements IVehicle which also contains the attribute
    /// </code>
    /// </example>
    /// <typeparam name="TClassType">Must be a class</typeparam>
    /// <typeparam name="TAttributeType">Type of class that inherits Attribute</typeparam>
    /// <returns>An IEnumerable of Type</returns>
    public static IEnumerable<Type> FindAllTypesInheritingWithAttribute<TClassType, TAttributeType>()
        where TClassType : class
        where TAttributeType : Attribute
    {
        return FindTypesInheriting(typeof(TClassType), typeof(TAttributeType));
    }

    /// <summary>
    /// Read an embedded resource's content as string
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var text = Assemblies.GetEmbeddedResource("Folder/SubFolder/SubFolder2", "json.txt");
    /// Assert.IsTrue(text.Contains("hello world"));
    /// // assume a file in Solution Explorer exists at "~/Folder/SubFolder/SubFolder2/json.txt"
    /// // assume "json.txt" has build action 'Embedded Resource'
    /// // assume "json.txt" contains 'hello world' this is now true
    /// </code>
    /// </example>
    public static string GetEmbeddedResource(string relativeFolderPath, string fileName, Asm assembly = null)
    {
        return ReadEmbeddedResourceAsString(relativeFolderPath, fileName, assembly ?? Asm.GetCallingAssembly());
    }

    /// <summary>
    /// Read an embedded resource's content as byte array
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// var bytes = Assemblies.GetEmbeddedResource("Folder/SubFolder/SubFolder2", "image.jpg");
    /// // bytes contains now the whole image.jpg as an byte array, assuming image.jpg was marked with the build action 'Embedded Resource'
    /// </code>
    /// </example>
    public static byte[] GetEmbeddedResourceAsBytes(string relativeFolderPath, string fileName, Asm assembly = null)
    {
        return ReadEmbeddedResourceAsBytes(relativeFolderPath, fileName, assembly ?? Asm.GetCallingAssembly());
    }
}