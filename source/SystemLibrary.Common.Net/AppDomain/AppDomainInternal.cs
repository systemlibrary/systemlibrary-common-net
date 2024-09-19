using System;
using System.IO;

namespace SystemLibrary.Common.Net;

internal static class AppDomainInternal
{
    static object Lock = new object();
    static string _ContentRootPath;
    
    // ContentRootPath never ends with slash
    internal static string ContentRootPath
    {
        get
        {
            if (_ContentRootPath == null)
            {
                lock (Lock)
                {
                    if (_ContentRootPath != null) return _ContentRootPath;

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

                    
                    bool IsWithinBin()
                    {
                        if (_ContentRootPath.Contains(".Tests\\", StringComparison.Ordinal) || _ContentRootPath.Contains(".Test\\", StringComparison.Ordinal))
                            return false;

                        return _ContentRootPath.Contains("\\bin\\", StringComparison.Ordinal) ||
                             _ContentRootPath.Contains("/bin/", StringComparison.Ordinal) ||
                             _ContentRootPath.Contains("/Bin/", StringComparison.Ordinal) ||
                            _ContentRootPath.Contains("\\Bin\\", StringComparison.Ordinal) ||
                            _ContentRootPath.Contains("\\BIN\\", StringComparison.Ordinal) || 
                            _ContentRootPath.EndsWith("\\bin", StringComparison.Ordinal) ||
                            _ContentRootPath.EndsWith("\\Bin", StringComparison.Ordinal) ||
                            _ContentRootPath.EndsWith("/bin", StringComparison.Ordinal) ||
                            _ContentRootPath.EndsWith("/Bin", StringComparison.Ordinal);
                    }

                    while (IsWithinBin())
                    {
                        var temp = _ContentRootPath;

                        _ContentRootPath = new DirectoryInfo(_ContentRootPath).Parent?.FullName;

                        if (_ContentRootPath == null)
                        {
                            _ContentRootPath = temp;
                            break;
                        }
                    }

                    if (_ContentRootPath.EndsWith("\\", StringComparison.Ordinal))
                        _ContentRootPath = _ContentRootPath.Substring(0, _ContentRootPath.Length - 1);

                    if (_ContentRootPath.EndsWith("/", StringComparison.Ordinal))
                        _ContentRootPath = _ContentRootPath.Substring(0, _ContentRootPath.Length - 1);

                    _ContentRootPath = _ContentRootPath.Replace("\\", "/");
                }
            }

            return _ContentRootPath;
        }
    }
}
