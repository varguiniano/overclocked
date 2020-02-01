﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public PieceManager PieceManager;
    public List<PieceContainer> PieceContainers = new List<PieceContainer>();
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.black;
    }

    public void Start()
    {
        BuildComputer();
    }

    public void BuildComputer()
    {
        //Aqui debemos crear aleatoriamente los componentes del pc
        PieceContainer container =  gameObject.AddComponent<PieceContainer>();
        container.AllowedPieceTypes.Add(PieceManager.CPU);
        
        container.AddPiece(Instantiate(PieceManager.CPUPrefab,transform));
        PieceContainers.Add(container);
        container =  gameObject.AddComponent<PieceContainer>();
        container.AllowedPieceTypes.Add(PieceManager.PS);
        container.AddPiece(Instantiate(PieceManager.PSPrefab,transform));
        PieceContainers.Add(container);
    }

    public bool CanReceivePiece(PieceType pieceType)
    {
        for (int i = 0; i < PieceContainers.Count; i++)
        {
            if (!PieceContainers[i].HasPiece && PieceContainers[i].AllowedPieceTypes.Contains(pieceType))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CanGivePiece(PieceType pieceType)
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
}