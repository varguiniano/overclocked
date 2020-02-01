using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComputerManager",menuName = "ScriptableObjects/Computer/ComputerManager")]
public class ComputerManager : ScriptableObject
{
    public ComputerSizeType Small;
    public ComputerSizeType Medium;
    public ComputerSizeType Big;
    
    public List<Computer> Prefabs = new List<Computer>();

    public Computer InstantiatePrefab(ComputerSizeType size)
    {
        for (int i = 0; i < Prefabs.Count; i++)
        {
            if (Prefabs[i].Size == size)
            {
                return Instantiate(Prefabs[i]);
            }
        }

        return null;
    }

    public Computer InstantiateRandomSizePrefab()
    {
        if (Prefabs.Count > 0)
        {
            return Instantiate(Prefabs[Random.Range(0, Prefabs.Count)]);
        }

        return null;
    }

    public ComputerSizeType GetRandomSize()
    {
        switch (Random.Range(0,3))
        {
            case 0:
            {
                return Small;
            }
            case 1:
                return Medium;
            case 2:
                return Big;
        }

        return Medium;
    }
}
