using System.Collections.Generic;
using UnityEngine;

public class Shelves : MonoBehaviour
{
    public List<PieceContainer> PieceContainers = new List<PieceContainer>();
    
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
            if (PieceContainers[i].HasPiece && PieceContainers[i].PieceType == pieceType)
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