using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Varguiniano.Core.Runtime.Localization;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

namespace Varguiniano.Core.Runtime.Common
{
    /// <summary>
    ///     Class with utility functions and extensions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        ///     Simplify an angle so that it is between 0º and 360º.
        /// </summary>
        /// <param name="angle">The original angle.</param>
        /// <returns>Its value between 0º and 360º.</returns>
        public static float SimplifyAngle(this float angle)
        {
            while (angle > 360) angle -= 360;
            while (angle < 0) angle += 360;
            return angle;
        }

        /// <summary>
        /// Converts a float to a string with two optional decimal digits.
        /// </summary>
        /// <param name="number">Original number.</param>
        /// <returns>The number in string value.</returns>
        public static string ToStringTwoOptionalDecimals(this float number) => number.ToString("0.##");

        /// <summary>
        /// Removes the last char of a string.
        /// </summary>
        /// <param name="text">The string to remove the text from.</param>
        /// <returns>The string without the last char.</returns>
        public static string RemoveLastChar(this string text) => text.Remove(text.Length - 1);

        /// <summary>
        /// "Decrypts" a string from base64.
        /// </summary>
        /// <param name="data">String to decrypt.</param>
        /// <returns>Decrypted string.</returns>
        public static string FromBase64(this string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// "Encrypts" a string to base64.
        /// </summary>
        /// <param name="data">String to encrypt.</param>
        /// <returns>Encrypted string.</returns>
        public static string ToBase64(this string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Compresses a directory into Zip.
        /// </summary>
        /// <param name="path">Directory to compress.</param>
        /// <param name="compressedPath">Compressed file.</param>
        /// <returns>True if it was successful.</returns>
        public static bool CompressDirectory(string path, out string compressedPath)
        {
            if (Directory.Exists(path))
            {
                compressedPath = path + ".zip";
                ZipFile.CreateFromDirectory(path, compressedPath);
                return true;
            }

            Logger.LogError("Directory " + path + " doesn't exist!", "CoreUtils");
            compressedPath = "";
            return false;
        }

        /// <summary>
        /// Decompress a zip file into the given path.
        /// </summary>
        /// <param name="zipPath">Path to the zip file.</param>
        /// <param name="targetPath">Path to decompress to.</param>
        /// <returns>True if it was successful.</returns>
        public static bool DecompressDirectory(string zipPath, string targetPath)
        {
            if (File.Exists(zipPath) || Path.GetExtension(zipPath) != "zip")
            {
                ZipFile.ExtractToDirectory(zipPath, targetPath);
                return true;
            }

            Logger.LogError("File " + zipPath + " doesn't exist or it's not a zip file.", "CoreUtils");
            return false;
        }

        /// <summary>
        /// Checks if a game object has the DontDestroyOnLoadFlag.
        /// </summary>
        /// <param name="gameObject">The game object to check.</param>
        /// <returns>True if it won't be destroyed.</returns>
        public static bool IsDontDestroyOnLoad(this GameObject gameObject) => gameObject.scene.buildIndex == -1;

        /// <summary>
        /// Copy the content of a directory to another.
        /// </summary>
        /// <param name="source">Source directory.</param>
        /// <param name="target">Target directory.</param>
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.Exists) target.Create();

            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

            foreach (FileInfo file in source.GetFiles()) file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }

        /// <summary>
        /// Wrapper for recursively deleting a directory that (almost) works around permissions and system delays.
        /// Refer to: https://stackoverflow.com/questions/1157246/unauthorizedaccessexception-trying-to-delete-a-file-in-a-folder-where-i-can-dele
        /// to: https://stackoverflow.com/questions/34981143/is-directory-delete-create-synchronous
        /// and to: https://stackoverflow.com/questions/755574/how-to-quickly-check-if-folder-is-empty-net
        /// </summary>
        /// <param name="targetDir">The directory to delete.</param>
        public static void DeleteDirectory(string targetDir)
        {
            if (!Directory.Exists(targetDir)) return;
            File.SetAttributes(targetDir, FileAttributes.Directory);
            File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
                while (File.Exists(file)) Thread.Sleep(100);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
                while (Directory.Exists(dir)) Thread.Sleep(100);
            }

            while (Directory.EnumerateFileSystemEntries(targetDir).Any()) Thread.Sleep(100);
            Directory.Delete(targetDir, false);
            while (Directory.Exists(targetDir)) Thread.Sleep(100);
        }

        /// <summary>
        /// Closes the game in a more elegant way than Alt+F4.
        /// </summary>
        public static void CloseGame()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    /// <summary>
    /// String bool pair for serializable "dictionaries".
    /// </summary>
    [Serializable]
    public struct StringBoolPair
    {
        /// <summary>
        /// String key.
        /// </summary>
        public string Key;

        /// <summary>
        /// Bool value.
        /// </summary>
        public bool Value;

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringBoolPair(string key, bool value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// String int pair for serializable "dictionaries".
    /// </summary>
    [Serializable]
    public struct StringIntPair
    {
        /// <summary>
        /// String key.
        /// </summary>
        public string Key;

        /// <summary>
        /// Int value.
        /// </summary>
        public int Value;

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringIntPair(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// LocalizableString int pair for serializable "dictionaries".
    /// </summary>
    [Serializable]
    public struct LocalizableStringIntPair
    {
        /// <summary>
        /// LocalizableString key.
        /// </summary>
        public LocalizableString Key;

        /// <summary>
        /// Int value.
        /// </summary>
        public int Value;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizableStringIntPair(LocalizableString key, int value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// String string pair for serializable "dictionaries".
    /// </summary>
    [Serializable]
    public struct StringStringPair
    {
        /// <summary>
        /// String key.
        /// </summary>
        public string Key;

        /// <summary>
        /// String value.
        /// </summary>
        public string Value;

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringStringPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}