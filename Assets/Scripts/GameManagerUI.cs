using System;
using System.Collections;
using System.Collections.Generic;
using GlobalVariables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Varguiniano.Core.Runtime.Common;
using Random = UnityEngine.Random;

public class GameManagerUI : Singleton<GameManager>
{
    public GameManager GameManager;
    public Animator animator;
    private int _animatorEndgame = Animator.StringToHash("EndGame");

    public CanvasGroup EndGamePanel;

    public void Awake()
    {
        GameManager.OnGameEnd.AddListener(OnGameEnd);
    }

    private void OnGameEnd()
    {
        animator.SetBool(_animatorEndgame,true);
    }
}