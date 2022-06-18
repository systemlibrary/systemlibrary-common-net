using System;
using System.IO;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class containing various environment specific variables common to all .NET applications based on your 'environmentConfig.json' file
    /// </summary>
    public class EnvironmentConfig : Config<EnvironmentConfig> 
    {
        enum Environment
        {
            Local,
            Dev,
            Development,
            UnitTest,
            QA,
            AT,
            Stage,
            Test,
            PreProd,
            PreProduction,
            Prod,
            Production
        }

        static string _AspNetCoreConfiguration;

        static string AspNetCoreConfigurationReadFirstFoundInLaunchSettings()
        {
            var rootDirectory = AppContext.BaseDirectory;

            string launchSettings = null;

            if (File.Exists(rootDirectory + "Properties\\launchSettings.json"))
            {
                launchSettings = File.ReadAllText(rootDirectory + "Properties\\launchSettings.json");
            }
            else if (File.Exists(rootDirectory + "launchSettings.json"))
            {
                launchSettings = File.ReadAllText(rootDirectory + "launchSettings.json");
            }

            if (launchSettings.Is())
            {
                var lines = launchSettings.Split(System.Environment.NewLine);
                if (lines != null && lines.Length > 0)
                {
                    //TODO: Read "launchSettings.json" as a proper json configuration
                    foreach (var line in lines)
                    {
                        if (line.Contains("ASPNETCORE_ENVIRONMENT"))
                        {
                            var keyValue = line.Split(':');
                            if (keyValue.Length > 1)
                                return keyValue[1].Trim().Replace("\"", "");
                            break;
                        }
                    }
                }

                //return launchSettings.PartialJson<string>("ASPNETCORE_ENVIRONMENT", true);
            }


            string isDebugging = null;
#if DEBUG
            isDebugging = "Debug";
#endif
            return isDebugging;
        }

        /// <summary>
        /// Gets the Environment variable 'ASPNETCORE_ENVIRONMENT'
        /// 
        /// If it does not exist, it reads 'Properties\launchSettings.json' returning the value of first found 'ASPNETCORE_ENVIRONMENT' (forward read only)
        /// 
        /// If it still does not exist, this returns "", never null
        /// </summary>
        internal static string AspNetCoreEnvironment
        {
            get
            {
                if (_AspNetCoreConfiguration == null)
                {
                    _AspNetCoreConfiguration = AspNetCoreConfigurationReadFirstFoundInLaunchSettings();

                    if (_AspNetCoreConfiguration.IsNot())
                        _AspNetCoreConfiguration = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    //TODO: Read EnvironmentName-variable used in web apps through "UseEnvironment()" call, somehow...
                    if (_AspNetCoreConfiguration.IsNot())
                        _AspNetCoreConfiguration = "";
                }
                return _AspNetCoreConfiguration;
            }
        }

        // Commented out: Got nothing to do in EnvironmentConfig -- appName does not change based on Envs when we have EnvironmentName variable
        //public static string ApplicationName => Environment.GetEnvironmentVariable("ApplicationName");
        
        Environment? _EnvironmentName;
        Environment EnvironmentName
        {
            get
            {
                if (_EnvironmentName == null)
                    _EnvironmentName = Name.ToEnum<Environment>();

                return _EnvironmentName.Value;
            }
        }

        string _Name;

        /// <summary>
        /// Current environment name, passed into your application either by 'environmentConfig.json' or by passing ASPNETCORE_ENVIRONMENT variable
        /// 
        /// IIS Express:
        /// <code class="language-csharp hljs">
        /// - if: ASPNETCORE_ENVIRONMENT exists in launchSettings
        ///     then: value is returned as 'name'
        /// - if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
        ///     then: value is returned as 'name'
        /// - if: ASNETCORE_ENVIRONMENT exists in web.config
        ///     then: value is returned as 'name'
        /// </code>
        ///  
        /// Test Explorer
        /// <code class="language-csharp hljs">
        /// if: Running Tests in 'Test Explorer'
        /// - Sets environment as 'Configuration Mode' selected in Visual Studio
        /// - if: environmentConfig.json exists
        ///     - if transformation file exists for 'Current Configuration Mode' 
        ///         then: transformation is ran
        ///     - if: environmentConfig.json contains 'name' property
        ///         then: value is returned as 'name'
        ///     - else if: launchSettings.json is found and contains 'ASPNETCORE_ENVIRONMENT'        
        ///         then: value is returned as 'name'
        ///     - else if ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
        ///         then: value is returned as 'name'
        /// 
        /// - if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
        ///     then: value is returned as 'name'
        /// 
        /// - else 
        ///     then: returns "" as 'name'
        /// </code>
        /// 
        /// Console Application
        /// <code class="language-csharp hljs">
        /// 
        /// </code>
        /// 
        /// IIS
        /// <code class="language-csharp hljs">
        /// if: environmentConfig.json exists:
        /// - if: it has transformation files:
        ///     - if: a transformation file exists equal to value of 'ASPNETCORE_ENVIRONMENT', then that transformation is ran
        ///     - else if: a transformation file exists equal to value of 'Configuration Mode' in Visual Studio, then that transformation is ran
        ///     
        /// - if: environmentConfig.json exists and has 'name' (transformations are ran before this step, hence name is 'transformed')
        ///     - return 'name' from environmentConfig.json
        /// 
        /// if: 'ASPNETCORE_ENVIRONMENT' exists:
        ///     - return value of 'ASPNETCORE_ENVIRONMENT'
        /// 
        /// else:
        /// - returns "", a blank string, never null
        /// </code>
        /// </summary>
        public string Name
        {
            get
            {
                if (_Name.Is())
                    return _Name;

                //TODO: Consider throwing new Exception("Environment 'Name' is not set in either 'ASPNETCORE_ENVIRONMENT', or in environmentConfig.json file, or environmentConfig.json file is located in wrong folder");
                //TODO: Consider this way it works, returns empty name, so it never does any transformations
                //TODO: Consider supporting 'environment' in 'appSettings' for the package: systemLibraryCommonNet { environment { name: '...' } }
                return AspNetCoreEnvironment;
            }
            set
            {
                Dump.Write("SETTING VALUE BASED ON CORE AND ITS RUNTIME ENVIRONMENT (auto)" + value);
                _Name = value;
            }
        }

        bool? _IsLocal;

        /// <summary>
        /// Returns true if IsTest and IsProd is false
        /// </summary>
        public bool IsLocal
        {
            get
            {
                if(_IsLocal == null)
                    _IsLocal = !IsTest && !IsProd;

                return _IsLocal.Value;
            }
        }

        bool? _IsProd;

        /// <summary>
        /// Returns true if environment name is 'prod' or 'production', case insensitive
        /// </summary>
        public bool IsProd
        {
            get
            {
                if (_IsProd == null)
                    _IsProd =
                        EnvironmentName == Environment.Prod ||
                        EnvironmentName == Environment.Production;

                return _IsProd.Value;
            }
        }

        bool? _IsTest;

        /// <summary>
        /// Returns true if environment is 'Test', 'Stage', 'QA', 'AT', case insensitive
        /// </summary>
        public bool IsTest
        {
            get
            {
                if (_IsTest == null)
                    _IsTest =
                        EnvironmentName == Environment.Test ||
                        EnvironmentName == Environment.Stage ||
                        EnvironmentName == Environment.AT ||
                        EnvironmentName == Environment.QA;

                return _IsTest.Value;
            }
        }
    }
}
