using System;
using System.Collections;
using System.Collections.Generic;
using GlobalVariables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Varguiniano.Core.Runtime.Common;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [Range(0, 10)] [SerializeField] private int _stars = 10;

    public float ElapsedSeconds;

    public int Stars
    {
        get { return _stars; }
        set
        {
            _stars = Mathf.Clamp(value, 0, 10);
            if (_stars == 0 && !GameBalance.GodMode)
            {
                EndGame();
            }
        }
    }

    public GameBalance GameBalance;
    public ConveyorSpeed ConveyorSpeed;
    public PlayersSelected PlayersSelected;
    
    public UnityEvent OnGameEnd;

    public List<Player> Players = new List<Player>();
    public ConveyorController ConveyorController;

    public bool GameEnded = false;
    public float Timer = 0;

    

    private Coroutine SpawnRoutine;

    public void Awake()
    {
        ConveyorController.OnComputerGoal.AddListener(ComputerReachedGoal);
    }

    private void OnEnable()
    {
        StartGame();
    }
    
    private IEnumerator TimerRoutine()
    {
        while (!GameEnded)
        {
            yield return new WaitForSeconds(1f);
            Timer++;
        }
    }

    // TODO: Call this somewhere.
    public void StartGame()
    {
        if (PlayersSelected.playerSelection == PlayersSelected.PlayerSelectOptions.bothPlayers)
        {
            Players[0].gameObject.SetActive(true);
            Players[1].gameObject.SetActive(true);
        }
        else if(PlayersSelected.playerSelection == PlayersSelected.PlayerSelectOptions.player1)
        {
            Players[0].gameObject.SetActive(true);
        } 
        else if(PlayersSelected.playerSelection == PlayersSelected.PlayerSelectOptions.player2)
        {
            Players[1].gameObject.SetActive(true);
        }


        SpawnRoutine = StartCoroutine(ComputerSpawnRoutine());
        StartCoroutine(TimerRoutine());
        ElapsedSeconds = 0;
    }

    public void Replay()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
        GameEnded = false;
    }

    private void Update()
    {
        ElapsedSeconds += Time.deltaTime;
        ConveyorSpeed.Value = GameBalance.ConveyorSpeedRate.Evaluate(ElapsedSeconds);
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
        GameEnded = true;
        OnGameEnd.Invoke();
    }

    private IEnumerator ComputerSpawnRoutine()
    {
        while (true)
        {
            ConveyorController.SpawnComputer(new ComputerDescriptor(
                Random.Range(0, (int) GameBalance.BrokenPiecesRate.Evaluate(ElapsedSeconds)),
                Random.Range(0, (int) GameBalance.BrokenPiecesRate.Evaluate(ElapsedSeconds)),
                Random.Range(0, (int) GameBalance.BrokenPiecesRate.Evaluate(ElapsedSeconds)),
                Random.Range(0, (int) GameBalance.BrokenPiecesRate.Evaluate(ElapsedSeconds))));
            yield return new WaitForSecondsRealtime(GameBalance.SpawnSpeedRate.Evaluate(ElapsedSeconds));
        }
    }
    
    
}