#if UNITY_EDITOR

using System;
using System.Text;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace TrexB.CipherUtility.Editor
{
    public class KeyIVGeneratorMenu : EditorWindow
    {
        #region Variables
        private string _password = "";
        private string _key = "";
        private string _iv = "";

        private bool _isKeyEmpty = true;
        private bool _isIvEmpty = true;
        #endregion

        #region Toolbar
        [MenuItem("TrexB/CipherUtility/Generate Key and IV")]
        private static void ShowWindow()
        {
            var window = GetWindow<KeyIVGeneratorMenu>();
            window.titleContent = new GUIContent("Key and IV Generator");
            // Set minimum size restriction - (minimum width, minimum height)
            window.minSize = new Vector2(500, 210);
            //// Set the default size of the window - (x, y, width, height)
            //window.position = new Rect(100, 100, 400, 200);
            window.Show();
        }
        #endregion

        #region GUI
        // Draws the user interface elements and handles user interaction.
        private void OnGUI()
        {
            GUILayout.Space(10);

            // Draw UI elements
            GUILayout.Label("Enter Password:");
            GUILayout.Space(1);
            _password = EditorGUILayout.TextField(_password);

            GUILayout.Space(10);

            // Display "Key" and "IV" labels with generated strings
            GUILayout.BeginVertical("box");
            // Clear labels if empty
            _key = _isKeyEmpty ? "" : _key;
            _iv = _isIvEmpty ? "" : _iv;

            DisplayLabelWithCopyButton("Key:", _key, ref _isKeyEmpty);
            DisplayLabelWithCopyButton("IV:", _iv, ref _isIvEmpty);
            GUILayout.EndVertical();

            GUILayout.Space(10);

            // Generate button
            if (GUILayout.Button("Generate", GUILayout.ExpandWidth(true)))
            {
                if (string.IsNullOrEmpty(_password))
                {
                    // Error message if password is empty
                    Debug.LogError("Generating the key requires a password!");
                    // Clear key and iv labels
                    _key = "";
                    _iv = "";
                    // Set flags to true
                    _isKeyEmpty = true;
                    _isIvEmpty = true;
                }
                else
                {
                    // Call function to generate key and iv
                    (_key, _iv) = GenerateKeyAndIV(_password);
                    // Update flags based on generated strings
                    _isKeyEmpty = string.IsNullOrEmpty(_key);
                    _isIvEmpty = string.IsNullOrEmpty(_iv);
                }
            }
        } // OnGUI

        private void DisplayLabelWithCopyButton(string label, string text, ref bool isEmpty)
        {
            GUILayout.Label(label);
            GUILayout.BeginHorizontal();
            GUILayout.Label(text);
            // Enable copy button if text is not empty
            GUI.enabled = !isEmpty;
            if (GUILayout.Button("Copy", GUILayout.Width(50)))
            {
                // Copy text to clipboard
                GUIUtility.systemCopyBuffer = text;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }
        #endregion

        #region Logic for creating a Key and IV
        private (string, string) GenerateKeyAndIV(string password)
        {
            using (var aes = Aes.Create())
            {
                #region KEY
                aes.GenerateKey();

                int validKeySize = aes.Key.Length;

                byte[] keyBytes = GenerateKeyFromPassword(password, validKeySize);

                #region Convert.ToBase64String Explaination
                // Convert.ToBase64String is a method that takes some data, like text or numbers, and converts it into a special type of text format called Base64.
                // This format is often used to represent binary data, like images or files, as text so that it can be easily stored or transmitted over networks.
                string key = Convert.ToBase64String(keyBytes);
                #endregion
                #endregion

                #region IV
                aes.GenerateIV();
                string iv = Convert.ToBase64String(aes.IV);
                #endregion

                return (key, iv);
            }
        } // GenerateKeyAndIV

        #region Converting a password into a suitable AES key
        // Ensure the AES key byte-array is the right size, otherwise, AES will reject it.
        private byte[] GenerateKeyFromPassword(string password, int desiredKeySize)
        {
            var keyBytes = Encoding.UTF8.GetBytes(password);

            if (keyBytes.Length != desiredKeySize)
            {
                var newKeyBytes = new byte[desiredKeySize];

                #region Array.Copy function parameter explaination
                // Array.Copy()
                // 1st parameter - source array meaning konse array se copy krna hai
                // 2nd parameter - destination array meaning konse arry mai copy krna hai
                // 3rd parameter - length meaning kitna number of elements copy krna hai
                #endregion
                #region Logic explaination
                // First Case - If the keyBytes array is longer than the newKeyBytes array, to jitna newKeyBytes accommodate kr skta hai utna elements wo copy kr
                // lega from the keyBytes array to fit the required length, even if it's longer initially.
                // Second Case - If the newKeyBytes array is longer than the keyBytes array, it copies all bytes from keyBytes to newKeyBytes,
                // padding any remaining space in newKeyBytes with zeros.
                // This ensures that the key is adjusted to fit the required length, even if it's shorter initially.
                #endregion
                Array.Copy(keyBytes, newKeyBytes, Math.Min(keyBytes.Length, newKeyBytes.Length));

                keyBytes = newKeyBytes;
            }

            return keyBytes;
        }
        #endregion
        #endregion
    } // class
}

#endif