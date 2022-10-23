using System;

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
}