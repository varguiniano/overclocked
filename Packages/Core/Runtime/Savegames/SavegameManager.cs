using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Configuration;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

#if UNITY_WEBGL && !UNITY_EDITOR
using Varguiniano.Core.Runtime.WebGL;
#endif

namespace Varguiniano.Core.Runtime.Savegames
{
    /// <summary>
    /// Manager that handles savegames.
    /// This class has a lot of linq, but as these operations are sporadic, there's no problem.
    /// </summary>
    public class SavegameManager : Service<SavegameManager>
    {
        /// <summary>
        /// Library of objects to save and load.
        /// </summary>
        public SavableObjectsLibrary Library;

        /// <summary>
        /// Flag that tells if there is an existing save.
        /// </summary>
        public static bool SaveExists => Directory.Exists(SavesPath) && Directory.GetDirectories(SavesPath).Length > 0;

        /// <summary>
        /// Get the path to the last save.
        /// </summary>
        public static string LastSavePath =>
            Directory.EnumerateDirectories(SavesPath).OrderByDescending(x => x).First();

        /// <summary>
        /// Current time in the format used for savegames.
        /// </summary>
        public static string CurrentTimeInSaveFormat => DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");

        /// <summary>
        /// Saves path.
        /// </summary>
        public static string SavesPath => Application.persistentDataPath + "/Saves/";

        /// <summary>
        /// Reference to the configuration.
        /// </summary>
        private SavesConfiguration config;

        /// <summary>
        /// Create the directory if it doesn't exist.
        /// </summary>
        private void OnEnable()
        {
            ConfigurationManager.OnServiceReady += ConfigManagerReady;
            if (!Directory.Exists(SavesPath)) Directory.CreateDirectory(SavesPath);
        }

        /// <summary>
        /// Called when the config manager is ready.
        /// </summary>
        /// <param name="manager">Reference to the manager.</param>
        private void ConfigManagerReady(ConfigurationManager manager)
        {
            config = manager.GetConfig<SavesConfiguration>();
            manager.ConfigurationUpdated += ConfigUpdated;
            Ready();
        }

        /// <summary>
        /// Called when a config file is updated.
        /// </summary>
        /// <param name="configuration">The config updated.</param>
        private void ConfigUpdated(ConfigData configuration)
        {
            if (!(configuration is SavesConfiguration newConfig)) return;
            config = newConfig;
        }

        /// <summary>
        /// SAves the game.
        /// </summary>
        public void SaveGame()
        {
            string savegameName = CurrentTimeInSaveFormat;
            string path = SavesPath + savegameName;
            Logger.LogInfo("Saving game to " + path, this);
            SaveGame(path);
        }

        /// <summary>
        /// Saves the game.
        /// </summary>
        /// <param name="path">Save game path.</param>
        private void SaveGame(string path)
        {
            Utils.DeleteDirectory(path);
            Directory.CreateDirectory(path);

            for (int i = 0; i < Library.SavableObjects.Count; i++)
                File.WriteAllText(path + "/" + i + ".savo", Library.SavableObjects[i].Save());

            CleanUpOldSaves();

            #if UNITY_WEBGL && !UNITY_EDITOR
            Logger.LogInfo("WebGL file sync in progress.", this);
            WebGlUtils.SyncFiles();
            #endif
        }

        /// <summary>
        /// Loads the latest game if any.
        /// </summary>
        public void LoadGame()
        {
            if (!SaveExists)
            {
                Logger.LogError("There is no game to load in " + SavesPath + ".", this);
                return;
            }

            IEnumerable<string> directories = Directory.EnumerateDirectories(SavesPath).OrderByDescending(x => x);
            string directory = directories.First();
            Logger.LogInfo("Loading game from " + directory, this);
            LoadGame(directory);
        }

        /// <summary>
        /// Loads the game.
        /// </summary>
        /// <param name="path">Savegame path.</param>
        private void LoadGame(string path)
        {
            if (!Directory.Exists(path)) return;

            for (int i = 0; i < Library.SavableObjects.Count; i++)
            {
                string filePath = path + "/" + i + ".savo";
                if (File.Exists(filePath)) Library.SavableObjects[i].Load(File.ReadAllText(filePath));
            }
        }

        /// <summary>
        /// Delete old save files if the config says so.
        /// </summary>
        private void CleanUpOldSaves()
        {
            if (!SaveExists) return;
            if (config.SavesToKeep == 0) return; // 0 = infinity

            List<string> directories = Directory.GetDirectories(SavesPath).OrderBy(x => x).ToList();

            if (directories.Count <= config.SavesToKeep) return;

            int directoriesToDelete = directories.Count - config.SavesToKeep;

            for (int i = 0; i < directoriesToDelete; ++i) Utils.DeleteDirectory(directories[i]);
        }
    }
}