using UnityEditor;
using UnityEngine;

namespace Varguiniano.ExtendedEditor.Editor
{
    /// <summary>
    /// Extended property drawer.
    /// </summary>
    public class ExtendedDrawer : PropertyDrawer
    {
        /// <summary>
        /// Paints the property given.
        /// </summary>
        /// <param name="rect">Position.</param>
        /// <param name="property">Property to look into.</param>
        /// <param name="name">Name of that property.</param>
        /// <param name="includeChildren">Should it include children?</param>
        protected static void
            PaintProperty(Rect rect, SerializedProperty property, string name, bool includeChildren = false) =>
            EditorGUI.PropertyField(rect, property.FindPropertyRelative(name), includeChildren);
    }
}