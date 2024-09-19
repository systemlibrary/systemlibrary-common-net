using System;
using System.Text;

namespace SystemLibrary.Common.Net;

/// <summary>
/// Shortcut for generating random variables through System.Random
/// </summary>
public static class Randomness
{
    static Random R;

    static char[] Chars;

    static int CharsLength;

    static Randomness()
    {
        R = new Random((DateTime.Now.Second * 1000) + DateTime.Now.Millisecond);

        Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        CharsLength = Chars.Length;
    }

    /// <summary>
    /// Generate a random Integer
    /// </summary>
    /// <returns>Integer >= 0</returns>
    public static int Int(int maxValue = int.MaxValue)
    {
        return R.Next(0, maxValue);
    }

    /// <summary>
    /// Generate a random Integer
    /// </summary>
    /// <returns>Integer >= 0</returns>
    public static int Int(int minValue, int maxValue)
    {
        return R.Next(minValue, maxValue);
    }

    /// <summary>
    /// Generate a random Byte array
    /// </summary>
    /// <returns>Byte array filled with random int's</returns>
    public static byte[] Bytes(int length = 16)
    {
        var bytes = new byte[length];

        R.NextBytes(bytes);

        return bytes;
    }

    /// <summary>
    /// Generate a random string
    /// </summary>
    /// <returns>Returns a string of length</returns>
    public static string String(int length = 6)
    {
        var result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            result.Append(Chars[R.Next(CharsLength)]);
        }
        return result.ToString();
    }
}
