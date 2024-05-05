#if UNITY_EDITOR

using System;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace TrexB.CipherUtility.Editor
{
    public class KeyIVGeneratorMenu : EditorWindow
    {
        private string _key = "";
        private string _iv = "";

        #region Toolbar
        [MenuItem("TrexB/CipherUtility/Generate Key and IV")]
        private static void ShowWindow()
        {
            var window = GetWindow<KeyIVGeneratorMenu>();

            window.titleContent = new GUIContent("Key and IV Generator");

            // Set minimum size restriction - (minimum width, minimum height)
            window.minSize = new Vector2(500, 170);

            window.Show();
        }
        #endregion

        #region GUI
        // Draws the user interface elements and handles user interaction.
        private void OnGUI()
        {
            GUILayout.Space(10);

            #region Display "Key" and "IV" labels with generated strings
            GUILayout.BeginVertical("box");

            DisplayLabelWithCopyButton("Key:", _key);
            DisplayLabelWithCopyButton("IV:", _iv);

            GUILayout.EndVertical();
            #endregion

            GUILayout.Space(10);

            #region Display Generate Button
            if (GUILayout.Button("Generate", GUILayout.ExpandWidth(true)))
            {
                (_key, _iv) = GenerateKeyAndIV();
            }
            #endregion
        }

        private void DisplayLabelWithCopyButton(string label, string text)
        {
            GUILayout.Label(label);

            GUILayout.BeginHorizontal();

            GUILayout.Label(text);

            // Enable copy button if text is not empty
            GUI.enabled = !string.IsNullOrEmpty(text);

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
        private (string, string) GenerateKeyAndIV()
        {
            using (var aes = Aes.Create())
            {
                // KEY
                aes.GenerateKey();
                #region Convert.ToBase64String Explaination
                // Convert.ToBase64String is a method that takes some data, like text or numbers, and converts it into a special type of text format called Base64.
                // This format is often used to represent binary data, like images or files, as text so that it can be easily stored or transmitted over networks.
                #endregion
                string key = Convert.ToBase64String(aes.Key);

                //IV
                aes.GenerateIV();
                string iv = Convert.ToBase64String(aes.IV);

                return (key, iv);
            }
        }
        #endregion
    } // class
}

#endif