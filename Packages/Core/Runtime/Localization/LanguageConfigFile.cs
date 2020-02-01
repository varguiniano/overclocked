using System;
using UnityEngine;
using Varguiniano.Core.Runtime.Configuration;

namespace Varguiniano.Core.Runtime.Localization
{
    /// <summary>
    /// Class representing a language config file.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Localization/Config", fileName = "LocalizationConfig")]
    public class LanguageConfigFile : ConfigFile<LocalizationConfig>
    {
    }

    /// <summary>
    /// Class representing a localization configuration.
    /// </summary>
    [Serializable]
    public class LocalizationConfig : ConfigData
    {
        /// <summary>
        /// Language to use.
        /// </summary>
        public uint Language;
    }
}