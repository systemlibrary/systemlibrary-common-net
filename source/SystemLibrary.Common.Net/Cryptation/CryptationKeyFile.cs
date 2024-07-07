using System.IO;
using System.Linq;

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Options;

namespace SystemLibrary.Common.Net;

internal static class CryptationKeyFile
{
    internal static string _NameHashed;

    internal static string Dir { get; set; }

    internal static string IV;

    static object _Lock = new object();

    internal static string NameHashed
    {
        get
        {
            if (_NameHashed == null)
            {
                lock (_Lock)
                {
                    if (_NameHashed != null) return _NameHashed;

                    var keyManagementOptions = Services.Get<IOptions<KeyManagementOptions>>();

                    Dir = (keyManagementOptions?.Value?.XmlRepository as FileSystemXmlRepository)?.Directory?.FullName;

                    if (Dir.Is())
                    {
                        var keyFileName = GetKeyFileFullName(Dir);

                        _NameHashed = Path.GetFileName(keyFileName);

                        if(_NameHashed.Is())
                        {
                            CryptationIV.SetIV(_NameHashed.Replace("key-", "")
                                .Replace("xml", "")
                                .Obfuscate(116)
                                .ToSha256Hash());

                            _NameHashed = _NameHashed.ToSha256Hash();
                        }
                    }

                    // NOTE: Not enabled, we do not fallback to "appName" as Key if a key-file do not exist
                    // var dataProtectionOptions = Services.Get<IOptions<DataProtectionOptions>>();
                    // dir = dataProtectionOptions?.Value?.ApplicationDiscriminator;
                    if (_NameHashed == null)
                        _NameHashed = "";
                }
            }

            return _NameHashed;
        }
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

            if (validated != null) return validated;
        }
        return null;
    }

    static string ValidateFileContent(string fullFileName)
    {
        if (fullFileName.Length < 44) return null;

        if (!fullFileName.Contains("key-")) return null;

        var content = File.ReadAllText(fullFileName);

        if (content.IsNot()) return null;

        content = content.ToLower();

        if (!content.Contains("deserialize")) return null;

        if (!content.Contains("encrypt")) return null;

        return fullFileName;
    }
}
