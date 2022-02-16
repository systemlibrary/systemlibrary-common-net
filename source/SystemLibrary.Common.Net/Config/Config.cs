using Microsoft.Extensions.Configuration;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class for creating configurations files next to your code files
    /// 
    /// Configuration can be placed in either of these 3 hardcoded locations in your app:
    /// ~/, ~/Configs/*, ~/Configurations/*
    /// 
    /// Configuration files can be on format: json or xml
    /// 
    /// Configurations can be added to your default 'appsettings.json' if you do not want additional files
    /// 
    /// Transformations are automatically ran based on the .NET variable 'ASPNETCORE_ENVIRONMENT' (google it)
    ///     * Unit tests can run transformations by passing 'ASPNETCORE_ENVIRONMENT' variable to its startup
    ///         * Add mstest.runsettings (google runsettings format)
    ///         * Add ASPNETCORE_ENVIRONMENT to the runsettings-file
    ///         * Register runsettings-file in your csproj variable: RunSettingsFilePath
    ///             * TIP: Look in the source code of SystemLibrary.Common.Net.Tests
    ///     
    /// If no 'ASPNETCORE_ENVIRONMENT' is specified, it will use Configuration Mode Name for Configuration Transformations (Debug or Release only [I think])
    /// 
    /// WARNING: If for instance 'Debug' is the environment you start app with, but a debug transformation file do not exist, it will transform the 'Release' instead, if 'Release' transformation file exists
    /// 
    /// WARNING: Singleton pattern behind the scene - requires app restart if configuration changes
    /// </summary>
    ///<example>
    /// A TestConfig class example:
    ///
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
    /// Add 'TestConfig.json' (can also be on the xml format) to either ~/, ~/Configs/*, ~/Configurations/*
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