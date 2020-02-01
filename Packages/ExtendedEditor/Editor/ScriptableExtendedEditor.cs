using UnityEditor;
using UnityEngine;

namespace Varguiniano.ExtendedEditor.Editor
{
    /// <inheritdoc />
    /// <summary>
    /// Class with some utilities for editors.
    /// </summary>
    public abstract class ScriptableExtendedEditor<T> : UnityEditor.Editor where T : ScriptableObject
    {
        /// <summary>
        /// Reference to the object being edited.
        /// </summary>
        protected T TargetObject => (T) target;

        /// <inheritdoc />
        /// <summary>
        /// Paint the UI and apply the properties.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            PaintUi();
            if (!EditorGUI.EndChangeCheck()) return;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(TargetObject);
            EditorApplication.update.Invoke();
        }

        /// <summary>
        /// Paint the UI.
        /// </summary>
        protected abstract void PaintUi();

        /// <summary>
        /// Paints the property given.
        /// </summary>
        /// <param name="propertyName">Name of that property.</param>
        /// <param name="includeChildren">Should it include children?</param>
        protected void PaintProperty(string propertyName, bool includeChildren = false) =>
            PaintProperty(serializedObject.FindProperty(propertyName), includeChildren);

        /// <summary>
        /// Paints the property given.
        /// </summary>
        /// <param name="serializedProperty">Property to paint.</param>
        /// <param name="includeChildren">Should it include children?</param>
        protected void PaintProperty(SerializedProperty serializedProperty, bool includeChildren = false) =>
            EditorGUILayout.PropertyField(serializedProperty, includeChildren);

        /// <summary>
        /// Paints the property given.
        /// </summary>
        /// <param name="rect">Rect to paint to.</param>
        /// <param name="serializedProperty">Property to paint.</param>
        /// <param name="includeChildren">Should it include children?</param>
        protected void PaintProperty(Rect rect, SerializedProperty serializedProperty, bool includeChildren = false) =>
            EditorGUI.PropertyField(rect, serializedProperty, includeChildren);
        
        /// <summary>
        /// Paints the property given.
        /// </summary>
        /// <param name="serializedProperty">Property to look into.</param>
        /// <param name="propertyName">Name of that property.</param>
        /// <param name="includeChildren">Should it include children?</param>
        protected void PaintPropertyFromProperty(SerializedProperty serializedProperty, string propertyName, bool includeChildren = false) =>
            EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative(propertyName), includeChildren);

        /// <summary>
        /// Paints the property given.
        /// </summary>
        /// <param name="rect">Rect to paint to.</param>
        /// <param name="serializedProperty">Property to look into.</param>
        /// <param name="propertyName">Name of that property.</param>
        /// <param name="includeChildren">Should it include children?</param>
        protected void PaintPropertyFromProperty(Rect rect, SerializedProperty serializedProperty, string propertyName, bool includeChildren = false) =>
            EditorGUI.PropertyField(rect, serializedProperty.FindPropertyRelative(propertyName), includeChildren);
    }
}