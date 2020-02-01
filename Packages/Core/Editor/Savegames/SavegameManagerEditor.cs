using UnityEditor;
using UnityEngine;
using Varguiniano.Core.Runtime.Savegames;
using Varguiniano.ExtendedEditor.Editor;

namespace Varguiniano.Core.Editor.Savegames
{
    /// <summary>
    /// Custom editor for SavegameManager.
    /// </summary>
    [CustomEditor(typeof(SavegameManager))]
    public class SavegameManagerEditor : MonoBehaviourExtendedEditor<SavegameManager>
    {
        /// <summary>
        /// Paints the UI.
        /// </summary>
        protected override void PaintUi()
        {
            PaintProperty("Library", true);

            if (GUILayout.Button("Save")) TargetObject.SaveGame();

            EditorGUI.BeginDisabledGroup(!SavegameManager.SaveExists);
            if (GUILayout.Button("Load")) TargetObject.LoadGame();
            EditorGUI.EndDisabledGroup();
        }
    }
}