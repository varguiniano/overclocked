using System.Collections.Generic;
using UnityEditor;
using Varguiniano.Core.Runtime.Localization;
using Varguiniano.ExtendedEditor.Editor;

namespace Varguiniano.Core.Editor.Localization
{
    /// <summary>
    /// Custom editor for the localization manager.
    /// </summary>
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : MonoBehaviourExtendedEditor<LocalizationManager>
    {
        /// <summary>
        /// Paint the UI.
        /// </summary>
        protected override void PaintUi()
        {
            if (TargetObject.LanguageLibrary != null)
            {
                List<LocalizableString> languageList = TargetObject.LanguageLibrary.LanguageNames;
                string[] languageNames = new string[languageList.Count];

                for (int i = 0; i < languageList.Count; ++i) languageNames[i] = languageList[i].LocalizedValue;

                TargetObject.CurrentLanguage =
                    (uint) EditorGUILayout.Popup("Current language", (int) TargetObject.CurrentLanguage, languageNames);
            }

            PaintProperty("LanguageLibrary");
        }
    }
}