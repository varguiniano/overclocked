using UnityEngine;

public class TrashContainer : MonoBehaviour
{
    public bool CanReceivePiece(PieceType pieceType, bool broken)
    {
        return true;
    }

    public bool CanGivePiece(PieceType pieceType, bool broken)
    {
        return false;
    }

    public void AddPiece(Piece piece)
    {
        Destroy(piece.gameObject);
    }
}