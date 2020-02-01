using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

public class Desk : MonoBehaviour
{
    public bool PieceBroken => _pieceContainer.PieceBroken;

    public Action OnRepair;

    private PieceContainer _pieceContainer;


    public bool CanReceivePiece(PieceType pieceType, bool broken)
    {
        return !_pieceContainer.HasPiece && _pieceContainer.AllowedPieceTypes.Contains(pieceType) && broken;
    }

    public bool CanGivePiece(PieceType pieceType, bool broken)
    {
        return _pieceContainer.HasPiece && _pieceContainer.PieceType == pieceType && !broken;
    }

    public void AddPiece(Piece piece)
    {
        _pieceContainer.AddPiece(piece);
    }

    public Piece TakePiece()
    {
        return _pieceContainer.TakePiece();
    }

    public void Repair(RepairType repairType)
    {
        switch (repairType)
        {
            case RepairType.Hold:
            case RepairType.Smash: // Same for now.
                _pieceContainer.Repair(10);
                OnRepair?.Invoke();
                break;
        }
    }

    public void Awake()
    {
        _pieceContainer = GetComponent<PieceContainer>();
        
        // TODO: Remove this debug when we have visual feedback.
        OnRepair += () => Logger.LogInfo("Repair!", this);
    }

    public enum RepairType
    {
        Smash,
        Hold
    }
}