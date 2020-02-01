using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Varguiniano.Core.Runtime.Common;

public class GameManager : Singleton<GameManager>
{
    [Range(0, 10)]
    [SerializeField]
    private int _stars = 6;
    
    public int Stars
    {
        get { return _stars; }
        set
        {
            _stars = Mathf.Clamp(value, 0, 10);
            if (_stars == 0)
            {
                EndGame();
            }
        }
    }

    public UnityEvent OnGameEnd;

    public List<Player> Players = new List<Player>();
    public ConveyorController ConveyorController;

    public void Awake()
    {
        ConveyorController.OnComputerGoal.AddListener(ComputerReachedGoal);
    }

    public void ComputerReachedGoal(Computer computer)
    {
        bool repaired = true;
        foreach (var container in computer.PieceContainers)
        {
            if (container.PieceBroken)
            {
                Stars -= 1;
                repaired = false;
            }
        }

        if (repaired)
        {
            Stars += 2;
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        foreach (var player in Players)
        {
            player.gameObject.SetActive(false);
        }
        OnGameEnd.Invoke();
    }
}
