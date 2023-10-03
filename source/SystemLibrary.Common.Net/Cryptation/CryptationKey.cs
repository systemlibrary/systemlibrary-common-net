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
    internal const string KeyName = "SYSLIBCRYPTATIONKEY";


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

                    if (Cryptation.DevelopmentCryptationKey?.Length > 0)
                    {
                        _Key = Encoding.UTF8.GetBytes(Cryptation.DevelopmentCryptationKey.ToMD5Hash().Replace("-", ""));
                        return _Key;
                    }

                    var temp = System.Environment.GetEnvironmentVariable(KeyName);

                    try
                    {
                        if (temp.IsNot())
                            temp = System.Environment.GetEnvironmentVariable(KeyName, System.EnvironmentVariableTarget.User);
                    }
                    catch
                    {
                    }

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

    internal static string GetExceptionMessage(string cipherText)
    {
        var m1 = "Could not decrypt value starting with letter: " + cipherText.MaxLength(2);
        var m2 = (string)null;

        if (Cryptation.DevelopmentCryptationKey?.Length > 0)
        {
            m1 += " from Cryptation.DevelopmentCryptationKey";
            m2 = "Tried decrypting with key starting with: " + Cryptation.DevelopmentCryptationKey.MaxLength(2);
        }
        else
        {
            try
            {
                var v1 = System.Environment.GetEnvironmentVariable(KeyName);
                if (v1.Is())
                {
                    m1 += " from Environment.GetEnvironmentVariable()";
                    m2 = "Tried decrypting with key starting with: " + v1.MaxLength(2);
                }
            }
            catch
            {
            }
            try
            {
                if (m2.IsNot())
                {
                    var v2 = System.Environment.GetEnvironmentVariable(KeyName, System.EnvironmentVariableTarget.User);
                    if (v2.Is())
                    {
                        m1 += " from Environment.GetEnvironmentVariable() as User";
                        m2 = "Tried decrypting with key starting with: " + v2.MaxLength(2);
                    }
                }
            }
            catch
            {
            }

            try
            {
                if (m2.IsNot())
                {
                    var v3 = System.Environment.GetEnvironmentVariable(KeyName, System.EnvironmentVariableTarget.Machine);
                    if (v3.Is())
                    {
                        m1 += " from Environment.GetEnvironmentVariable() as Machine";
                        m2 = "Tried decrypting with key starting with: " + v3.MaxLength(2);
                    }
                }
            }
            catch
            {
            }

            try
            {
                if (m2.IsNot())
                {
                    var v4 = System.Environment.GetEnvironmentVariable(KeyName, System.EnvironmentVariableTarget.Process);
                    if (v4.Is())
                    {
                        m1 += " from Environment.GetEnvironmentVariable() as Process";
                        m2 = "Tried decrypting with key starting with: " + v4.MaxLength(2);
                    }
                }
            }
            catch
            {
            }

            try
            {
                if (m2.IsNot())
                {
                    var v5 = CryptationKeyFile.Name;
                    if (v5.Is())
                    {
                        m1 += " from CryptationKeyFile";
                        m2 = "Tried decrypting with key starting with: " + v5.MaxLength(2);
                    }
                }
            }
            catch
            {
            }

            if (m2.IsNot())
            {
                m1 += " default library hardcoded value";
                m2 = "Tried decrypting with key starting with: AB";
            }
        }

        if(m2.IsNot())
        {
            m2 = "Tried decrypting, but could not find a key to decrypt with";
        }

        return m1 + "\n" + m2;
    }
}
