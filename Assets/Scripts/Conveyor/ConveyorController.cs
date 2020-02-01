using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConveyorController : MonoBehaviour
{
    public GameObject Spawner;
    public ConveyorGoalTrigger Goal;
    public PieceManager PieceManager;
    public ComputerBuilder ComputerBuilder;
    public ComputerManager ComputerManager;
    

    public GoalEvent OnComputerGoal;

    public void Awake()
    {
        Goal.OnComputerGoal.AddListener(ComputerGoalReached);
    }
    
    public void SpawnComputer(ComputerDescriptor computerDescriptor)
    {
        Computer computer =  ComputerBuilder.BuildComputer(computerDescriptor, ComputerManager.GetRandomSize());
        computer.transform.position = Spawner.transform.position;
    }

    private void ComputerGoalReached(Computer computer)
    {
        Debug.Log(computer.PieceContainers.Count + " : " + computer.name);
        OnComputerGoal.Invoke(computer);
        Destroy(computer.gameObject);
    }
    
}
