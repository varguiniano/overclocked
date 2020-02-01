using UnityEditor;
using UnityEngine;
using Varguiniano.Core.Runtime.Localization;
using Varguiniano.ExtendedEditor.Editor;

namespace Varguiniano.Core.Editor.Localization
{
    /// <summary>
    /// Custom drawer for the localizable string.
    /// </summary>
    [CustomPropertyDrawer(typeof(LocalizableString))]
    public class LocalizableStringDrawer : ExtendedDrawer
    {
        /// <summary>
        /// Just show the key as a field.
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) =>
            EditorGUI.PropertyField(position, property.FindPropertyRelative("Key"), label);
    }
}