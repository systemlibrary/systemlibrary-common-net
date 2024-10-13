using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net;

internal static class Sha256
{
    const int ResetCounter = 400;
    static int SHA256Counter = ResetCounter;
    static object counterlock = new object();
    static SHA256 _SHA256;
    static SHA256 SHA256
    {
        get
        {
            System.Threading.Interlocked.Increment(ref SHA256Counter);

            if (SHA256Counter > ResetCounter)
            {
                lock (counterlock)
                {
                    if (SHA256Counter > ResetCounter)
                    {
                        var oldRef = _SHA256;

                        _SHA256 = null;

                        _SHA256 = SHA256.Create();

                        SHA256Counter = 0;

                        Task.Run(async () =>
                        {
                            await Task.Delay(1000);
                            oldRef?.Dispose();
                        });
                    }
                }
            }

            return _SHA256 ?? SHA256.Create();
        }
    }

    internal static string Compute(byte[] bytes)
    {
        if (bytes == null) return null;

        if (bytes.Length == 0) return "";

        return BitConverter.ToString(SHA256.ComputeHash(bytes));
    }

    internal static string Compute(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        return BitConverter.ToString(SHA256.ComputeHash(stream));
    }

    internal static async Task<string> ComputeAsync(Stream stream)
    {
        if (stream == null) return null;

        if (!stream.CanRead) return null;

        return BitConverter.ToString(await SHA256.ComputeHashAsync(stream).ConfigureAwait(false));
    }
}
