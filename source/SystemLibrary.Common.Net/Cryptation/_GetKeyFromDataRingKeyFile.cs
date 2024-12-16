using System;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Options;

using SystemLibrary.Common.Net.Configurations;

namespace SystemLibrary.Common.Net;

partial class CryptationKey
{
    internal static string TryGetKeyFromDataRingKeyFile()
    {
        if (_KeyFileFullName.Is()) return Path.GetFileName(_KeyFileFullName);

        var keyManagementOptions = Services.Get<IOptions<KeyManagementOptions>>();

        var keyDirectory = (keyManagementOptions?.Value?.XmlRepository as FileSystemXmlRepository)?.Directory?.FullName;

        if (keyDirectory.IsNot())
            keyDirectory = EnvironmentConfig.Current.ContentRootPath;

        if (keyDirectory.IsNot()) return null;

        _KeyFileFullName = GetKeyFileFullName(keyDirectory);

        if (_KeyFileFullName.IsNot())
        {
            DirectoryInfo parent = new DirectoryInfo(keyDirectory).Parent;

            var count = 13;

            while (_KeyFileFullName.IsNot() && count > 0)
            {
                if (parent == null) return null;

                count--;

                _KeyFileFullName = GetKeyFileFullName(parent.FullName);

                if(_KeyFileFullName.IsNot())
                    parent = parent.Parent;
            }

            if(_KeyFileFullName.Is())
                Debug.Log("Found key ring file in " + parent.FullName);
        }

        if (_KeyFileFullName.IsNot()) return null;

        return Path.GetFileName(_KeyFileFullName);
    }

    static string GetKeyFileFullName(string keyDirectory)
    {
        var fileNames = Directory.GetFiles(keyDirectory, "*.xml", SearchOption.TopDirectoryOnly);

        if (fileNames == null || fileNames.Length == 0)
            return null;

        // Order to preserve the key file used if found multiple
        if(fileNames.Length > 1)
        {
            fileNames = fileNames
               .OrderBy(file =>
               {
                   var creationTime = File.GetCreationTime(file);
                   return creationTime == DateTime.MinValue
                       ? File.GetLastWriteTime(file)
                       : creationTime;
               })
            .ToArray();
        }

        foreach (var fullFileName in fileNames)
        {
            var validated = ValidateFileContent(fullFileName);

            if (validated != null) return validated.Replace("key-", "");
        }
        return null;
    }

    static string ValidateFileContent(string fullFileName)
    {
        if (fullFileName.Length < 44) return null;

        if (!fullFileName.Contains("key-")) return null;

        var content = File.ReadAllText(fullFileName);

        if (content.IsNot()) return null;

        if (!content.Contains("key")) return null;

        if (!content.Contains("deserialize")) return null;

        if (!content.Contains("encrypt")) return null;

        return fullFileName;
    }
}
