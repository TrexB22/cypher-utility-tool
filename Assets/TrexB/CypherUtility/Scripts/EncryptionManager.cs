using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace TrexB.CipherUtility.Security
{
    public static class EncryptionManager
    {
        public static void WriteEncryptedData(string key, string iv,FileStream stream, string jsonData)
        {
            // Encoding.UTF8: Converting characters into binary format
            byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

            // This will create a new instance of the AES Provider, which automatically creates a new KEY and IV, each time when its new instance is created.
            using Aes aes = Aes.Create();

            // Overwriting given KEY and IV with our predefined values.
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            // ICryptoTransform: This is responsible for encrypting data with our predefines KEY and IV.
            // aes.CreateEncryptor() because we are encrypting data
            using ICryptoTransform cryptoTransform = aes.CreateEncryptor();

            using CryptoStream cryptoStream = new CryptoStream(
                stream, // This will write data to the stream we passed in
                cryptoTransform, // This will encrypt data for us
                CryptoStreamMode.Write // Because we are gonna write data to this stream
            );

            // Write(): This functions expects byte[] i.e bytes not strings.
            cryptoStream.Write(dataBytes);
        }
    } // class
}