using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Varguiniano.Core.Runtime.Common;
using Random = UnityEngine.Random;

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
    public SpawnSpeed SpawnSpeed;

    private Coroutine SpawnRoutine;

    public void Awake()
    {
        ConveyorController.OnComputerGoal.AddListener(ComputerReachedGoal);
    }

    private void OnEnable()
    {
        StartGame();
    }

    // TODO: Call this somewhere.
    public void StartGame()
    {
        SpawnRoutine = StartCoroutine(ComputerSpawnRoutine());
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
        StopCoroutine(SpawnRoutine);
        OnGameEnd.Invoke();
    }

    private IEnumerator ComputerSpawnRoutine()
    {
        while (true)
        {
            ConveyorController.SpawnComputer(new ComputerDescriptor(Random.Range(0,3),Random.Range(0,3),Random.Range(0,3),Random.Range(0,3)));
            yield return new WaitForSecondsRealtime(SpawnSpeed.Value);
        }
    }
}
