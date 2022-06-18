using Microsoft.Extensions.Configuration;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class for creating configurations (xml or json) files next to your code files
    /// 
    /// Configurations must be placed in either:
    /// ~/*.json, ~/*.xml, ~/Configs/**.[json|xml], or ~/Configurations/**.[json|xml]
    /// 
    /// Configurations can also be appended to 'appSettings.json' if you do not want Configs/Configurations folder in root 
    /// 
    /// Transformations are ran by setting 'ASPNETCORE_ENVIRONMENT' in launchSettings.json or web.config, or in mstest.runsettings
    /// - launchSettings.json when using IIS Express
    /// - web.config if you use IIS locally, or IIS in a server environment
    /// - mstest.runsettings if you run transformations in unit tests
    /// 
    /// If 'ASPNETCORE_ENVIRONMENT' variable is defined in launchSettings and web.config, the one in launchSettings overwrites the value in web.config, if you run IIS Express at least
    /// 
    /// If no 'ASPNETCORE_ENVIRONMENT is specified it will transform based on 'Configuration Mode' your code was built with: 'Release' or 'Debug' only
    /// 
    /// WARNING: Bug in Microsoft's code, the call "UseEnvironment()" wont transform based on the argument
    /// 
    /// WARNING: Requires app restart if configuration changes
    /// </summary>
    /// <example>
    /// Let's add our own custom configuration file, named 'TestConfig.json', without any transformations yet:
    /// - Place the file in either ~/, ~/Configs/ or ~/Configurations/
    /// 
    /// <code class="language-xml hljs">
    /// {
    ///     "Name": "Hello World",
    ///     "Number": 1234,
    ///     
    ///     //"Options" refers to name of the property in 'TestConfig' class, not the type in your C# which would be 'ApiOptions'
    ///     "Options": {
    ///         "Url": "https://....",
    ///     },
    ///     
    ///     "ValidPhoneNumbers": [0,1,2,3]
    /// }
    /// </code>
    /// 
    /// Create new class with same name as the json file, and inherit Config&lt;&gt;:
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
    /// Use your configurations in json through the C# class, at runtime:
    /// <code class="language-csharp hljs">
    /// var testConfig = TestConfig.Current;
    /// var name = testConfig.Name;
    /// //name is now Hello World
    /// </code>
    /// 
    /// Let's add transformation file to our newly created TestConfig.json, for 'dev' environment:
    /// 
    /// - create new file 'TestConfig.dev.json' and place it in the same folder as TestConfig.json
    /// 
    /// - visual studio should mark TestConfig.dev.json as IsTransformFile=true and DependentUpon 'TestConfig.json'
    /// - if not you can try dragging 'TestConfig.dev.json' in under 'TestConfig.json' in Solution Explorer
    /// - only variables we want to transform are required
    /// <code class="language-xml hljs">
    /// {
    ///     "Name": "Hello Dev!",
    /// }
    /// </code>
    /// 
    /// 
    /// Three ways of specifying the environment, in launchSettings, web.config and in mstest.runsettings, which then all configurations which inherits Config&lt;&gt; is ran if they do have transformation files:
    /// 
    /// 1 launchSettings.json:
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
    /// 2 web.config: 
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
    /// 3 mstest.runsettings:
    /// - Note: add mstest.runsettings to your csproj-variable: 'RunSettingsFilePath'
    /// - Tip: View source code of SystemLibrary.Common.Net.Tests inside the repo SystemLibrary.Common.Net on github
    /// <code class="language-csharp hljs">
    /// &lt;RunSettings&gt;
    ///   &lt;RunConfiguration&gt;
    ///       &lt;EnvironmentVariables&gt;
    ///           &lt;ASPNETCORE_ENVIRONMENT&gt;Dev&lt;/ASPNETCORE_ENVIRONMENT&gt;
    /// </code>
    /// 
    /// Use our transformations
    /// 
    /// Note: transformations are ran the first time '.Current' on a config is invoked
    /// <code class="language-csharp hljs">
    /// var testConfig = TestConfig.Current;
    /// var name = testConfig.Name;
    /// //name is now equal to 'Hello Dev!', our transformed configuration
    /// </code>
    /// </example>
    /// <typeparam name="T">T is the class inheriting Config&lt;&gt;, also referenced as 'self'. Note that T cannot be a nested class</typeparam>
    public abstract partial class Config<T> where T : class
    {
        static bool IsInitialized = false;

        static T _Config = default;

        /// <summary>
        /// Get the current configuration as a Singleton object
        /// </summary>
        public static T Current
        {
            get
            {
                if (!IsInitialized)
                {
                    IsInitialized = true;

                    if (_Config != null) return _Config;

                    _Config = ConfigLoader<T>.Load()?.Get<T>();
                }
                return _Config;
            }
        }
    }
}