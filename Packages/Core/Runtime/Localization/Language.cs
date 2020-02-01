using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Varguiniano.Core.Runtime.Localization
{
    /// <summary>
    /// Scriptable object that represents a language.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Localization/Language", fileName = "Language")]
    public class Language : ScriptableObject
    {
        /// <summary>
        /// Language key.
        /// </summary>
        public string Key;

        /// <summary>
        /// Language words.
        /// </summary>
        public LanguageWords Words = new LanguageWords();

        /// <summary>
        /// Easily accessible word dictionary.
        /// </summary>
        public Dictionary<string, string> WordDictionary
        {
            get
            {
                if (wordDictionary != null) return wordDictionary;

                wordDictionary = new Dictionary<string, string>();

                for (int i = 0; i < Words.Words.Count; ++i) wordDictionary[Words.Words[i].Key] = Words.Words[i].Value;

                return wordDictionary;
            }
        }

        /// <summary>
        /// Backfield for WordDictionary.
        /// </summary>
        private Dictionary<string, string> wordDictionary;

        #if UNITY_EDITOR
        /// <summary>
        /// Path to the json this asset should save.
        /// </summary>
        public string JsonPath => Path.ChangeExtension(Path.GetFullPath(AssetDatabase.GetAssetPath(this)), ".json");
        #endif

        /// <summary>
        /// Saves the scriptable to the json file.
        /// </summary>
        public void SaveToFile()
        {
            #if UNITY_EDITOR
            string json = JsonUtility.ToJson(Words, true);

            File.WriteAllText(JsonPath, json);
            #endif
        }

        /// <summary>
        /// Loads the scriptable from the json file.
        /// </summary>
        public void LoadFromFile()
        {
            #if UNITY_EDITOR
            string json = File.ReadAllText(JsonPath);

            Words = JsonUtility.FromJson<LanguageWords>(json);
            #endif
        }
    }

    /// <summary>
    /// Serializable class to save the words of a language.
    /// </summary>
    [Serializable]
    public class LanguageWords
    {
        /// <summary>
        /// List of all "words" in the language.
        /// </summary>
        public List<StringStringPair> Words = new List<StringStringPair>();
    }
}