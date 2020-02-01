using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;

namespace Varguiniano.Core.Runtime.Scenes
{
    /// <summary>
    /// Library with the scenes to save to the build settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Scenes/SceneLibrary", fileName = "SceneLibrary")]
    public class SceneLibraryAsset : ScriptableObject
    {
        /// <summary>
        /// Dictionary with the scenes to save to the build settings.
        /// </summary>
        public List<StringBoolPair> SceneDictionary = new List<StringBoolPair>();

        /// <summary>
        /// Array of the active scenes in build.
        /// </summary>
        public string[] Scenes => ScenesList.ToArray();

        /// <summary>
        /// List of the active scenes in build.
        /// </summary>
        public List<string> ScenesList
        {
            get
            {
                List<string> list = new List<string>();

                for (int i = 0; i < SceneDictionary.Count; ++i)
                    if (SceneDictionary[i].Value)
                        list.Add(Path.GetFileNameWithoutExtension(SceneDictionary[i].Key));

                return list;
            }
        }
    }
}