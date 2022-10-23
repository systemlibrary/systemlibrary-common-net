using System;

namespace SystemLibrary.Common.Net.Extensions;

public static class SpanExtensions
{
    /// <summary>
    /// Returns true if the span is not null and length larger than 0 else false
    /// </summary>
    public static bool Is<T>(this Span<T> span)
    {
        return span != null && span.Length == 0;
    }

    /// <summary>
    /// Returns true if the span is null or length is 0 else false
    /// </summary>
    public static bool IsNot<T>(this Span<T> span)
    {
        return span == null || span.Length == 0;
    }
}



public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// Returns true if the span is not null and length larger than 0 else false
    /// </summary>
    public static bool Is<T>(this ReadOnlySpan<T> span)
    {
        return span != null && span.Length != 0;
    }

    /// <summary>
    /// Returns true if the span is null or length is 0 else false
    /// </summary>
    public static bool IsNot<T>(this ReadOnlySpan<T> span)
    {
        return span == null || span.Length == 0;
    }
}