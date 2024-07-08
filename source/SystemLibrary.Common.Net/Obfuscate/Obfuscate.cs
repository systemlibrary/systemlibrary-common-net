using System;

namespace SystemLibrary.Common.Net;

internal static class Obfuscate
{
    internal static string Convert(string text, int salt, bool deobfuscate)
    {
        if (salt <= 0)
            throw new Exception("Cannot obfuscate a string with a salt of 0 or less");

        if (text == null) return null;
        if (text == "") return "";

        var maxChar = System.Convert.ToInt32(char.MaxValue);
        var minChar = System.Convert.ToInt32(char.MinValue);

        var span = text.AsSpan();

        if (deobfuscate)
            salt *= -1;

        var l = text.Length;
        var chars = new char[l];

        for (int i = 0; i < span.Length; i++)
        {
            chars[i] = (char)(span[i] - salt);

            //NOTE: Odds that salt + char is actually out of bounds is rare, or "never", so could remove? Do we support all chars like that - what are the last 5K chars... and a huge salt? ...
            if (chars[i] > maxChar)
                chars[i] -= (char)(chars[i] - maxChar);

            else if (chars[i] < minChar)
                chars[i] = (char)(chars[i] + maxChar);
        }

        return new string(chars);
    }
}
