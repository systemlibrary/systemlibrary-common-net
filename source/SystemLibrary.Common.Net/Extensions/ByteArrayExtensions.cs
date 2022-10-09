using System;
using System.Security.Cryptography;

namespace SystemLibrary.Common.Net.Extensions;

public static class ByteArrayExtensions
{
    /// <summary>
    /// Return a base64 string of the bytes
    /// 
    /// If input is null, it returns null
    /// </summary>
    public static string ToBase64(this byte[] bytes)
    {
        if (bytes == null) return null;

        return Convert.ToBase64String(bytes);
    }
}
