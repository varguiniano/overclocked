using UnityEditor;
using UnityEngine;

namespace Varguiniano.ExtendedEditor.Editor
{
    /// <inheritdoc />
    /// <summary>
    /// Class with some utilities for editors.
    /// </summary>
    public abstract class MonoBehaviourExtendedEditor<T> : UnityEditor.Editor where T : MonoBehaviour
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), includeChildren);
    }
}