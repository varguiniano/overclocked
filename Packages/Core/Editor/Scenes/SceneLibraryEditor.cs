using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Scenes;
using Varguiniano.ExtendedEditor.Editor;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

namespace Varguiniano.Core.Editor.Scenes
{
    /// <summary>
    /// Custom editor for SceneLibrary.
    /// </summary>
    [CustomEditor(typeof(SceneLibraryAsset))]
    public class SceneLibraryEditor : ScriptableExtendedEditor<SceneLibraryAsset>
    {
        /// <summary>
        /// Dictionary that saves the scene assets and if they should be active.
        /// </summary>
        private readonly List<SceneAssetBoolPair> sceneDictionary = new List<SceneAssetBoolPair>();

        /// <summary>
        /// Reorderable list to display the scene list.
        /// </summary>
        private ReorderableList reorderableList;

        /// <summary>
        /// Called when this object is enabled.
        /// </summary>
        private void OnEnable()
        {
            LoadDictionary();
            LoadListEditor();
        }

        /// <summary>
        /// Const for two thirds.
        /// </summary>
        private const float TwoThirds = 2 / 3f;

        /// <summary>
        /// Paints the UI.
        /// </summary>
        protected override void PaintUi()
        {
            if (GUILayout.Button("Save library and build settings")) SaveDictionary();
            reorderableList.DoLayoutList();
        }

        /// <summary>
        /// Loads the scene dictionary.
        /// </summary>
        private void LoadDictionary()
        {
            sceneDictionary.Clear();
            List<StringBoolPair> scenesToRemove = new List<StringBoolPair>();

            for (int i = 0; i < TargetObject.SceneDictionary.Count; ++i)
            {
                SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(TargetObject.SceneDictionary[i].Key);

                if (scene == null)
                {
                    Logger.LogError("Library contained a path to a scene that doesn't exist, deleting it.", this);
                    scenesToRemove.Add(TargetObject.SceneDictionary[i]);
                    continue;
                }

                sceneDictionary.Add(new SceneAssetBoolPair(scene, TargetObject.SceneDictionary[i].Value));
            }

            for (int i = 0; i < scenesToRemove.Count; ++i) TargetObject.SceneDictionary.Remove(scenesToRemove[i]);
        }

        /// <summary>
        /// Saves the scene dictionary.
        /// </summary>
        private void SaveDictionary()
        {
            List<StringBoolPair> scenesToSave = new List<StringBoolPair>();
            List<EditorBuildSettingsScene> scenesToBuild = new List<EditorBuildSettingsScene>();

            for (int i = 0; i < sceneDictionary.Count; ++i)
            {
                if (sceneDictionary[i].Key == null) continue;
                string path = AssetDatabase.GetAssetPath(sceneDictionary[i].Key);

                scenesToSave.Add(new StringBoolPair(path,
                                                    sceneDictionary[i].Value));

                scenesToBuild.Add(new EditorBuildSettingsScene(path,
                                                               sceneDictionary[i].Value));
            }

            TargetObject.SceneDictionary = scenesToSave;
            EditorBuildSettings.scenes = scenesToBuild.ToArray();
        }

        /// <summary>
        /// Loads the editor for the Exercise List.
        /// </summary>
        private void LoadListEditor() =>
            reorderableList =
                new ReorderableList(sceneDictionary, typeof(SceneAssetBoolPair))
                {
                    drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Scene list"),
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                                          {
                                              rect.width *= TwoThirds;

                                              sceneDictionary[index].Key = (SceneAsset) EditorGUI.ObjectField(rect,
                                                                                                              sceneDictionary
                                                                                                                      [index]
                                                                                                                 .Key,
                                                                                                              typeof(
                                                                                                                  SceneAsset
                                                                                                              ),
                                                                                                              false);

                                              rect.x += rect.width;
                                              rect.width = 20;

                                              sceneDictionary[index].Value =
                                                  EditorGUI.Toggle(rect, sceneDictionary[index].Value);
                                          }
                };
    }

    /// <summary>
    /// String bool pair for serializable "dictionaries".
    /// </summary>
    [Serializable]
    public class SceneAssetBoolPair
    {
        /// <summary>
        /// Scene key.
        /// </summary>
        public SceneAsset Key;

        /// <summary>
        /// Bool value.
        /// </summary>
        public bool Value;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SceneAssetBoolPair()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SceneAssetBoolPair(SceneAsset key, bool value)
        {
            Key = key;
            Value = value;
        }
    }
}