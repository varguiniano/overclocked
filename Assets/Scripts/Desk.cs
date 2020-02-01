using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

public class Desk : MonoBehaviour
{
    public DeskFillGauge DeskFillGauge;

    public bool PieceBroken => _pieceContainer.PieceBroken;

    public ParticleSystem Sparks;
    public ParticleSystem Lightnings;
    public ParticleSystem Smoke;
    
    public Action OnRepair;

    private PieceContainer _pieceContainer;
    public PieceManager PieceManager;


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
        if (_pieceContainer.PieceType == PieceManager.CPU)
        {
            Sparks.Play();
            Smoke.Play();
        }
        if (_pieceContainer.PieceType == PieceManager.PS)
        {
            Lightnings.Play();
            Smoke.Play();
        }
        if (_pieceContainer.PieceType == PieceManager.HDD)
        {
            Sparks.Play();
            Smoke.Play();
        }
        if (_pieceContainer.PieceType == PieceManager.GTX)
        {
            Lightnings.Play();
            Smoke.Play();
        }
        switch (repairType)
        {
            case RepairType.Hold:
            case RepairType.Smash: // Same for now.
                _pieceContainer.Repair(10);
                OnRepair?.Invoke();
                break;
        }
    }

    private void Update()
    {
        if (_pieceContainer.HasPiece) DeskFillGauge.SetValue(_pieceContainer.Health);
        else
        {
            DeskFillGauge.Disable();
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