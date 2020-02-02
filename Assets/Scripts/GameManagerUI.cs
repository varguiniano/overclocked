using System;
using System.Collections;
using System.Collections.Generic;
using GlobalVariables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Varguiniano.Core.Runtime.Common;
using Random = UnityEngine.Random;

public class GameManagerUI : Singleton<GameManager>
{
    public GameManager GameManager;
    public Animator animator;
    public TextMeshProUGUI TimerText;
    private int _animatorEndgame = Animator.StringToHash("EndGame");


    public void Awake()
    {
        GameManager.OnGameEnd.AddListener(OnGameEnd);
    }

    public void Update()
    {
        TimerText.text = FloatToTime(GameManager.Timer, "#00:00");

    }

    private void OnGameEnd()
    {
        animator.SetBool(_animatorEndgame,true);
    }
    
    
    public string FloatToTime (float toConvert, string format){
         switch (format){
             case "00.0":
                 return string.Format("{0:00}:{1:0}", 
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*10) % 10));//miliseconds
             case "#0.0":
                 return string.Format("{0:#0}:{1:0}", 
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*10) % 10));//miliseconds
             
             case "00.00":
                 return string.Format("{0:00}:{1:00}", 
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*100) % 100));//miliseconds
             
             case "00.000":
                 return string.Format("{0:00}:{1:000}", 
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*1000) % 1000));//miliseconds
             
             case "#00.000":
                 return string.Format("{0:#00}:{1:000}", 
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*1000) % 1000));//miliseconds
             
             case "#0:00":
                 return string.Format("{0:#0}:{1:00}",
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60);//seconds
             
             case "#00:00":
                 return string.Format("{0:#00}:{1:00}", 
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60);//seconds
             
             case "0:00.0":
                 return string.Format("{0:0}:{1:00}.{2:0}",
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*10) % 10));//miliseconds
             
             case "#0:00.0":
                 return string.Format("{0:#0}:{1:00}.{2:0}",
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*10) % 10));//miliseconds
             
             case "0:00.00":
                 return string.Format("{0:0}:{1:00}.{2:00}",
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*100) % 100));//miliseconds
             
             case "#0:00.00":
                 return string.Format("{0:#0}:{1:00}.{2:00}",
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*100) % 100));//miliseconds
             
             case "0:00.000":
                 return string.Format("{0:0}:{1:00}.{2:000}",
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*1000) % 1000));//miliseconds
             
             case "#0:00.000":
                 return string.Format("{0:#0}:{1:00}.{2:000}",
                     Mathf.Floor(toConvert / 60),//minutes
                     Mathf.Floor(toConvert) % 60,//seconds
                     Mathf.Floor((toConvert*1000) % 1000));//miliseconds
             
         }
         return "error";
     }
}