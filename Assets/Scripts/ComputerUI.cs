using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUI : MonoBehaviour
{
    public Image[] imgsIcons;
    public Sprite HDDSprite;
    public Sprite GFXSprite;
    public Sprite CPUSprite;
    public Sprite PSSprite;
    public PieceManager PieceManager;

    public void SetIcons(List<PieceContainer> pieceContainers) {
        for (int i = 0; i < pieceContainers.Count; i++) {
            if (pieceContainers[i].PieceType == PieceManager.CPU) {
                imgsIcons[i].sprite = CPUSprite;
                imgsIcons[i].gameObject.SetActive(true);
            }
            else if (pieceContainers[i].PieceType == PieceManager.GTX)
            {
                imgsIcons[i].sprite = GFXSprite;
                imgsIcons[i].gameObject.SetActive(true);
            }
            else if (pieceContainers[i].PieceType == PieceManager.HDD)
            {
                imgsIcons[i].sprite = HDDSprite;
                imgsIcons[i].gameObject.SetActive(true);
            }
            else if (pieceContainers[i].PieceType == PieceManager.PS)
            {
                imgsIcons[i].sprite = PSSprite;
                imgsIcons[i].gameObject.SetActive(true);
            }

        }

    }

}
