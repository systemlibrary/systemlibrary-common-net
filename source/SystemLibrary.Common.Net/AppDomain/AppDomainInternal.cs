using System;
using System.IO;

namespace SystemLibrary.Common.Net;

internal static class AppDomainInternal
{
    static string _ContentRootPath;
    internal static string ContentRootPath
    {
        get
        {
            if (_ContentRootPath == null)
            {
                try
                {
                    _ContentRootPath = AppDomain.CurrentDomain?.GetData("ContentRootPath") + "";
                }
                catch
                {
                    // swallow
                }

                if (_ContentRootPath.IsNot())
                    _ContentRootPath = new DirectoryInfo(AppContext.BaseDirectory).FullName;

                if (_ContentRootPath.EndsWith("\\", StringComparison.Ordinal))
                    _ContentRootPath = _ContentRootPath.Substring(0, _ContentRootPath.Length - 1);

                bool IsWithinBin()
                {
                    // TODO: Consider test projects if config files are set as "content" in build mode or not
                    //if (_ContentRootPath.Contains(".Tests\\"))
                    //    return false;

                    return _ContentRootPath.Contains("\\bin\\", StringComparison.Ordinal) ||
                         _ContentRootPath.Contains("/bin/", StringComparison.Ordinal) ||
                         _ContentRootPath.Contains("/Bin/", StringComparison.Ordinal) ||
                        _ContentRootPath.Contains("\\Bin\\", StringComparison.Ordinal) ||
                        _ContentRootPath.Contains("\\BIN\\", StringComparison.Ordinal);
                }

                var wasInBin = false;
                while (IsWithinBin())
                {
                    wasInBin = true;

                    var temp = _ContentRootPath;

                    _ContentRootPath = new DirectoryInfo(_ContentRootPath).Parent?.FullName;

                    if (_ContentRootPath == null)
                    {
                        _ContentRootPath = temp;
                        break;
                    }
                }

                if (wasInBin)
                {
                    // Move to parent of Bin as the Condition checks ending slash of Bin
                    _ContentRootPath = new DirectoryInfo(_ContentRootPath).Parent.FullName;
                }

                _ContentRootPath = _ContentRootPath.Replace("\\", "/");
            }

            return _ContentRootPath;
        }
    }
}
