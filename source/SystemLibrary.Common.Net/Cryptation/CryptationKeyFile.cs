using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net;

internal static class CryptationKeyFile
{
    internal static string _Name;

    static object _NameLock = new object();

    internal static string Name
    {
        get
        {
            if (_Name == null)
            {
                lock (_NameLock)
                {
                    if (_Name != null) return _Name;

                    // Try read the data protected key file and use its filename as the "key"
                    var root = AppDomain.CurrentDomain?.BaseDirectory;

                    if (root == null)
                    {
                        _Name = "";
                        return _Name;
                    }

                    var rootDirectoryInfo = new DirectoryInfo(root);

                    int maxSearchDepth = 10;

                    var currentSearchDir = new DirectoryInfo(root);

                    while (maxSearchDepth > 0)
                    {
                        var fullName = GetKeyFileFullName(currentSearchDir.FullName);

                        if (fullName != null)
                        {
                            _Name = Path.GetFileName(fullName).Replace(".xml", "");
                            break;
                        }

                        currentSearchDir = currentSearchDir?.Parent;

                        if (currentSearchDir == null) break;

                        maxSearchDepth--;
                    }

                    if (_Name == null)
                        _Name = "";
                }
            }

            return _Name;
        }
    }

    static string GetKeyFileFullName(string rootDirectory)
    {
        var fileNames = Directory.GetFiles(rootDirectory, "*.xml", SearchOption.TopDirectoryOnly);

        if (fileNames == null || fileNames.Length == 0)
            return null;

        var directory = new DirectoryInfo(rootDirectory);

        foreach (var fileName in fileNames)
        {
            if (fileName.Length < 44) continue;

            if (!fileName.Contains("\\key-")) continue;

            var content = File.ReadAllText(fileName);

            if (content.IsNot()) continue;

            if (!content.Contains("deserializerType")) continue;

            if (!content.Contains("encryption")) continue;

            return fileName;
        }
        return null;
    }
}
