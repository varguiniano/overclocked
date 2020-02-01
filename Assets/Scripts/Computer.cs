using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public PieceManager PieceManager;
    public List<PieceContainer> PieceContainers = new List<PieceContainer>();
    public ComputerUI ui;

    [Header("GlobalVariables")]
    public ConveyorSpeed ConveyorSpeed;
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
        ui.SetIcons(PieceContainers);
    }

    public void BuildComputer()
    {
        //Aqui debemos crear aleatoriamente los componentes del pc
        PieceContainer container =  gameObject.AddComponent<PieceContainer>();
        container.AllowedPieceTypes.Add(PieceManager.CPU);

        Piece piece = Instantiate(PieceManager.CPUPrefab, transform);
        piece.Health = 0;
        container.AddPiece(piece);
        PieceContainers.Add(container);
        container =  gameObject.AddComponent<PieceContainer>();
        container.AllowedPieceTypes.Add(PieceManager.PS);
        container.AddPiece(Instantiate(PieceManager.PSPrefab,transform));
        PieceContainers.Add(container);
    }

    public void Update()
    {
        transform.Translate(transform.right * (ConveyorSpeed.Value * Time.deltaTime));
    }

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
            if (PieceContainers[i].HasPiece && PieceContainers[i].PieceType == pieceType && broken)
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
                ui.TakePiece(PieceContainers[i]);
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
