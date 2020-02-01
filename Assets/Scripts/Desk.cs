using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    private PieceContainer _pieceContainer;
    
    
    public bool CanReceivePiece(PieceType pieceType)
    {
        if (!_pieceContainer.HasPiece && _pieceContainer.AllowedPieceTypes.Contains(pieceType))
        {
            return true;
        }
        return false;
    }
    
    public bool CanGivePiece(PieceType pieceType)
    {
        if (_pieceContainer.HasPiece && _pieceContainer.PieceType == pieceType)
        {
            return true;
        }
        return false;
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
