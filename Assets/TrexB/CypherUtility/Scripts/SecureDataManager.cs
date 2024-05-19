using System;
using System.IO;
using UnityEngine;
using TrexB.CipherUtility.Security;

namespace TrexB.CipherUtility
{
    public static class SecureDataManager
    {
        // Constants by default are static in nature, that's why we can't use static keyword with const.
        private const string _KEY = "B9+fuTANTgqOr+eNSrdZ6xcUntdBAUzxgCqdv2j6iHg=";
        private const string _IV = "cil6P5eP3cfjS1DVki/IhQ==";

        #region Save Data
        /// <summary>
        /// Saves the provided data securely to a file.
        /// </summary>
        /// <typeparam name="T">The type of data to be saved.</typeparam>
        /// <param name="outputFileNameWithExtension">The name of the file to save the data to, including the file extension.</param>
        /// <param name="dataToSave">The data to be saved.</param>
        /// <returns>True if the data is saved successfully; otherwise, false.</returns>
        public static bool SaveData<T>(string outputFileNameWithExtension, T dataToSave)
        {
            string path = GetFilePath(outputFileNameWithExtension);

            // JsonUtility.ToJson(): This method returns a string with JSON representation.
            string jsonData = JsonUtility.ToJson(dataToSave);

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

                SecureDataWriter.WriteEncryptedData(_KEY, _IV, stream, jsonData);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
                return false;
            }
        } // SaveData
        #endregion

        #region Load Data
        /// <summary>
        /// Loads and decrypts data of type T from a file.
        /// </summary>
        /// <typeparam name="T">The type of data to be loaded.</typeparam>
        /// <param name="outputFileNameWithExtension">The name of the file containing the data to load, including the file extension.</param>
        /// <returns>The decrypted data of type T.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
        /// <exception cref="Exception">Thrown if an error occurs during data loading and decryption.</exception>
        public static T LoadData<T>(string outputFileNameWithExtension)
        {
            string path = GetFilePath(outputFileNameWithExtension);

            if (!File.Exists(path))
            {
                Debug.LogError($"Cannot load file at {path}. File does not exist!");

                // This is import when we are loading the data, we want to receive the data back, so if can't get the data back, we throw an exception
                // This way, any developer can put a try catch block around and appropriately handle those scenarios.
                throw new FileNotFoundException($"{path} does not exist!");
            }

            try
            {
                T data = SecureDataReader.ReadEncryptedData<T>(_KEY, _IV, path);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                throw;
            }
        } // LoadData
        #endregion

        #region Common Functions
        private static string GetFilePath(string fileNameWithExtension)
        {
            string path;

            #if UNITY_EDITOR
            {
                // For testing purpose
                // Path.Combine for path creation to ensure platform-independent path formatting.
                path = Path.Combine(Application.dataPath, fileNameWithExtension);
            }
            #else
            {
                path = Path.Combine(Application.persistentDataPath, fileNameWithExtension);
            }
            #endif

            return path;
        }
        #endregion
    } // class
}