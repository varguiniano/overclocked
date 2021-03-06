﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceManager", menuName = "ScriptableObjects/Piece/PieceManager")]
public class PieceManager : ScriptableObject
{
    public PieceType CPU;
    public PieceType GTX;
    public PieceType PS;
    public PieceType HDD;

    public Piece CPUPrefab;
    public Piece GTXPrefab;
    public Piece PSPrefab;
    public Piece HDDPrefab;


    public Piece InstantiatePiece(PieceType pieceType)
    {
        if (pieceType == CPU)
        {
            return Instantiate(CPUPrefab);
        }
        if (pieceType == GTX)
        {
            return Instantiate(GTXPrefab);
        }
        if (pieceType == PS)
        {
            return Instantiate(PSPrefab);
        }
        if (pieceType == HDD)
        {
            return Instantiate(HDDPrefab);
        }

        return null;
    }
}
