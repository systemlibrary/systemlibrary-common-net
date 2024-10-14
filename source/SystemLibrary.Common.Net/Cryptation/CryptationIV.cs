namespace SystemLibrary.Common.Net;

static internal class CryptationIV
{
    internal static byte[] Current
    {
        get
        {
            return Randomness.Bytes(16);
        }
    }
}
