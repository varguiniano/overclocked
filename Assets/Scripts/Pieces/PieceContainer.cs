﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceContainer : MonoBehaviour
{
    public List<PieceType> AllowedPieceTypes = new List<PieceType>();

    public Transform piecePlaceholder;

    public bool usePlaceholder;
    public bool disablePiece;

    public bool PieceBroken => _piece == null || _piece.Broken;

    [SerializeField] private Piece _piece;

    public bool HasPiece => _piece != null;
    public PieceType PieceType => _piece.pieceType;

    public int Health => _piece.Health;

    public Piece TakePiece()
    {
        if (!HasPiece) return null;
        Piece returnPiece = _piece;
        _piece = null;
        returnPiece.gameObject.SetActive(true);
        return returnPiece;
    }

    public bool AddPiece(Piece piece)
    {
        if (HasPiece) return false;
        _piece = piece;
        piece.transform.parent = transform;
        if (usePlaceholder)
            piece.transform.position = piecePlaceholder.position;
        if(disablePiece)
            piece.gameObject.SetActive(false);
        return true;
    }

    public void Repair(int health)
    {
        if (HasPiece) _piece.Repair(health);
    }
}