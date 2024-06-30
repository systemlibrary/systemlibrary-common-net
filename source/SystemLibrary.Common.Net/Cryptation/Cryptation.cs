using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace SystemLibrary.Common.Net;

internal static class Cryptation
{
    public static byte[] Encrypt(string text, byte[] key, byte[] iv = null)
    {
        // CREDS: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

        if (text.IsNot()) return text.GetBytes();

        if (iv == null)
            iv = new byte[16];

        byte[] bytes;

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(text);
                    }

                    bytes = memoryStream.ToArray();
                }
            }
        }
        return bytes;
    }

    static ConcurrentDictionary<string, string> DecryptedShelf = new ConcurrentDictionary<string, string>();

    static object DecryptedShelfLock = new object();

    public static string Decrypt(string cipherText, byte[] key, byte[] iv, bool onErrorOutputKeyParts)
    {
        // CREDS: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

        if (cipherText.IsNot()) return cipherText;

        if (iv == null)
            iv = new byte[16];

        var shelfKey = key[0] + key[1] + iv.Length + cipherText + key.Length;

        if (DecryptedShelf.ContainsKey(shelfKey)) return DecryptedShelf[shelfKey];

        lock (DecryptedShelfLock)
        {
            if (DecryptedShelf.ContainsKey(shelfKey)) return DecryptedShelf[shelfKey];

            byte[] buffer = Convert.FromBase64String(cipherText);

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                var value = streamReader.ReadToEnd();

                                DecryptedShelf.TryAdd(shelfKey, value);

                                return value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = CryptationKey.GetExceptionMessage(cipherText, onErrorOutputKeyParts);

                throw new Exception(message, ex);
            }
        }
    }
}
