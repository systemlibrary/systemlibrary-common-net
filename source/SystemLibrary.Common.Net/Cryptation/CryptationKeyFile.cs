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
    internal static string Name
    {
        get
        {
            if (_Name == null)
            {
                // Try read the data protected key file and use its filename as the "key"
                var dir = AppDomain.CurrentDomain?.BaseDirectory;

                if (dir == null)
                {
                    _Name = "";
                    return _Name;
                }

                var files = Directory.GetFiles(dir, "*.xml", SearchOption.TopDirectoryOnly);

                if (files == null || files.Length == 0)
                    files = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);

                var directory = new DirectoryInfo(dir);

                foreach (var file in files)
                {
                    if (file.Length < 44) continue;

                    if (!file.Contains("\\key-")) continue;

                    var content = File.ReadAllText(file);

                    if (content.IsNot()) continue;

                    if (!content.Contains("deserializerType")) continue;

                    _Name = Path.GetFileName(file).Replace(".xml", "");

                    break;
                }

                if (_Name == null)
                    _Name = "";
            }

            return _Name;
        }
    }

}
