using System.Text;

namespace SystemLibrary.Common.Net;

static internal class CryptationIV
{
    internal static byte[] IV = new byte[16];
 
    internal static void SetIV(string ivBasedOnData)
    {
        Dump.Write(ivBasedOnData);
        var ivBytes = Encoding.UTF8.GetBytes(ivBasedOnData);

        for (int i = 0; i < IV.Length; i++)
        {
            IV[i] = ivBytes[i];
        }
    }
}
