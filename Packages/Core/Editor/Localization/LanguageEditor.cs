using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Varguiniano.Core.Runtime.Localization;
using Varguiniano.ExtendedEditor.Editor;

namespace Varguiniano.Core.Editor.Localization
{
    /// <summary>
    /// Custom editor for Language.
    /// </summary>
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : ScriptableExtendedEditor<Language>
    {
        /// <summary>
        /// Paints the UI.
        /// </summary>
        protected override void PaintUi()
        {
            PaintProperty("Key");

            if (string.IsNullOrEmpty(TargetObject.Key) || string.IsNullOrWhiteSpace(TargetObject.Key))
            {
                EditorGUILayout.HelpBox("Language key can't be empty.", MessageType.Error);
                return;
            }

            EditorGUILayout.HelpBox("Opening the json will override changes on it, load from json first.",
                                    MessageType.Info);

            if (GUILayout.Button("Open JSON"))
            {
                TargetObject.SaveToFile();
                Process.Start(TargetObject.JsonPath);
            }

            EditorGUILayout.HelpBox("Remember to load from json each time you modify the file.",
                                    MessageType.Info);

            EditorGUI.BeginDisabledGroup(!File.Exists(TargetObject.JsonPath));

            {
                if (GUILayout.Button("Load from JSON")) TargetObject.LoadFromFile();
            }

            EditorGUI.EndDisabledGroup();

            List<string> foundList = new List<string>();

            for (int i = 0; i < TargetObject.Words.Words.Count; ++i)
            {
                if (foundList.Contains(TargetObject.Words.Words[i].Key))
                {
                    EditorGUILayout.HelpBox("List of words contains two instances of the key "
                                          + TargetObject.Words.Words[i].Key
                                          + "! This will break stuff.",
                                            MessageType.Error);

                    break;
                }

                foundList.Add(TargetObject.Words.Words[i].Key);
            }

            PaintProperty("Words", true);
        }
    }
}