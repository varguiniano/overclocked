using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public PieceManager PieceManager;
    public List<PieceContainer> PieceContainers = new List<PieceContainer>();
    public ComputerUI ui;
    public ComputerSizeType Size;

    public Transform Placeholder;

    [Header("GlobalVariables")]
    public ConveyorSpeed ConveyorSpeed;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.black;
    }

    public void Update()
    {
        transform.Translate(transform.right * (ConveyorSpeed.Value * Time.deltaTime));
    }

    public bool CanReceivePiece(PieceType pieceType, bool broken)
    {
        for (int i = 0; i < PieceContainers.Count; i++)
        {
            if (!PieceContainers[i].HasPiece && PieceContainers[i].AllowedPieceTypes.Contains(pieceType) && !broken)
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CanGivePiece(PieceType pieceType, bool broken)
    {
        for (int i = 0; i < PieceContainers.Count; i++)
        {
            if (PieceContainers[i].HasPiece && PieceContainers[i].PieceType == pieceType && broken)
            {
                return true;
            }
        }
        return false;
    }

    public void AddPiece(Piece piece)
    {
        for (int i = 0; i < PieceContainers.Count; i++)
        {
            if (!PieceContainers[i].HasPiece && PieceContainers[i].AllowedPieceTypes.Contains(piece.pieceType))
            {
                PieceContainers[i].AddPiece(piece);
            }
        }
    }

    public Piece TakePiece(PieceType pieceType)
    {
        for (int i = 0; i < PieceContainers.Count; i++)
        {
            if (PieceContainers[i].HasPiece && PieceContainers[i].PieceType == pieceType)
            {
                return PieceContainers[i].TakePiece();
            }
        }
        // ESTO ES MUY MALLLOOOOO NO HEMOS COMPROBADO SI HAY PIEZA PRIMEROOOOO MALOOOOO
     
        return null;
    }

    public bool IsPieceBroken(PieceType pieceType)
    {
        for (int i = 0; i < PieceContainers.Count; i++)
        {
            if (PieceContainers[i].HasPiece && PieceContainers[i].PieceType == pieceType)
            {
                return PieceContainers[i].PieceBroken;
            }
        }

        return false;
    }
}
