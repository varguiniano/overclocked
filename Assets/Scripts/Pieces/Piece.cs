using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceType pieceType;

    public bool Broken => _health < 100;

    [SerializeField]
    [Range(0,100)]
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
}
