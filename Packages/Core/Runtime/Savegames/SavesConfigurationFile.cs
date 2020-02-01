using System;
using UnityEngine;
using Varguiniano.Core.Runtime.Configuration;

namespace Varguiniano.Core.Runtime.Savegames
{
    /// <summary>
    /// Class representing the saves configuration file.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Savegames/Configuration", fileName = "SavesConfig")]
    public class SavesConfigurationFile : ConfigFile<SavesConfiguration>
    {
    }

    /// <summary>
    /// Class representing saves configuration.
    /// </summary>
    [Serializable]
    public class SavesConfiguration : ConfigData
    {
        /// <summary>
        /// Number of saves to keep before cleaning them up.
        /// 0 is infinite.
        /// </summary>
        public ushort SavesToKeep;
    }
}