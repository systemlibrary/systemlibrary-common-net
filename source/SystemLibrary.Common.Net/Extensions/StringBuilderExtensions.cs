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
}
