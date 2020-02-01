using System;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Configuration;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

namespace Varguiniano.Core.Runtime.Localization
{
    /// <summary>
    /// Localization class that handles string translation.
    /// </summary>
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        /// <summary>
        /// Reference to the language library.
        /// </summary>
        public LanguageLibrary LanguageLibrary;

        /// <summary>
        /// Current language.
        /// </summary>
        public uint CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                if (currentLanguage == value) return;

                if (value >= LanguageLibrary.Languages.Count)
                {
                    Logger.LogError(value
                                  + " is bigger or equal to the number of languages("
                                  + LanguageLibrary.Languages.Count
                                  + ").",
                                    this);

                    return;
                }

                currentLanguage = value;

                Logger.LogInfo("Setting language to "
                             + LanguageLibrary.LanguageNames[(int) currentLanguage].LocalizedValue
                             + ".",
                               this);

                if (Application.isPlaying)
                    ConfigurationManager.OnServiceReady += service =>
                                                           {
                                                               LocalizationConfig config =
                                                                   service.GetConfig<LocalizationConfig>();

                                                               if (config != null)
                                                               {
                                                                   if (config.Language == CurrentLanguage) return;
                                                                   config.Language = CurrentLanguage;
                                                                   service.SetConfig(config);
                                                               }
                                                               else
                                                                   Logger
                                                                      .LogWarning("There is no config file for localization. Language change will not persist after the game closes.",
                                                                                  this);
                                                           };

                LanguageChanged?.Invoke(currentLanguage);
            }
        }

        /// <summary>
        /// Backfield for CurrentLanguage.
        /// </summary>
        private uint currentLanguage;

        /// <summary>
        /// Get the name of the current language.
        /// </summary>
        public string CurrentLanguageName => LanguageLibrary.LanguageNames[(int) CurrentLanguage].LocalizedValue;

        /// <summary>
        /// Event raised when language changes.
        /// </summary>
        public Action<uint> LanguageChanged;

        /// <summary>
        /// Easy access to a language word.
        /// </summary>
        /// <param name="key">The key to look for on the current language.</param>
        public string this[string key] =>
            LanguageLibrary.Languages[(int) CurrentLanguage].WordDictionary.ContainsKey(key)
                ? LanguageLibrary.Languages[(int) CurrentLanguage].WordDictionary[key]
                : key;

        /// <summary>
        /// Try to get the language from the config file and set it.
        /// </summary>
        private void OnEnable() =>
            ConfigurationManager.OnServiceReady += service =>
                                                   {
                                                       LocalizationConfig config =
                                                           service.GetConfig<LocalizationConfig>();

                                                       if (config != null)
                                                           CurrentLanguage = config.Language;
                                                       else
                                                       {
                                                           Logger
                                                              .LogWarning("There is no config file for localization. Language will always be default on init.",
                                                                          this);
                                                       }
                                                   };
    }
}