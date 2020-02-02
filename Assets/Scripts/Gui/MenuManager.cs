using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Renderer player1;
    public Renderer player2;
    private bool player1Active;
    private bool player2Active;
    public PlayersSelected PlayersSelected;

    public Material originalMat;
    public Material greyMat;
    // Start is called before the first frame update
    void Start()
    {
        player1Active = true;
        player2Active = true;
        PlayersSelected.playerSelection = PlayersSelected.PlayerSelectOptions.bothPlayers;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickPlayer1()
    {
        if (player1Active && player2Active)
        {
            player1Active = false;
            player1.material = greyMat;
            PlayersSelected.playerSelection = PlayersSelected.PlayerSelectOptions.player2;
        }
        else
        {
            if (player1Active)
            {
                player1Active = false;
                player1.material = greyMat; 
                player2Active = true;
                player2.material = originalMat;
                PlayersSelected.playerSelection = PlayersSelected.PlayerSelectOptions.player2; 
               
            }
            else
            {
                player1Active = true;
                player1.material = originalMat;
                PlayersSelected.playerSelection = PlayersSelected.PlayerSelectOptions.bothPlayers;
            }
        }
    }
    public void ClickPlayer2()
    {
        if (player1Active && player2Active)
        {
            player2Active = false;
            player2.material = greyMat;
            PlayersSelected.playerSelection = PlayersSelected.PlayerSelectOptions.player1;
        }
        else
        {
            if (player2Active)
            {
                player2Active = false;
                player2.material = greyMat; 
                player1Active = true;
                player1.material = originalMat;
                PlayersSelected.playerSelection = PlayersSelected.PlayerSelectOptions.player1;
               
            }
            else
            {
                player2Active = true;
                player2.material = originalMat;
                PlayersSelected.playerSelection =PlayersSelected.PlayerSelectOptions.bothPlayers;
            }
        }
    }
}
