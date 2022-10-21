using System;

namespace SystemLibrary.Common.Net.Extensions;

public static class ByteArrayExtensions
{
    /// <summary>
    /// Return a base64 string of the bytes
    /// 
    /// If input is null or empty it returns null or empty string
    /// </summary>
    /// <example>
    /// <code>
    /// var bytes = "hello world".GetBytes();
    /// var base64string = bytes.ToBase64();
    /// </code>
    /// </example>
    public static string ToBase64(this byte[] bytes)
    {
        if (bytes == null) return null;

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Returns a hash string of the bytes
    /// 
    /// If input is null or empty it returns null or empty string
    /// </summary>
    /// <example>
    /// <code>
    /// var bytes = "hello world".GetBytes();
    /// var sha1string = bytes.ToSha1Hash();
    /// </code>
    /// </example>
    public static string ToSha1Hash(this byte[] bytes)
    {
        if (bytes == null) return null;

        if (bytes.Length == 0) return "";

        return Sha1.Compute(bytes);
    }

    /// <summary>
    /// Returns a hash string of the bytes
    /// 
    /// If input is null or empty it returns null or empty string
    /// </summary>
    /// <example>
    /// <code>
    /// var bytes = "hello world".GetBytes();
    /// var md5string = bytes.ToMD5Hash();
    /// </code>
    /// </example>
    public static string ToMD5Hash(this byte[] bytes)
    {
        if (bytes == null) return null;

        if (bytes.Length == 0) return "";

        return Md5.Compute(bytes);
    }
}
