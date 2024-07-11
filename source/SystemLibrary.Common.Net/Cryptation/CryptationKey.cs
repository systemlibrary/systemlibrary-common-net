using System.Text;

namespace SystemLibrary.Common.Net;

internal static partial class CryptationKey
{
    internal static byte[] _Key;

    static object Lock = new object();

    internal static string _KeyFileName;

    internal static string _KeyDirectory;

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
        var key = TryGetKeyFromDataRingKeyFile();

        if (key.IsNot())
        {
            key = TryGetKeyFromAppNameOrAsmName();

            if(key != null)
                Debug.Write("Encryption Key is based on app name");
        }

        if (key.IsNot())
        {
            key = "ABCDEFGHIJKLMNOPQRST123456789011";

            Debug.Write("Encryption Key is default from Library, call service.AddDataProtection to use a unique");

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
