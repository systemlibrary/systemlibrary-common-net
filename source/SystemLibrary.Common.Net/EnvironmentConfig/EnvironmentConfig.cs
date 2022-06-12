using System;
using System.IO;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class containing various environment specific configurations common to all .NET applications
    /// 
    /// For instance, it contains the value of 'ASPNETCORE_ENVIRONMENT', which is used internally by all config transformations
    /// </summary>
    public class EnvironmentConfig //: Config<EnvironmentConfig>
    {
        static string _AspNetCoreConfiguration;

        static string AspNetCoreConfigurationReadFirstFoundInLaunchSettings()
        {
            var rootDirectory = AppContext.BaseDirectory;
            if (File.Exists(rootDirectory + "Properties\\launchSettings.json"))
            {
                var lines = File.ReadAllLines(rootDirectory + "Properties\\launchSettings.json");
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
            }
            return null;
        }

        /// <summary>
        /// Gets the Environment variable 'ASPNETCORE_ENVIRONMENT'
        /// 
        /// If it does not exist, it reads 'Properties\launchSettings.json' returning the value of first found 'ASPNETCORE_ENVIRONMENT' (forward read only)
        /// 
        /// If it still does not exist, this returns either 'Debug' or 'Release' based wether it is a Debug or Release build
        /// </summary>
        public static string AspNetCoreEnvironment
        {
            get
            {
                if (_AspNetCoreConfiguration == null)
                {
                    _AspNetCoreConfiguration = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    if (_AspNetCoreConfiguration.IsNot())
                        _AspNetCoreConfiguration = Environment.GetEnvironmentVariable("ASPNET_ENV");

                    if (_AspNetCoreConfiguration.IsNot())
                        _AspNetCoreConfiguration = AspNetCoreConfigurationReadFirstFoundInLaunchSettings();

                    //TODO: Read EnvironmentName-variable used in web apps through "UseEnvironment()" call, somehow...

                    if (_AspNetCoreConfiguration.IsNot())
                    {
                        bool isDebug = false;
#if DEBUG
                        isDebug = true;
#endif
                        if (isDebug)
                            _AspNetCoreConfiguration = "Debug";
                        else
                            _AspNetCoreConfiguration = "Release";
                    }
                }
                return _AspNetCoreConfiguration;
            }
        }

        // Commented out: Got nothing to do in EnvironmentConfig -- appName does not change based on Envs when we have EnvironmentName variable
        //public static string ApplicationName => Environment.GetEnvironmentVariable("ApplicationName");
    }
}
