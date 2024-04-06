using System;
using System.Text;

using static System.Net.Mime.MediaTypeNames;

namespace SystemLibrary.Common.Net.Extensions;

public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// Returns true if the span is not null and length larger than 0 else false
    /// </summary>
    /// <example>
    /// <code>
    /// var textSpan = "Hello World".AsSpan();
    /// 
    /// var isSpan = textSpan.Is(); // True, span is not null
    /// </code>
    /// </example>
    public static bool Is<T>(this ReadOnlySpan<T> span)
    {
        return span != null && span.Length != 0;
    }

    /// <summary>
    /// Returns true if the span is null or length is 0 else false
    /// </summary>
    /// <example>
    /// <code>
    /// var textSpan = "Hello World".AsSpan();
    /// 
    /// var isSpan = textSpan.IsNot(); // False, as span is not null, and has text length &gt; 0
    /// </code>
    /// </example>
    public static bool IsNot<T>(this ReadOnlySpan<T> span)
    {
        return span == null || span.Length == 0;
    }

    /// <summary>
    /// Convert a ReadOnlySpan to Base64 (only for Char type for time being)
    /// 
    /// Returns null if input is null, else a Base64 representation of the input
    /// </summary>
    /// <example>
    /// <code>
    /// var textSpan = "Hello World".AsSpan();
    /// 
    /// var base64 = textSpan.ToBase64();
    /// </code>
    /// </example>
    public static string ToBase64(this ReadOnlySpan<char> span, Encoding encoding = default)
    {
        if (span == null) return default;

        Span<byte> bytes = new byte[span.Length];

        if (encoding == default)
        {
            Encoding.UTF8.GetBytes(span, bytes);

            return bytes.ToArray().ToBase64();
        }

        encoding.GetBytes(span, bytes);

        return bytes.ToArray().ToBase64();
    }

    /// <summary>
    /// Converts the ReadOnlySpan of Char to a byte array
    /// 
    /// Returns null if input is null, else byte array
    /// </summary>
    /// <example>
    /// <code>
    /// var textSpan = "Hello World".AsSpan();
    /// 
    /// var bytes = textSpan.GetBytes();
    /// </code>
    /// </example>
    public static byte[] GetBytes(this ReadOnlySpan<char> span, Encoding encoding = default)
    {
        if (span == null) return default;

        Span<byte> bytes = new byte[span.Length];

        if (encoding == default)
        {
            Encoding.UTF8.GetBytes(span, bytes);

            return bytes.ToArray();
        }

        encoding.GetBytes(span, bytes);

        return bytes.ToArray();
    }
}