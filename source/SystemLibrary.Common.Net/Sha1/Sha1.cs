﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net;

internal static class Sha1
{
    const int ResetCounter = 400;
    static int SHA1Counter = ResetCounter;
    static object counterlock = new object();
    static SHA1 _SHA1;
    static SHA1 SHA1
    {
        get
        {
            System.Threading.Interlocked.Increment(ref SHA1Counter);

            if (SHA1Counter > ResetCounter)
            {
                lock (counterlock)
                {
                    if (SHA1Counter > ResetCounter)
                    {
                        _SHA1?.Dispose();
                        _SHA1 = null;
                        _SHA1 = SHA1.Create();
                        SHA1Counter = 0;
                    }
                }
            }

            if (_SHA1 == null)
                return SHA1.Create();

            return _SHA1;
        }
    }

    internal static string Compute(byte[] bytes)
    {
        if (bytes == null) return null;

        if (bytes.Length == 0) return "";

        return BitConverter.ToString(SHA1.ComputeHash(bytes));
    }

    internal static string Compute(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        return BitConverter.ToString(SHA1.ComputeHash(stream));
    }

    internal static async Task<string> ComputeAsync(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        return BitConverter.ToString(await SHA1.ComputeHashAsync(stream).ConfigureAwait(false));
    }
}
