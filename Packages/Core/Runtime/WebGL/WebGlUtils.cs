#if UNITY_WEBGL
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Localization;
using Varguiniano.Core.Runtime.Savegames;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;
#endif

namespace Varguiniano.Core.Runtime.WebGL
{
    /// <summary>
    /// Class that uses javascript library to handle some browser utils.
    /// </summary>
    public static class WebGlUtils
    {
        #if UNITY_WEBGL
        /// <summary>
        /// Function to sync files in the persistent data path.
        /// </summary>
        [DllImport("__Internal")]
        // ReSharper disable once UnusedMember.Global
        public static extern void SyncFiles();

        /// <summary>
        /// Download a byte array.
        /// </summary>
        [DllImport("__Internal")]
        private static extern void DownloadFile(byte[] array, int byteLength, string fileName);

        /// <summary>
        /// Allow the player to upload a file from the browser.
        /// </summary>
        [DllImport("__Internal")]
        private static extern void GetFileFromBrowser(string objectName,
                                                      string callbackFuncName,
                                                      string filesToAccept,
                                                      string chooseFileText,
                                                      string cancelText,
                                                      string noFileSelectedText,
                                                      string cancelledText);

        /// <summary>
        /// Save and download the savegame.
        /// </summary>
        public static void SaveAndDownloadSaveGame() => SavegameManager.OnServiceReady += SaveAndDownloadSaveGame;

        /// <summary>
        /// Save and download the savegame.
        /// </summary>
        private static void SaveAndDownloadSaveGame(SavegameManager service)
        {
            service.SaveGame();

            if (Utils.CompressDirectory(SavegameManager.LastSavePath, out string compressedPath))
            {
                DownloadFile(compressedPath);
                File.Delete(compressedPath);
                return;
            }

            Logger.LogInfo("Error compressing savegame.", "WebGLUtils");
        }

        /// <summary>
        /// Download a file.
        /// Make sure the file exists before calling this.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        public static void DownloadFile(string path)
        {
            Logger.LogInfo("Downloading file " + path + ".", "WebGLUtils");
            byte[] array = File.ReadAllBytes(path);
            DownloadFile(array, array.Length, Path.GetFileName(path));
            File.Delete(path);
        }

        /// <summary>
        /// Upload and load a savegame.
        /// </summary>
        public static void UploadAndLoadSavegame()
        {
            JsCallbackReceiver.Instance.FileReceived += SaveGameReceived;
            GetFileFromUserAsync(FileType.Zip);
        }

        /// <summary>
        /// Called when the savegame is received.
        /// </summary>
        /// <param name="file">The savegame data.</param>
        private static void SaveGameReceived(byte[] file)
        {
            JsCallbackReceiver.Instance.FileReceived -= SaveGameReceived;
            SavegameManager manager = null;
            SavegameManager.OnServiceReady += service => manager = service;
            string temporalSavePath = Application.persistentDataPath + "/TempSave.zip";
            File.WriteAllBytes(temporalSavePath, file);

            Utils.DecompressDirectory(temporalSavePath,
                                      SavegameManager.SavesPath + SavegameManager.CurrentTimeInSaveFormat);

            while (manager == null) Logger.LogInfo("Waiting for saves manager to be ready.", "WebGLUtils");
            manager.LoadGame();
            File.Delete(temporalSavePath);
        }

        /// <summary>
        /// Allows the player to upload a file.
        /// </summary>
        /// <param name="fileType">Type of sile to upload.</param>
        private static void GetFileFromUserAsync(FileType fileType)
        {
            if (JsCallbackReceiver.Instance == null) return; // Make sure the callback receiver is ready.

            Logger.LogInfo("Calling Js to get file from browser.", "WebGLUtils");

            GetFileFromBrowser("[Singleton] JsCallbackReceiver",
                               "ReceiveFile",
                               FileTypeToString(fileType),
                               new LocalizableString {Key = "webgl_choose_file"}.LocalizedValue,
                               new LocalizableString {Key = "webgl_cancel"}.LocalizedValue,
                               new LocalizableString {Key = "webgl_no_file_selected"}.LocalizedValue,
                               new LocalizableString {Key = "webgl_cancelled"}.LocalizedValue);
        }

        /// <summary>
        /// Translate the file type to the string html needs.
        /// </summary>
        /// <param name="type">The type.</param>
        // ReSharper disable once UnusedMember.Local
        private static string FileTypeToString(FileType type)
        {
            switch (type)
            {
                case FileType.Txt: return ".txt";
                case FileType.Zip: return ".zip";
            }

            return "*";
        }

        #endif
    }

    /// <summary>
    /// Enum of files to be retrieved by webgl.
    /// </summary>
    public enum FileType
    {
        Txt,
        Zip
    }
}