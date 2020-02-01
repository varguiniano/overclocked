using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceContainer : MonoBehaviour
{
    public List<PieceType> AllowedPieceTypes = new List<PieceType>();
    [Range(0,20)]
    public int capacity = 1;
    
    [SerializeField]
    private List<Piece> _pieces = new List<Piece>();

    
    public int Count
    {
        get => _pieces.Count;
    }

    public bool Contains(Piece piece)
    {
        return _pieces.Contains(piece);
    }

    public Piece TakePiece(int index)
    {
        Piece returnPiece = _pieces[index];
        _pieces.RemoveAt(index);
        return returnPiece;
    }
    
    public Piece TakePiece()
    {
        if (_pieces.Count > 0)
        {
            Piece firstPiece = _pieces[0];
            _pieces.RemoveAt(0);
            return firstPiece;
        }

        return null;
    }

    public Piece TakePiece(PieceType pieceType)
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            if (_pieces[i].pieceType == pieceType)
            {
                Piece returnPiece = _pieces[i];
                _pieces.RemoveAt(i);
                return returnPiece;
            }
        }

        return null;
    }

    public bool AddPiece(Piece piece)
    {
        if (_pieces.Count >= capacity)
        {
            return false;
        }
        _pieces.Add(piece);
        return true;
    }
}
