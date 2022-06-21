using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Configuration;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net
{
    partial class Config<T>
    {
        internal static class ConfigLoader<TConf> where TConf : class
        {
            static string[] ConfigurationFiles;

            static IConfigurationRoot AppSettings;

            static string _EnvironmentNameLowered;

            static string EnvironmentNameLowered
            {
                get
                {
                    if(_EnvironmentNameLowered== null){
                        _EnvironmentNameLowered = AspNetCoreEnvironment.Value.ToLower();
                    }
                    return _EnvironmentNameLowered;
                }
            }

            static bool IgnoreRuntimeConfigAndDeps(string file)
            {
                if (file.IsNot()) return false;
                file = file.ToLower();
                    
                if(file.Contains(".runtimeconfig.") ||
                    file.Contains(".deps.json"))
                    return false;

                return true;
            }

            static bool FilterAppSettingsFiles(string file)
            {
                if (file.IsNot()) return false;

                file = file.ToLower();

                return file.StartsWith("appsettings.") && (file.EndsWith(".json") || file.EndsWith(".xml"));
            }

            static ConfigLoader()
            {
                var rootDirectory = AppContext.BaseDirectory;

                var rootConfigurationFiles = GetConfigurationFilesInFolder(rootDirectory, false);

                var configs = GetConfigurationFilesInFolder(rootDirectory + "configs\\", true);

                var configurations = GetConfigurationFilesInFolder(rootDirectory + "configurations\\", true);
                
                ConfigurationFiles = rootConfigurationFiles.Add(IgnoreRuntimeConfigAndDeps, configs, configurations);

                var builder = new ConfigurationBuilder();

                var appSettingFiles = rootConfigurationFiles.Where(FilterAppSettingsFiles);

                AddConfigurationFiles(builder, appSettingFiles);

                AppSettings = builder
                    .AddEnvironmentVariables()
                    .Build();
            }

            static string[] GetConfigurationFilesInFolder(string directoryPath, bool searchRecursively)
            {
                if (!Directory.Exists(directoryPath)) return new string[0];

                string[] files;
                if(!searchRecursively)
                    files = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly);
                else
                    files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                if (files == null || files.Length == 0) return new string[0];

                return files
                    .Where(x => x.EndsWithAnyCaseInsensitive(".json", ".xml", ".config"))
                    .ToArray();
            }

            internal static IConfiguration Load()
            {
                var type = typeof(T);

                var configurationName = type.Name.ToLower();

                var files = new List<string>();

                if (ConfigurationFiles != null && ConfigurationFiles.Length > 0)
                {
                    foreach (var file in ConfigurationFiles)
                    {
                        if (file.Is())
                        {
                            var lowered = file.ToLower();

                            if (lowered.Contains(configurationName))
                            {
                                var values = lowered.Split('.');

                                if (values != null && values.Length > 1 && values[^2].Contains(configurationName))
                                {
                                    if (!lowered.Contains("." + EnvironmentNameLowered + "."))
                                    {
                                        files.Add(file);
                                    }
                                }
                            }
                        }
                    }
                }

                if (EnvironmentNameLowered.Is())
                {
                    var configTransformationName = configurationName + "." + EnvironmentNameLowered + ".";

                    foreach (var file in ConfigurationFiles)
                        if (file.ToLower().Contains(configTransformationName) &&
                            !files.Contains(file))
                        {
                            files.Add(file);
                        }
                }

                if (files.Count > 0)
                {
                    var builder = new ConfigurationBuilder();

                    AddConfigurationFiles(builder, files);

                    return builder
                        .AddEnvironmentVariables()
                        .Build();
                }

                return AppSettings;
            }

            static void AddConfigurationFiles(ConfigurationBuilder builder, IEnumerable<string> files)
            {
                if (files != null)
                {
                    foreach (var f in files)
                    {
                        if (f.Is())
                        {
                            var extension = Path.GetExtension(f)?.ToLower();
                            if (extension == ".json")
                            {
                                builder.AddJsonFile(f, true, true);
                            }

                            else if (extension == ".xml")
                            {
                                builder.AddXmlFile(f, true, true);
                            }
                        }
                    }
                }
            }
        }
    }
}
