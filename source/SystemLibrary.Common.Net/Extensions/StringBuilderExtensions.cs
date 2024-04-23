using System;
using System.Collections.Generic;
using System.Text;

namespace SystemLibrary.Common.Net.Extensions;

/// <summary>
/// String Builder extensions
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Returns true if the string builder is not null and has a text length greater than 0
    /// </summary>
    public static bool Is(this StringBuilder stringBuilder)
    {
        return stringBuilder != null && stringBuilder.Length != 0;
    }

    /// <summary>
    /// Returns true if string builder is null or is empty
    /// </summary>
    public static bool IsNot(this StringBuilder stringBuilder)
    {
        return stringBuilder == null || stringBuilder.Length == 0;
    }

    /// <summary>
    /// Returns true if stringbuilder ends with a certain text, else false
    /// 
    /// - does not throw exception on null
    /// - pass flag for case sensitivity
    /// </summary>
    public static bool EndsWith(this StringBuilder stringBuilder, string ending, bool caseInsensitive = false)
    {
        if (stringBuilder == null || stringBuilder.Length == 0) return false;

        if (ending == null || ending == "") return false;

        var endingLength = ending.Length;

        if (endingLength > stringBuilder.Length) return false;

        var startIndex = stringBuilder.Length - endingLength;
        var endIndex = startIndex + endingLength;
        var j = 0;

        if (!caseInsensitive)
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                if (stringBuilder[i] != ending[j]) return false;
                j++;
            }
        }
        else
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                if (char.ToLowerInvariant(stringBuilder[i]) != char.ToLowerInvariant(ending[j])) return false;
                j++;
            }
        }
        return true;
    }

    /// <summary>
    /// Removes the ending of the text inside the stringbuilder, if it matches the 'ending'
    /// 
    /// Returns true if a text was removed, else false
    /// </summary>
    public static bool TrimEnd(this StringBuilder stringBuilder, params string[] values)
    {
        if (values.IsNot()) return false;

        for (int i = 0; i < values.Length; i++)
        {
            var value = values[i];
            if (stringBuilder.EndsWith(value, false))
            {
                var length = value.Length;

                stringBuilder.Remove(stringBuilder.Length - length, length);

                return true;
            }
        }
        return false;
    }

    //Creds: https://stackoverflow.com/questions/1359948/why-doesnt-stringbuilder-have-indexof-method
    /// <summary>
    /// Returns the index of the text within the StringBuilder or -1 if not found
    /// </summary>        
    /// <param name="text">The string to find</param>
    /// <param name="start">The starting index.</param>
    /// <param name="ignoreCase">if set to <c>true</c> it will ignore case</param>
    public static int IndexOf(this StringBuilder stringBuilder, string text, bool ignoreCase = false, int start = 0)
    {
        if (stringBuilder == null) return -1;

        if (text == null) return -1;

        int index;
        int length = text.Length;

        if (length > stringBuilder.Length) return -1;

        int maxSearchLength = (stringBuilder.Length - length) + 1;

        if (ignoreCase)
        {
            for (int i = start; i < maxSearchLength; ++i)
            {
                if (Char.ToLower(stringBuilder[i]) == Char.ToLower(text[0]))
                {
                    index = 1;
                    while ((index < length) && (Char.ToLower(stringBuilder[i + index]) == Char.ToLower(text[index])))
                        ++index;

                    if (index == length)
                        return i;
                }
            }

            return -1;
        }

        for (int i = start; i < maxSearchLength; ++i)
        {
            if (stringBuilder[i] == text[0])
            {
                index = 1;
                while ((index < length) && (stringBuilder[i + index] == text[index]))
                    ++index;

                if (index == length)
                    return i;
            }
        }

        return -1;
    }

    static Dictionary<string, string> HtmlEncodeReplacements = new Dictionary<string, string>
    {
        { "\"", "&quot;" },
        { "'", "&apos;" },
    };

    public static void HtmlEncodeQuotes(this StringBuilder html, Dictionary<string, string> additionalReplacements = null)
    {
        foreach (var replacement in HtmlEncodeReplacements)
            html.Replace(replacement.Key, replacement.Value);

        if(additionalReplacements != null)
        {
            foreach(var replacement in additionalReplacements)
                html.Replace(replacement.Key, replacement.Value);
        }
    }

    static Dictionary<string, string> HtmlDecodeReplacements = new Dictionary<string, string>
    {
        { "&quot;", "\"" },
        { "&apos;", "'" }
    };

    public static void HtmlDecodeQuotes(this StringBuilder html, Dictionary<string, string> additionalReplacements = null)
    {
        foreach (var replacement in HtmlDecodeReplacements)
            html.Replace(replacement.Key, replacement.Value);

        if(additionalReplacements != null)
        {
            foreach(var replacement in additionalReplacements)
                html.Replace(replacement.Key, replacement.Value);
        }
    }
}
