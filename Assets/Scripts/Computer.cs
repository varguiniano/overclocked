using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.black;
    }
}
