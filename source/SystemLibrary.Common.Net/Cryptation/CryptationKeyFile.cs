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

                    int searchBinFolderDepth = 3;

                    var startSearch = new DirectoryInfo(root);
                    while(searchBinFolderDepth > 0 )
                    {
                        if(startSearch?.Name?.ToLower() == "bin")
                        {
                            rootDirectoryInfo = startSearch.Parent;
                            break;
                        }
                        startSearch = startSearch?.Parent;

                        searchBinFolderDepth--;
                    }
                    var fullName = GetKeyFileFullName(rootDirectoryInfo.FullName);

                    if (fullName != null)
                        _Name = Path.GetFileName(fullName).Replace(".xml", "");
                    else
                        _Name = "";
                }
            }

            return _Name;
        }
    }

    static string GetKeyFileFullName(string rootDirectory)
    {
        var files = Directory.GetFiles(rootDirectory, "*.xml", SearchOption.TopDirectoryOnly);

        if (files == null || files.Length == 0)
            files = Directory.GetFiles(rootDirectory, "*.xml", SearchOption.AllDirectories);

        var directory = new DirectoryInfo(rootDirectory);

        foreach (var file in files)
        {
            if (file.Length < 44) continue;

            if (!file.Contains("\\key-")) continue;

            var content = File.ReadAllText(file);

            if (content.IsNot()) continue;

            if (!content.Contains("deserializerType")) continue;

            return file;
        }
        return null;
    }
}
