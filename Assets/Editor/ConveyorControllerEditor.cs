using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConveyorController))]
public class ConveyorControllerEditor : Editor
{
    ConveyorController _this => target as ConveyorController;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("SpawnComputer"))
        {
            _this.SpawnComputer(new ComputerDescriptor(Random.Range(0,3),Random.Range(0,3),Random.Range(0,3),Random.Range(0,3)));
        }
    }
}
