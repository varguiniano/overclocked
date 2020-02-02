using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersSelected",menuName = "GlobalVariables/PlayersSelected")]
public class PlayersSelected: ScriptableObject
{
    public enum PlayerSelectOptions
    {
        player1,
        player2,
        bothPlayers
    }

    public PlayerSelectOptions playerSelection;
}
