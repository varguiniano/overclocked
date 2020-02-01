using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GoalEvent : UnityEvent<Computer>
{
    
}
public class ConveyorGoalTrigger : MonoBehaviour
{
    [SerializeField]
    public GoalEvent OnComputerGoal;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Computer"))
        {
            OnComputerGoal.Invoke(other.GetComponent<Computer>());
        }
    }
}
