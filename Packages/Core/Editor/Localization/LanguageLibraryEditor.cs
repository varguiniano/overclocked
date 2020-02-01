using System.Collections.Generic;
using UnityEditor;
using Varguiniano.Core.Runtime.Localization;
using Varguiniano.ExtendedEditor.Editor;

namespace Varguiniano.Core.Editor.Localization
{
    /// <summary>
    /// Custom editor for LanguageLibrary.
    /// </summary>
    [CustomEditor(typeof(LanguageLibrary))]
    public class LanguageLibraryEditor : ScriptableExtendedEditor<LanguageLibrary>
    {
        /// <summary>
        /// Paints the UI.
        /// </summary>
        protected override void PaintUi()
        {
            List<string> foundList = new List<string>();

            for (int i = 0; i < TargetObject.Languages.Count; ++i)
            {
                if (foundList.Contains(TargetObject.Languages[i].Key))
                {
                    EditorGUILayout.HelpBox("There are two languages the key "
                                          + TargetObject.Languages[i].Key
                                          + "! This will break stuff.",
                                            MessageType.Error);

                    break;
                }

                foundList.Add(TargetObject.Languages[i].Key);
            }

            PaintProperty("Languages", true);
        }
    }
}