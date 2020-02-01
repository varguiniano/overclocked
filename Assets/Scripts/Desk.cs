using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    private PieceContainer _pieceContainer;
    
    
    public bool CanReceivePiece(PieceType pieceType, bool broken)
    {
        return !_pieceContainer.HasPiece && _pieceContainer.AllowedPieceTypes.Contains(pieceType) &&  broken;
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
    public void Awake()
    {
        
        _pieceContainer =  GetComponent<PieceContainer>();
      
    }
}
