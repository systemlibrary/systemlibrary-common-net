using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net;

internal static class Md5
{
    const int ResetCounter = 400;
    static object counterlock = new object();
    static int MD5Counter = ResetCounter;
    static MD5 _MD5;
    static MD5 MD5
    {
        get
        {
            System.Threading.Interlocked.Increment(ref MD5Counter);

            if (MD5Counter > ResetCounter)
            {
                lock (counterlock)
                {
                    if (MD5Counter > ResetCounter)
                    {
                        _MD5?.Dispose();
                        _MD5 = null;
                        _MD5 = MD5.Create();
                        MD5Counter = 0;
                    }
                }
            }

            if (_MD5 == null)
                return MD5.Create();

            return _MD5;
        }
    }

    internal static string Compute(byte[] bytes)
    {
        if (bytes == null) return null;

        if (bytes.Length == 0) return "";

        return BitConverter.ToString(MD5.ComputeHash(bytes));
    }

    internal static string Compute(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        return BitConverter.ToString(MD5.ComputeHash(stream));
    }

    internal static async Task<string> ComputeAsync(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        return BitConverter.ToString(await MD5.ComputeHashAsync(stream).ConfigureAwait(false));
    }
}
