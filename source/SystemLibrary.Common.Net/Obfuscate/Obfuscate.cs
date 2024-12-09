using System;

namespace SystemLibrary.Common.Net;

internal static class Obfuscate
{
    static int MaxChar = System.Convert.ToInt32(char.MaxValue);
    static int MinChar = System.Convert.ToInt32(char.MinValue);
    static int RangeSize = MaxChar - MinChar + 1;
    internal static string Convert(string text, int salt, bool deobfuscate)
    {
        if (salt <= 0)
            throw new Exception("Cannot obfuscate a string with a salt of 0 or less");

        if (text == null) return null;
        if (text == "") return "";

        if (deobfuscate)
            salt *= -1;

        var l = text.Length;
        var chars = new char[l];

        int shifted;

        for (int i = 0; i < l; i++)
        {
            shifted = text[i] + salt; 

            if (shifted > MaxChar)
            {
                shifted -= RangeSize;
            }
            else if (shifted < MinChar)
            {
                shifted += RangeSize;
            }

            chars[i] = (char)shifted;
        }

        return new string(chars);
    }
}
