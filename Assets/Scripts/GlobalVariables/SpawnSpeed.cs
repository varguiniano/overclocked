using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSpeed",menuName = "GlobalVariables/SpawnSpeed")]
public class SpawnSpeed : ScriptableObject
{
    [Tooltip("Seconds between computers.")]
    public float Value = 1;
}
