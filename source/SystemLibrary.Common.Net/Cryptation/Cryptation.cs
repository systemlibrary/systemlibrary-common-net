﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;

namespace SystemLibrary.Common.Net;

internal static class Cryptation
{
    public static byte[] Encrypt(string text, byte[] key, byte[] iv, bool addIV)
    {
        // CREDS: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

        if (text.IsNot()) return text.GetBytes();

        if (iv == null)
        {
            if (!addIV)
                iv = new byte[16];
            else
                iv = CryptationIV.Current;
        }

        byte[] bytes;

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                if (addIV)
                    memoryStream.Write(iv, 0, iv.Length);

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
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

    public static string Decrypt(string cipherText, byte[] key, byte[] iv, bool addedIV)
    {
        // CREDS: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

        if (cipherText.IsNot()) return cipherText;

        string shelfKey = null;
        if (cipherText.Length > 148)
        {
            shelfKey = cipherText.MaxLength(148);
            if (DecryptedShelf.ContainsKey(shelfKey)) return DecryptedShelf[shelfKey];
        }
        else
        {
            if (DecryptedShelf.ContainsKey(cipherText)) return DecryptedShelf[cipherText];
        }
        lock (DecryptedShelfLock)
        {
            if (shelfKey != null)
            {
                if (DecryptedShelf.ContainsKey(shelfKey)) return DecryptedShelf[shelfKey];
            }
            else
            {
                if (DecryptedShelf.ContainsKey(cipherText)) return DecryptedShelf[cipherText];
            }

            byte[] buffer = Convert.FromBase64String(cipherText);
            if (iv == null)
            {
                iv = new byte[16];

                if (addedIV)
                {
                    Array.Copy(buffer, iv, 16);

                    byte[] temp = new byte[buffer.Length - 16];
                    Array.Copy(buffer, 16, temp, 0, temp.Length);
                    buffer = temp;
                    temp = null;
                }
            }

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
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                var value = streamReader.ReadToEnd();

                                if (shelfKey != null)
                                {
                                    DecryptedShelf.TryAdd(shelfKey, value);
                                }
                                else
                                {
                                    DecryptedShelf.TryAdd(cipherText, value);
                                }

                                return value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = CryptationKey.GetExceptionMessage(cipherText);
                throw new Exception(message, ex);
            }
        }
    }
}
