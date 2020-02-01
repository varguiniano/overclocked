using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUI : MonoBehaviour
{
    public Image imgHDD;
    public Image imgGFX;
    public Image imgCPU;
    public Image imgPS;
    public Sprite HDDBrokenSprite;
    public Sprite GFXBrokenSprite;
    public Sprite CPUBrokenSprite;
    public Sprite PSBrokenSprite;
    public Sprite HDDEmptySprite;
    public Sprite GFXEmptySprite;
    public Sprite CPUEmptySprite;
    public Sprite PSEmptySprite;
    public PieceManager PieceManager;
    private List<PieceContainer> _pieceContainers;

    public void SetIcons(List<PieceContainer> pieceContainers)
    {
        _pieceContainers = pieceContainers;
    }

    private void Update()
    {
        for (int i = 0; i < _pieceContainers.Count; i++)
        {
            if (_pieceContainers[i].PieceType == PieceManager.CPU)
            {
                if (_pieceContainers[i].HasPiece)
                {
                    if (_pieceContainers[i].PieceBroken == false)
                    {
                        imgCPU.gameObject.SetActive(false);
                    }
                    else
                    {
                        imgCPU.sprite = CPUBrokenSprite;
                        imgCPU.gameObject.SetActive(true);
                    }
                }
                else
                {
                    imgCPU.sprite = CPUEmptySprite;
                    imgCPU.gameObject.SetActive(true);
                }
            }
            else if (_pieceContainers[i].PieceType == PieceManager.GTX)
            {
                if (_pieceContainers[i].HasPiece)
                {
                    if (_pieceContainers[i].PieceBroken == false)
                    {
                        imgGFX.gameObject.SetActive(false);
                    }
                    else
                    {
                        imgGFX.sprite = GFXBrokenSprite;
                        imgGFX.gameObject.SetActive(true);
                    }
                }
                else
                {
                    imgGFX.sprite = GFXEmptySprite;
                    imgGFX.gameObject.SetActive(true);
                }
            }
            else if (_pieceContainers[i].PieceType == PieceManager.HDD)
            {
                if (_pieceContainers[i].HasPiece)
                {
                    if (_pieceContainers[i].PieceBroken == false)
                    {
                        imgHDD.gameObject.SetActive(false);
                    }
                    else
                    {
                        imgHDD.sprite = HDDBrokenSprite;
                        imgHDD.gameObject.SetActive(true);
                    }
                }
                else
                {
                    imgHDD.sprite = HDDEmptySprite;
                    imgHDD.gameObject.SetActive(true);
                }
            }
            else if (_pieceContainers[i].PieceType == PieceManager.PS)
            {
                if (_pieceContainers[i].HasPiece)
                {
                    if (_pieceContainers[i].PieceBroken == false)
                    {
                        imgPS.gameObject.SetActive(false);
                    }
                    else
                    {
                        imgPS.sprite = PSBrokenSprite;
                        imgPS.gameObject.SetActive(true);
                    }
                }
                else
                {
                    imgPS.sprite = PSEmptySprite;
                    imgPS.gameObject.SetActive(true);
                }
            }
        }
    }
}