using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PieceDescriptor
{
    public PieceType PieceType;
    public int Health;

    public PieceDescriptor(PieceType pieceType, int health)
    {
        Health = health;
        PieceType = pieceType;
    }
}
[Serializable]
public struct ComputerDescriptor
{
    public int CPUS;
    public int GTX;
    public int HDD;
    public int PS;

    public ComputerDescriptor(int cpus, int gtx, int hdds, int pss)
    {
        CPUS = cpus;
        GTX = gtx;
        HDD = hdds;
        PS = pss;
    }
}
