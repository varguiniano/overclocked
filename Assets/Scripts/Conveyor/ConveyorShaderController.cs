using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConveyorShaderController : MonoBehaviour
{
    public Material conveyorMaterial;
    public ConveyorSpeed Speed;
    public float SpeedMultiplier;

    public void Update()
    {
        conveyorMaterial.SetFloat("Speed",Speed.Value*SpeedMultiplier);
    }
}
