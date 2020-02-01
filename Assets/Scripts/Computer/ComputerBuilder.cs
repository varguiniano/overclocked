using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ComputerBuilder",menuName = "ScriptableObjects/Builders/Computer")]
public class ComputerBuilder : ScriptableObject
{
    public PieceManager PieceManager;
    public ComputerManager ComputerManager;

    public Computer BuildComputer(ComputerDescriptor descriptor, ComputerSizeType size)
    {
        Computer computer = ComputerManager.InstantiatePrefab(size);
        if (computer != null)
        {
            for (int i = 0; i < descriptor.CPUS; i++)
            {
                PieceContainer container = computer.gameObject.AddComponent<PieceContainer>();
                container.AllowedPieceTypes.Add(PieceManager.CPU);
                Piece piece = PieceManager.InstantiatePiece(PieceManager.CPU);
                piece.Health = 0;
                container.AddPiece(piece);
                computer.PieceContainers.Add(container);
            }
            for (int i = 0; i < descriptor.GTX; i++)
            {
                PieceContainer container = computer.gameObject.AddComponent<PieceContainer>();
                container.AllowedPieceTypes.Add(PieceManager.GTX);
                Piece piece = PieceManager.InstantiatePiece(PieceManager.GTX);
                piece.Health = 0;
                container.AddPiece(piece);
                computer.PieceContainers.Add(container);
            }
            for (int i = 0; i < descriptor.HDD; i++)
            {
                PieceContainer container = computer.gameObject.AddComponent<PieceContainer>();
                container.AllowedPieceTypes.Add(PieceManager.HDD);
                Piece piece = PieceManager.InstantiatePiece(PieceManager.HDD);
                piece.Health = 0;
                container.AddPiece(piece);
                computer.PieceContainers.Add(container);
            }
            for (int i = 0; i < descriptor.PS; i++)
            {
                PieceContainer container = computer.gameObject.AddComponent<PieceContainer>();
                container.AllowedPieceTypes.Add(PieceManager.PS);
                Piece piece = PieceManager.InstantiatePiece(PieceManager.PS);
                piece.Health = 0;
                container.AddPiece(piece);
                computer.PieceContainers.Add(container);
            }
        }

        return computer;

    }
}
