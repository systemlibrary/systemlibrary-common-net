using System.Text;

namespace SystemLibrary.Common.Net;

internal static partial class CryptationKey
{
    internal static byte[] _Key;

    static object Lock = new object();

    internal static string _KeyDirectory;
    internal static string KeyDirectory
    {
        get
        {
            if (_KeyDirectory == null)
            {
                var temp = Current;

                _KeyDirectory ??= "";
            }

            return _KeyDirectory;
        }
    }

    internal static string KeyStart;

    internal static byte[] Current
    {
        get
        {
            if (_Key == null)
            {
                lock (Lock)
                {
                    if (_Key != null) return _Key;

                    _Key = Encoding.UTF8.GetBytes(GetKey());
                }
            }
            return _Key;
        }
    }

    static string GetKey()
    {
        var key = GetKeyFromDataRingKeyFile();

        if (key.IsNot())
        {
            key = GetKeyFromAsmName();
        }

        if (key.IsNot())
        {
            key = "ABCDEFGHIJKLMNOPQRST123456789011";

            KeyStart = key.MaxLength(maxLength: 4);
        }
        else
        {
            KeyStart = key.MaxLength(maxLength: 2);
        }
        return key.ToSha256Hash().MaxLength(47).Replace("-", "");
    }

    internal static string GetExceptionMessage(string cipherText)
    {
        var error = "Could not decrypt value starting with: " + cipherText.MaxLength(4);

        error += "\nTried decrypt with data key starting with: " + KeyStart + "...";

        return error;
    }
}
