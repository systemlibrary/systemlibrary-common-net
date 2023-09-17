using System;

using Microsoft.Extensions.Configuration;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class for creating configurations (xml or json) files next to your code files
    /// 
    /// Configurations must be placed in either:
    /// ~/*.json, ~/*.xml, ~/Configs/**.[json|xml], or ~/Configurations/**.[json|xml]
    /// 
    /// Configurations can also be appended to 'appSettings.json' if you do not want Configs/Configurations folder in root, but careful about naming clashes then
    /// 
    /// Transformations are ran by passing 'ASPNETCORE_ENVIRONMENT' to your application on startup:
    /// - launchSettings.json when using IIS Express
    /// - web.config if you use IIS
    /// - mstest.runsettings if you run transformations in unit tests
    /// - commandline with --configuration if running as 'exe'
    /// 
    /// NOTE: Read the example of 'EnvironmentConfig.Name' property, it gives details on where/how to set environment per application type
    /// 
    /// NOTE: Transformations are only ran on the first time '.Current' is invoked
    /// 
    /// NOTE: Environment variables like 'UserName', is added only to 'appSettings' and not your custom configurations like: ~/Configs/testConfig.json, which mean 'UserName' always exists in 'AppSettings' if you create the string variable and returns the user name on your computer
    /// 
    /// WARNING: The generic T cannot be a nested class
    /// </summary>
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
    ///     
    ///     "ValidPhoneNumbers": [0,1,2,3]
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
                // NOTE: Static properties inside the Config class errors unless already instantiated
                // could check for static members myself, but try-catch for now
                _Config = Activator.CreateInstance<T>();
                _Config = configLoader.Get<T>();
            }

            if (_Config == null && typeof(T) == typeof(EnvironmentConfig))
                throw new System.Exception("EnvironmentConfig could not be created - make sure the 'environmentConfig.json' is not empty, it must minimum contain one property, for instance 'name' set to some value like 'prod'.");
        }

        /// <summary>
        /// Get the current configuration as a Singleton object
        /// </summary>
        public static T Current => _Config;
    }
}