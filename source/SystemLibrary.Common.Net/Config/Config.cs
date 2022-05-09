using Microsoft.Extensions.Configuration;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class for creating configurations (xml or json) files next to your code files
    /// 
    /// Configurations must be placed in either:
    /// ~/*.json, ~/*.xml, ~/Configs/**.[json|xml], ~/Configurations/**.[json|xml]
    /// 
    /// Configurations can also be appended to 'appSettings.json' if you do not want Configs/Configurations folder in root 
    /// 
    /// Transformations are ran by setting 'ASPNETCORE_ENVIRONMENT' launchSettings.json, web.config or in mstest.runsettings if you are in a unit test project
    /// 
    /// If 'ASPNETCORE_ENVIRONMENT' variable is defined in launchSettings and web.config, the one in launchSettings overwrites the value in web.config
    /// 
    /// If no 'ASPNETCORE_ENVIRONMENT is specified it will transform based on 'Configuration Mode' your code was built with: 'Release' or 'Debug' only
    /// 
    /// WARNING: Bug in Microsoft's code, if you call "UseEnvironment()" this wont transform based on that environment (as of today...)
    /// 
    /// WARNING: Requires app restart if configuration changes
    /// </summary>
    ///<example>
    /// Samle of launchSettings.json:
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
    /// Sample of web.config: 
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
    /// Sample of mstest.runsettings:
    /// - Register runsettings file in your csproj variable: 'RunSettingsFilePath'
    /// - Tip: View source code of SystemLibrary.Common.Net.Tests inside the repo SystemLibrary.Common.Net on github
    /// <code class="language-csharp hljs">
    /// &lt;RunSettings&gt;
    ///   &lt;RunConfiguration&gt;
    ///       &lt;EnvironmentVariables&gt;
    ///           &lt;ASPNETCORE_ENVIRONMENT&gt;Debug&lt;/ASPNETCORE_ENVIRONMENT&gt;
    /// </code>
    /// 
    /// A TestConfig class example:
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
    /// Add 'TestConfig.json' (can also be on the xml format) to either ~/, ~/Configs/**, ~/Configurations/**
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
    /// <code>
    /// var testConfig = TestConfig.Current;
    /// var name = testConfig.Name;
    /// //'name' is now 'Hello World'
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