using System;
using System.IO;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

#if UNITY_WEBGL && !UNITY_EDITOR
using Varguiniano.Core.Runtime.WebGL;
#endif

namespace Varguiniano.Core.Runtime.Configuration
{
    /// <summary>
    /// Abstract class for creating a configuration file that can be used by the application.
    /// TODO: How does this whole config folder stuff work on non-standalone/non-WebGL targets?
    /// </summary>
    /// <typeparam name="T">The type of configuration data.</typeparam>
    public abstract class ConfigFile<T> : ConfigFile where T : ConfigData
    {
        /// <summary>
        /// Reference to the configuration data.
        /// </summary>
        [SerializeField]
        public T ConfigData;

        /// <summary>
        /// Load the configuration file.
        /// </summary>
        public override void Load()
        {
            Logger.LogInfo("Trying to read configuration file from " + FullPath + ".", LoggingName);

            if (!File.Exists(FullPath))
            {
                Logger.LogWarning("File " + FullPath + " not found, using default settings and generating file.",
                                  LoggingName);

                Save();
                return;
            }

            string data = File.ReadAllText(FullPath);

            if (Encrypted) data = data.FromBase64();

            ConfigData = JsonUtility.FromJson<T>(data);
            Logger.LogInfo("Loaded configuration " + FullPath + ".", LoggingName);
        }

        /// <summary>
        /// Saved the data edited in the scriptable to the configuration file.
        /// </summary>
        public override void Save()
        {
            Logger.LogInfo("Trying to save configuration file to " + FullPath + ".", LoggingName);

            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);

            string data = JsonUtility.ToJson(ConfigData, true);

            if (Encrypted) data = data.ToBase64();

            File.WriteAllText(FullPath, data);
            Logger.LogInfo("Saved configuration file to " + FullPath + ".", LoggingName);

            #if UNITY_WEBGL && !UNITY_EDITOR
            WebGlUtils.SyncFiles();
            #endif
        }
    }

    /// <summary>
    /// Abstract class for creating a configuration file that can be used by the application.
    /// </summary>
    public abstract class ConfigFile : ScriptableObject
    {
        /// <summary>
        /// Should this file be encrypted?
        /// </summary>
        public bool Encrypted;

        /// <summary>
        /// Name of the file.
        /// </summary>
        public string FileName = "Config";

        /// <summary>
        /// Name for logging messaged.
        /// </summary>
        protected string LoggingName => FileName + "ConfigFile";

        #if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>
        /// Path to the file.
        /// </summary>
        protected static string Path => Application.persistentDataPath + "/Configuration";
        #else
        /// <summary>
        /// Path to the file.
        /// </summary>
        protected static string Path => Directory.GetParent(Application.dataPath) + "/Configuration";
        #endif

        /// <summary>
        /// Full path to the file.
        /// </summary>
        protected string FullPath => Path + "/" + FileName + Extension;

        /// <summary>
        /// File extension.
        /// </summary>
        private string Extension => Encrypted ? ".bcfg" : ".cfg";

        /// <summary>
        /// Load the configuration file.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Saved the data edited in the scriptable to the configuration file.
        /// </summary>
        public abstract void Save();
    }

    /// <summary>
    /// Class where the configuration data is saved.
    /// </summary>
    [Serializable]
    public abstract class ConfigData
    {
    }
}