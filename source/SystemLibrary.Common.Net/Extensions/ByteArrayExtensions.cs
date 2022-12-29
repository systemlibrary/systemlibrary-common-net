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
    /// 
    /// //Tip: If you dont need base64 format, .Obfuscating() method is faster if data is less than ~400KB
    /// </code>
    /// </example>
    public static string ToBase64(this byte[] bytes)
    {
        if (bytes == null) return null;
        
        // Research performance? Add compress to base64? Or a new method .Compress extension?
        //https://learn.microsoft.com/en-us/answers/questions/226531/c-best-method-to-reduce-size-of-large-string-data.html

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Returns a hash string of the bytes
    /// 
    /// If input is null or empty it returns null or empty string
    /// 
    /// Tip: If data is less than ~200 bytes then .ToMD5Hash() is faster
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
    /// 
    /// Tip: If data is larger than ~200 bytes then .ToSha1Hash() is faster
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
