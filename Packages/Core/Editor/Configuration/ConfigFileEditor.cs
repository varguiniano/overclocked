using UnityEditor;
using UnityEngine;
using Varguiniano.Core.Runtime.Configuration;
using Varguiniano.ExtendedEditor.Editor;

namespace Varguiniano.Core.Editor.Configuration
{
    /// <summary>
    /// Custom editor for config file.
    /// </summary>
    [CustomEditor(typeof(ConfigFile), true)]
    public class ConfigFileEditor : ScriptableExtendedEditor<ConfigFile>
    {
        /// <summary>
        /// Paint the UI.
        /// </summary>
        protected override void PaintUi()
        {
            PaintProperty("FileName");
            
            PaintProperty("Encrypted");
            
            PaintProperty("ConfigData", true);

            if (GUILayout.Button("Load")) TargetObject.Load();

            if (GUILayout.Button("Save")) TargetObject.Save();
        }
    }
}