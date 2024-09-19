﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;

using SystemLibrary.Common.Net.Attributes;
using SystemLibrary.Common.Net.Configurations;

namespace SystemLibrary.Common.Net;

/// <summary>
/// Class to load and read configuration files (xml, json or config) as a Class with transformations if exist and with decrypting encrypted properties
/// <para>Configurations can be placed in either:</para>
/// ~/*.json, ~/*.xml, ~/Configs/**.[json|xml], or ~/Configurations/**.[json|xml]
/// <para>Or appended to your existing 'appSettings.json' file</para>
/// Transformations are ran based on the 'ASPNETCORE_ENVIRONMENT' variable passed to your application
/// <para>Recommended places to set ASPNETCORE_ENVIRONMENT:</para>
/// - launchSettings.json when using IIS Express
/// <para>- web.config if you use IIS</para>
/// - mstest.runsettings if you run transformations in unit tests
/// <para>- commandline with --configuration if running as 'exe'</para>
/// </summary>
/// <remarks>
/// The Current instance on the Config object is a Singleton and only loaded once
/// <para>Read the example of 'EnvironmentConfig.Name' property, it gives details on where/how to set environment per application type</para>
/// Encrypted properties such as "ApiToken {get;set;}" can be decrypted auto by creating a "ApiTokenDecrypt {get;set;}"
/// <para>must be string, public, and marked with get;set;</para>
/// <para>convention by specifying suffix "Decrypt" or "Decrypted" or use attribute [Decrypt] on a property</para>
/// <para>Environment variables like 'UserName' is only added to 'appSettings' reads not your custom configuration files</para>
/// WARNING: The generic T cannot be a nested class
/// </remarks>
/// <example>
/// - Create new file '~/TestConfig.json'
/// - Can also be placed under ~/Configs/ or ~/Configurations/
/// <code class="language-xml hljs">
/// {
///     "Name": "Hello World",
///     "Number": 1234,
///     
///     "Options": {
///         "Url": "https://....",
///     },
///     "ValidPhoneNumbers": [0,1,2,3]
///     "password": "/HjLI26feCvCwaBOmXfu6ZFK1KLR33YbAdDZIlzuM5I=",
/// }
/// </code>
/// 
/// - Create C# class with same name as your newly created json file and inherit Config&lt;&gt;: 
/// <code class="language-csharp hljs">
/// public class TestConfig : Config&lt;TestConfig&gt; 
/// {
///    public string Name { get; set; }
///    
///    public int Number { get; set;}
/// 
///    public ApiOptions Options { get; set; }
///    
///    public int[] ValidPhoneNumbers { get; set; }
///    
///    public class Password { get; set; } // Contains encrypted value
///    public class PasswordDecrypted { get; set; } // Contains decrypted Password at runtime
/// }
/// 
/// public class ApiOptions 
/// {
///     public string Url { get; set; }
/// }
/// </code>
/// 
/// Usage:
/// <code class="language-csharp hljs">
/// var testConfig = TestConfig.Current;
/// var name = testConfig.Name;
/// // name is now Hello World
/// </code>
/// 
/// Add transformation per 'environmnet' to our newly created TestConfig.json
/// - Add transformation for an environment, lets call it 'dev'
/// - Create TestConfig.dev.json file, place it in same folder as TestConfig.json
///     - Visual Studio should mark the new file as 'IsTransformFile=true' and 'DependentUpon=TestConfig.json'
///         - If not, try dragging "TestConfig.dev.json" in under "TestConfig.json" via Solution Explorer
///         
/// - Define only variables that we want to transform:
/// <code class="language-xml hljs">
/// {
///     "Name": "Hello Dev!",
/// }
/// </code>
/// 
/// Here are three ways of specifying the environment, by either launchSettings.json, web.config or mstest.runsettings.
/// 
/// 1 launchSettings.json with IIS:
/// <code class="language-csharp hljs">
/// { 
/// 	...
/// 	{
/// 		"profiles": {
/// 			"IIS": {
/// 				"environmentVariables": {
/// 					"ASPNETCORE_ENVIRONMENT": "Dev",
/// 				}
/// 			}
/// 		}
/// 	}
/// 	...
/// }
/// </code>
/// 
/// 2 web.config with IIS or IISExpress: 
/// <code class="language-csharp hljs">
/// &lt;configuration&gt;
///   &lt;location path = "." inheritInChildApplications="false"&gt;
/// 	&lt;/system.webServer&gt;
/// 	  &lt;aspNetCore processPath = "bin\Demo.exe" arguments="" stdoutLogEnabled="false" hostingModel="inprocess"&gt;
///         &lt;environmentVariables&gt;
///           &lt;environmentVariable name = "ASPNETCORE_ENVIRONMENT" value="Dev" /&gt;
///         &lt;/environmentVariables&gt;
///       &lt;/aspNetCore&gt;
///     &lt;/system.webServer&gt;
///   &lt;/location&gt;
/// &lt;/configuration&gt;
/// </code>
/// 
/// 3 mstest.runsettings if running through Test Explorer (unit tests):
/// - Note: add mstest.runsettings to your csproj-variable: 'RunSettingsFilePath'
/// - Tip: View source code of SystemLibrary.Common.Net.Tests inside the repo SystemLibrary.Common.Net on github
/// <code class="language-csharp hljs">
/// &lt;RunSettings&gt;
///   &lt;RunConfiguration&gt;
///       &lt;EnvironmentVariables&gt;
///           &lt;ASPNETCORE_ENVIRONMENT&gt;Dev&lt;/ASPNETCORE_ENVIRONMENT&gt;
/// </code>
/// 
/// Usage:
/// - Assume IISExpress and web.config setup above:
/// <code class="language-csharp hljs">
/// var testConfig = TestConfig.Current;
/// var name = testConfig.Name;
/// // name is now equal to 'Hello Dev!', which is our transformed value
/// </code>
/// </example>
/// <typeparam name="T">T is the class inheriting Config&lt;&gt;, also referenced as 'self'. Note that T cannot be a nested class</typeparam>
public abstract partial class Config<T> where T : class
{
    static T _Config = default;

    static Config()
    {
        var configLoader = ConfigLoader<T>.Load();
        try
        {
            _Config = configLoader.Get<T>();
        }
        catch
        {
            // NOTE: static properties inside the Config class errors unless already instantiated
            // could check for static members myself, but try-catch for now
            _Config = Activator.CreateInstance<T>();
            _Config = configLoader.Get<T>();
        }

        if (_Config == null && typeof(T) == typeof(EnvironmentConfig))
            throw new Exception("EnvironmentConfig could not be created - make sure the 'environmentConfig.json' is not empty, it must minimum contain one property, for instance 'name' set to some value like 'prod'.");

        try
        {
            DecryptPublicGetSetProperties(_Config, typeof(T));
        }
        catch (Exception ex)
        {
            Debug.Log("Decrypt configuration failed: " + ex.Message);
        }
    }

    /// <summary>
    /// Get the current configuration as a singleton object, always instantiated
    /// </summary>
    public static T Current => _Config;

    static void DecryptPublicGetSetProperties(object instance, Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)?.Where(prop => prop.PropertyType == SystemType.StringType);

        if (properties == null) return;

        foreach (var property in properties)
        {
            if (property == null) continue;

            if (!property.CanWrite || !property.CanRead) continue;

            var isEligibleForDecryption = property.Name.EndsWith("Decrypted", StringComparison.Ordinal) || property.Name.EndsWith("Decrypt", StringComparison.Ordinal);

            var attribute = property.GetCustomAttribute<DecryptAttribute>();

            // Not a decrypt property
            if (!isEligibleForDecryption && attribute == null) continue;

            // Name convention mismatch, but propertyName not specified, not able to decrypt
            if (!isEligibleForDecryption && attribute.PropertyName.IsNot())
            {
                Debug.Log("Config did not decrypt " + property.Name + " as DecryptAttribute 'PropertyName' is null/blank");
                continue;
            }

            var encryptedPropertyName = attribute?.PropertyName ??
                property.Name.ReplaceAllWith("", "Decrypted", "Decrypt");

            var encryptedProperty = FindEncryptedProperty(properties, encryptedPropertyName);

            if (encryptedProperty == null)
            {
                Debug.Log("Config did not decrypt " + property.Name + " as the encrypted property was not found: " + encryptedPropertyName);
                continue;
            }

            var cipherText = encryptedProperty.GetValue(instance) as string;

            if (cipherText == null)
            {
                Debug.Log("Config property " + encryptedProperty.Name + " is null, decrypting nothing, continue...");
                continue;
            }

            var decryptedValue = cipherText.Decrypt();

            if (decryptedValue != null)
            {
                property.SetValue(instance, decryptedValue);
            }
            else
            {
                Debug.Log("Decrypting " + encryptedProperty.Name + " returned null");
            }
        }
    }

    static PropertyInfo FindEncryptedProperty(IEnumerable<PropertyInfo> properties, string encryptedPropertyName)
    {
        return properties.FirstOrDefault(x => x.Name == encryptedPropertyName);
    }
}