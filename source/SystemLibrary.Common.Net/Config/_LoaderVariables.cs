using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Configuration;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net;

internal static class ConfigVariables
{
    internal static IConfigurationRoot AppSettings;

    internal static string _EnvironmentNameLowered;

    internal static string EnvironmentNameLowered
    {
        get
        {
            _EnvironmentNameLowered ??= AspNetCoreEnvironment.Value.ToLower();
            return _EnvironmentNameLowered;
        }
    }

    internal static string[] ConfigurationFilesLowered;

    internal static string[] AppSettingFilesLowered;

    static ConfigVariables()
    {
        var contentRootDirectory = AppDomainInternal.ContentRootPath;

        if (!contentRootDirectory.EndsWith("/", StringComparison.Ordinal) && !contentRootDirectory.EndsWith("\\", StringComparison.Ordinal))
        {
            if (contentRootDirectory.Contains("/", StringComparison.Ordinal))
                contentRootDirectory += "/";
            else
                contentRootDirectory += "\\";
        }

        // TODO: Optimize by invoking the searches in paralell ? 
        // - not sure its worth it, benchmark, as one often just have 1 folder anyways
        // - at least try it out
        var rootConfigurationFiles = GetConfigurationFilesInFolder(contentRootDirectory, false);

        var config = GetConfigurationFilesInFolder(contentRootDirectory + "config\\", true);

        var configs = GetConfigurationFilesInFolder(contentRootDirectory + "configs\\", true);

        var configuration = GetConfigurationFilesInFolder(contentRootDirectory + "configuration\\", true);

        var configurations = GetConfigurationFilesInFolder(contentRootDirectory + "configurations\\", true);

        var tempConfigFiles = rootConfigurationFiles.Add(FilterValidConfigurationFileNames, config, configs, configuration, configurations);

        ConfigurationFilesLowered = Array.ConvertAll(tempConfigFiles, s => s.ToLower());

        AppSettingFilesLowered = ConfigurationFilesLowered.Where(FilterAppSettingFiles).ToArray();

        var appSettingsBuilder = new ConfigurationBuilder();

        AddConfigurationFilesAndTransformationFiles(appSettingsBuilder, AppSettingFilesLowered);

        AppSettings = appSettingsBuilder
            .AddEnvironmentVariables()
            .Build();
    }

    static string[] GetConfigurationFilesInFolder(string directoryPath, bool searchRecursively)
    {
        if (!Directory.Exists(directoryPath)) return new string[0];

        string[] files;
        if (!searchRecursively)
            files = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly);
        else
            files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

        if (files == null || files.Length == 0) return new string[0];

        return files
            .Where(x => x.EndsWithAny(StringComparison.OrdinalIgnoreCase, ".json", ".xml", ".config"))
            .ToArray();
    }

    static bool FilterAppSettingFiles(string fileLowered)
    {
        if (fileLowered.IsNot()) return false;

        fileLowered = fileLowered.ToLower();

        return (fileLowered.Contains("\\appsettings.") || fileLowered.Contains("/appsettings.")) && fileLowered.EndsWithAny(StringComparison.OrdinalIgnoreCase, ".json", ".xml", ".config");
    }

    static bool FilterValidConfigurationFileNames(string file)
    {
        if (file.IsNot()) return false;

        file = file.ToLower();

        if (file.Contains(".runtimeconfig.", StringComparison.Ordinal) ||
            file.Contains(".deps.json", StringComparison.Ordinal) ||
            file.Contains("microsoft.visualstudio", StringComparison.Ordinal) ||
            file.ContainsAny(StringComparison.Ordinal, "packages.json", "packages.xml", "package.json", "package-lock.json"))
            return false;

        return true;
    }

    internal static void AddConfigurationFilesAndTransformationFiles(ConfigurationBuilder builder, IEnumerable<string> files)
    {
        if (files == null) return;

        foreach (var f in files)
        {
            if (f.IsNot()) continue;

            var extension = Path.GetExtension(f)?.ToLower();
            if (extension == ".json")
            {
                builder.AddJsonFile(f, true, true);
            }
            else if (extension == ".xml")
            {
                if (!f.Contains(".Tests", StringComparison.Ordinal) && f.Contains("\\SystemLibrary.Common.", StringComparison.Ordinal)) continue;

                builder.AddXmlFile(f, true, true);
            }
            else if (extension == ".config")
            {
                if (!f.Contains(".Tests", StringComparison.Ordinal) && f.Contains("\\SystemLibrary.Common.", StringComparison.Ordinal)) continue;

                var data = File.ReadAllText(f);
                if (data.Length > 0)
                {
                    if (data[0] == '<' || data.EndsWith(">", StringComparison.Ordinal))
                        builder.AddXmlFile(f, true, true);
                    else
                        builder.AddJsonFile(f, true, true);
                }
            }
        }
    }
}
