using System.IO;
using System.Linq;

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Options;

namespace SystemLibrary.Common.Net;

partial class CryptationKey
{
    internal static string TryGetKeyFromDataRingKeyFile()
    {
        if (_KeyFileName.Is()) return _KeyFileName;

        var keyManagementOptions = Services.Get<IOptions<KeyManagementOptions>>();

        _KeyDirectory = (keyManagementOptions?.Value?.XmlRepository as FileSystemXmlRepository)?.Directory?.FullName;

        if (_KeyDirectory.IsNot())
            _KeyDirectory = EnvironmentConfig.Current.ContentRootPath;

        if (_KeyDirectory.IsNot()) return null;

        _KeyFileName = GetKeyFileFullName(_KeyDirectory);

        if (_KeyFileName.IsNot()) return null;

        return Path.GetFileName(_KeyFileName);
    }

    static string GetKeyFileFullName(string keyDirectory)
    {
        var fileNames = Directory.GetFiles(keyDirectory, "*.xml", SearchOption.TopDirectoryOnly);

        if (fileNames == null || fileNames.Length == 0)
            return null;

        fileNames = fileNames.Order().ToArray();

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
