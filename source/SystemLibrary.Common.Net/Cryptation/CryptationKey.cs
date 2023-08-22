using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace SystemLibrary.Common.Net;

internal static class CryptationKey
{
    const string KeyName = "SYSLIBCRYPTATIONKEY";

    internal static byte[] _Key;
    static object KeyLock = new object();

    internal static byte[] Current
    {
        get
        {
            if (_Key == null)
            {
                lock (KeyLock)
                {
                    if (_Key != null) return _Key;

                    var temp = System.Environment.GetEnvironmentVariable(KeyName);

                    if (temp.IsNot())
                        temp = System.Environment.GetEnvironmentVariable(KeyName, System.EnvironmentVariableTarget.Machine);

                    if (temp.IsNot())
                        temp = System.Environment.GetEnvironmentVariable(KeyName, System.EnvironmentVariableTarget.Process);

                    if (temp.IsNot())
                        temp = CryptationKeyFile.Name;

                    if (temp.IsNot())
                        temp = "ABCDEFGH098765432";

                    _Key = Encoding.UTF8.GetBytes(temp.ToMD5Hash().Replace("-", ""));
                }
            }
            return _Key;
        }
    }

}
