using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace SystemLibrary.Common.Net;

internal static class Cryptation
{
    public static string DevelopmentCryptationKey;

    static byte[] _Dummy;
    static byte[] Dummy
    {
        get
        {
            if (_Dummy == null)
                _Dummy = "1234567890123456".GetBytes();

            return _Dummy;
        }
    }

    static void ValidateKey(ref byte[] key)
    {
        if(key.Length < 16)
        {
            var temp = new byte[16];
            for(int i = 0; i < temp.Length; i++)
            {
                if (i < key.Length)
                    temp[i] = key[i];
                else
                    temp[i] = Dummy[i];
            }
            key = temp;
        }
    }

    public static string Encrypt(string text, byte[] key)
    {
        // CREDS: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
        if (text.IsNot()) return text;

        ValidateKey(ref key);

        byte[] iv = new byte[16];
        byte[] array;

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

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    static ConcurrentDictionary<string, string> DecryptedShelf = new ConcurrentDictionary<string, string>();

    static object DecryptedShelfLock = new object();

    public static string Decrypt(string cipherText, byte[] key, bool onErrorOutputKeyParts)
    {
        // CREDS: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

        if (cipherText.IsNot()) return cipherText;

        ValidateKey(ref key);

        var shelfKey = cipherText + key.Length;

        if (DecryptedShelf.ContainsKey(shelfKey)) return DecryptedShelf[shelfKey];

        lock (DecryptedShelfLock)
        {
            if (DecryptedShelf.ContainsKey(shelfKey)) return DecryptedShelf[shelfKey];

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    try
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
                    catch(Exception ex)
                    {
                        var message = CryptationKey.GetExceptionMessage(cipherText, onErrorOutputKeyParts);

                        throw new Exception(message, ex);
                    }
                }
            }
        }
    }
}
