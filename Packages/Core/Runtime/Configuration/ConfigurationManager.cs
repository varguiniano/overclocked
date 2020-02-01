using System;
using System.Collections.Generic;
using Varguiniano.Core.Runtime.Common;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

namespace Varguiniano.Core.Runtime.Configuration
{
    /// <summary>
    /// Manager that grants access to the configuration files.
    /// </summary>
    public class ConfigurationManager : Service<ConfigurationManager>
    {
        /// <summary>
        /// List with all the available config files.
        /// </summary>
        public List<ConfigFile> ConfigFiles;

        /// <summary>
        /// Action raised when a configuration file is updated.
        /// </summary>
        public Action<ConfigData> ConfigurationUpdated;

        /// <summary>
        /// Load the config files.
        /// </summary>
        private void OnEnable()
        {
            Logger.LogInfo("Initializing configuration manager...", this);

            List<Type> checkedTypes = new List<Type>();

            for (int i = 0; i < ConfigFiles.Count; ++i)
            {
                if (checkedTypes.Contains(ConfigFiles[i].GetType()))
                {
                    Logger.LogError("You have two configuration files of the same type added to the configuration list. "
                                  + "Only one file of each type should be created. Manager won't initialize.",
                                    this);

                    return;
                }

                checkedTypes.Add(ConfigFiles[i].GetType());
                ConfigFiles[i].Load();
            }

            Ready();
        }

        /// <summary>
        /// Gets the configuration file with the given type.
        /// </summary>
        /// <typeparam name="T">The type of the configuration file we are looking for.</typeparam>
        /// <returns>The matching configuration file.</returns>
        [Obsolete("GetConfigFile is obsolete as there is no need to get the file directly, use GetConfig or SetConfig instead.")]
        public T GetConfigFile<T>() where T : ConfigFile
        {
            if (!IsReady)
            {
                Logger.LogError("Configuration manager has not initialized, will return null.", this);
                return null;
            }

            for (int i = 0; i < ConfigFiles.Count; ++i)
                if (ConfigFiles[i] is T)
                    return (T) ConfigFiles[i];

            Logger.LogError("Config file not found! Returning null.", this);
            return null;
        }

        /// <summary>
        /// Gets the configuration file with the given type.
        /// </summary>
        /// <typeparam name="T">The type of the configuration file we are looking for.</typeparam>
        /// <returns>The matching configuration file.</returns>
        public T GetConfig<T>() where T : ConfigData
        {
            if (!IsReady)
            {
                Logger.LogError("Configuration manager has not initialized, will return null.", this);
                return null;
            }

            for (int i = 0; i < ConfigFiles.Count; ++i)
                if (ConfigFiles[i] is ConfigFile<T>)
                    return ((ConfigFile<T>) ConfigFiles[i]).ConfigData;

            Logger.LogError("Config file not found! Returning null.", this);
            return null;
        }

        /// <summary>
        /// Sets a new config for the given type.
        /// </summary>
        /// <param name="newConfig">New config to save.</param>
        /// <typeparam name="T">The type of config to save.</typeparam>
        /// <returns>True if it saved successfully.</returns>
        public bool SetConfig<T>(T newConfig) where T : ConfigData
        {
            if (!IsReady)
            {
                Logger.LogError("Configuration manager has not initialized, will not update config.", this);
                return false;
            }
            
            for (int i = 0; i < ConfigFiles.Count; ++i)
                if (ConfigFiles[i] is ConfigFile<T>)
                {
                    ConfigFile<T> file = (ConfigFile<T>) ConfigFiles[i];
                    file.ConfigData = newConfig;
                    file.Save();
                    
                    ConfigurationUpdated?.Invoke(newConfig);
                    
                    return true;
                }

            Logger.LogError("No file found for that config type, will not update config.", this);
            return false;
        }
    }
}