using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace TrexB.CipherUtility
{
    public static class SecureDataManager
    {
        // Constants by default are static in nature, that's why we can't use static keyword with const.
        private const string KEY = "B9+fuTANTgqOr+eNSrdZ6xcUntdBAUzxgCqdv2j6iHg=";
        private const string IV = "cil6P5eP3cfjS1DVki/IhQ==";

        #region Encrypt And Save Data
        public static bool SaveData<T>(string relativePath, T rawData)
        {
            string path;

            #if UNITY_EDITOR
            {
                // Testing
                 path = Application.dataPath + relativePath;
            }
            #else
            {
                path = Application.persistentDataPath + relativePath;
            }
            #endif

            // JsonUtility.ToJson(): This method returns a string with JSON representation.
            string jsonData = JsonUtility.ToJson(rawData);

            try
            {
                if (File.Exists(path))
                {
                    Debug.Log("Data exists. Deleting old file and writing a new one!");
                    File.Delete(path);
                }
                else
                {
                    Debug.Log("Writing file for the first time.");
                }

                using FileStream stream = File.Create(path);

                WriteEncryptedData(stream, jsonData);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
                return false;
            }
        } // SaveData

        private static void WriteEncryptedData(FileStream stream, string jsonData)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(jsonData);

            // This will create a new instance of the AES Provider, which automatically creates a new KEY and IV, each time when its new instance is created.
            using Aes aes = Aes.Create();

            // Overwriting given KEY and IV with our predefined values.
            aes.Key = Convert.FromBase64String(KEY);
            aes.IV = Convert.FromBase64String(IV);

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
#endregion

        #region Decrypt And Load Data
        public static T LoadData<T>(string relativePath)
        {
            string path;

            #if UNITY_EDITOR
            {
                // Testing
                path = Application.dataPath + relativePath;
            }
            #else
            {
                path = Application.persistentDataPath + relativePath;
            }
            #endif

            if (!File.Exists(path))
            {
                Debug.LogError($"Cannot load file at {path}. File does not exist!");

                // This is import when we are loading the data, we want to receive the data back, so if can't get the data back, we throw an exception
                // This way, any developer can put a try catch block around and appropriately handle those scenarios.
                throw new FileNotFoundException($"{path} does not exist!");
            }

            try
            {
                T data = ReadEncryptedData<T>(path);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                throw;
            }
        } // LoadData

        private static T ReadEncryptedData<T>(string path)
        {
            // Streams deals with bytes not strings
            // This will return all the data which is already encrypted in the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(path);

            using Aes aes = Aes.Create();

            aes.Key = Convert.FromBase64String(KEY);
            aes.IV = Convert.FromBase64String(IV);

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
        #endregion
    } // class
}