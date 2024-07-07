using System.Text;

namespace SystemLibrary.Common.Net;

internal static class CryptationKey
{
    internal static byte[] _Key;

    static object KeyLock = new object();

    internal static byte[] Current
    {
        get
        {
            if (_Key == null)
            {
                lock (KeyLock)
                {
                    if (_Key != null) return _Key;

                    var key = CryptationKeyFile.NameHashed;

                    if (key.IsNot())
                    {
                        key = "ABCDEFGHIJKLMNOPQRST123456789011";
                    }
                    _Key = Encoding.UTF8.GetBytes(key.MaxLength(47).Replace("-", ""));
                }
            }
            return _Key;
        }
    }

    internal static string GetExceptionMessage(string cipherText)
    {
        var error = "Could not decrypt value starting with the two chars: " + cipherText.MaxLength(2);
        var hasKeyFile = CryptationKeyFile.NameHashed.Is();
        if (hasKeyFile)
        {
            error += "\nTried decrypt with data key file starting with: " + CryptationKeyFile.NameHashed.MaxLength(5) + "...";
        }
        else    
        {
            error += "\nTried decrypt with key from Library, starting with ABC and IV starting with " + CryptationIV.IV[0];
        }

        return error;
    }
}
