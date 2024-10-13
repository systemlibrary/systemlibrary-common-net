﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net;

internal static class Md5
{
    internal static string Compute(byte[] bytes)
    {
        if (bytes == null) return null;

        if (bytes.Length == 0) return "";

        using(var hasher = MD5.Create())
            return BitConverter.ToString(hasher.ComputeHash(bytes));
    }

    internal static string Compute(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        using (var hasher = MD5.Create())
            return BitConverter.ToString(hasher.ComputeHash(stream));
    }

    internal static async Task<string> ComputeAsync(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        using (var hasher = MD5.Create())
            return BitConverter.ToString(await hasher.ComputeHashAsync(stream).ConfigureAwait(false));
    }
}
