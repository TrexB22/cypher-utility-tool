using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace TrexB.CipherUtility.Security
{
    public class DecryptionManager
    {
        public static T ReadEncryptedData<T>(string key, string iv,string path)
        {
            // Streams deals with bytes not strings
            // This will return all the data which is already encrypted in the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(path);

            using Aes aes = Aes.Create();

            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            using ICryptoTransform cryptoTransform = aes.CreateDecryptor(
                aes.Key,
                aes.IV
            );

            #region Explanation
            // MemoryStream is like a container in your computer's memory where you can temporarily store data.
            // It's similar to a file that you can write to and read from, but instead of being stored on your hard drive, it's stored in your computer's RAM,
            // which is much faster to access.
            // Once you're done using the MemoryStream, you can empty it out and free up the memory it was using.
            #endregion
            using MemoryStream decryptionStream = new MemoryStream(fileBytes);

            using CryptoStream cryptoStream = new CryptoStream(
                decryptionStream, // We are passing the stream that we want to read from
                cryptoTransform, // This will decrypt the data to normal bytes
                CryptoStreamMode.Read // Because we want to read the data
            );

            // We pass cryptoStream because we want to read data from it
            using StreamReader reader = new StreamReader(cryptoStream);

            // This will give us a string with all of our data decrypted
            string result = reader.ReadToEnd();

            Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or iv): {result}");

            return JsonUtility.FromJson<T>(result);
        }
    } // class
}