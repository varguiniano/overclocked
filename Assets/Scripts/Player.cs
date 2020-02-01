using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PieceContainer))]
public class Player : MonoBehaviour
{
    PlayerInput _playerInput;
    private CharacterController _characterController;
    private PieceContainer _pieceContainer;

    public Animator animator;
    private int _animatorSpeed = Animator.StringToHash("Speed");
    private int _carryingLayer;
    
    public PlayerAttributes Attributes;
    public PieceManager PieceManager;

    public Transform PiecePosition;
    public Transform RightHand;

    [Header("Runtime Interactions")] public Computer computer;

    public Desk desk;

    public TrashContainer trashContainer;
    
    public Shelves shelves;

    public bool HasPiece => _pieceContainer.HasPiece;

    public void AddPiece(Piece piece)
    {
        _pieceContainer.AddPiece(piece);
        piece.transform.parent = RightHand;
        piece.transform.position = PiecePosition.position;
        
    }

    public Piece TakePiece()
    {
        return _pieceContainer.TakePiece();
    }

    public void Awake()
    {
        
        _playerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        _pieceContainer = GetComponent<PieceContainer>();
        _carryingLayer = animator.GetLayerIndex("CarryPiece");
    }

    public void OnBTNGFX()
    {
        Debug.Log("GFX");
        if (CheckIfRepair())
        {
            desk.Repair(Desk.RepairType.Smash);
        }
        else
        {
            ExchangePieceProcedure(PieceManager.GTX);
        }
    }

    public void OnBTNCPU()
    {
        Debug.Log("CPU");
        if (CheckIfRepair())
        {
            desk.Repair(Desk.RepairType.Smash);
        }
        else
        {
            ExchangePieceProcedure(PieceManager.CPU);
        }
    }

    public void OnBTNHDD()
    {
        Debug.Log("HDD");
        if (CheckIfRepair())
        {
            desk.Repair(Desk.RepairType.Hold);
        }
        else
        {
            ExchangePieceProcedure(PieceManager.HDD);
        }
    }

    public void OnBTNPS()
    {
        Debug.Log("PS");
        if (CheckIfRepair())
        {
            desk.Repair(Desk.RepairType.Hold);
        }
        else
        {
            ExchangePieceProcedure(PieceManager.PS);
        }
    }

    public void ExchangePieceProcedure(PieceType pieceType)
    {
        if (computer != null)
        {
            if (HasPiece)
            {
                if (computer.CanReceivePiece(pieceType, _pieceContainer.PieceBroken))
                {
                    computer.AddPiece(TakePiece());
                }
            }
            else
            {
                if (computer.CanGivePiece(pieceType, computer.IsPieceBroken(pieceType)))
                {
                    AddPiece(computer.TakePiece(pieceType));
                }
            }
        }

        if (desk != null)
        {
            if (HasPiece)
            {
                if (desk.CanReceivePiece(pieceType, _pieceContainer.PieceBroken))
                {
                    desk.AddPiece(TakePiece());
                }
            }
            else
            {
                if (desk.CanGivePiece(pieceType, desk.PieceBroken))
                {
                    AddPiece(desk.TakePiece());
                }
            }
        }

        if (trashContainer != null && HasPiece &&
            trashContainer.CanReceivePiece(pieceType, _pieceContainer.PieceBroken))
        {
            trashContainer.AddPiece(TakePiece());
        }
        
        if (shelves != null)
        {
            if (HasPiece)
            {
                if (shelves.CanReceivePiece(pieceType, _pieceContainer.PieceBroken))
                {
                    shelves.AddPiece(TakePiece());
                }
            }
            else
            {
                if (shelves.CanGivePiece(pieceType, shelves.IsPieceBroken(pieceType)))
                {
                    AddPiece(shelves.TakePiece(pieceType));
                }
            }
        }
    }

    private bool CheckIfRepair()
    {
        return !_pieceContainer.HasPiece && desk != null && desk.PieceBroken;
    }

    public void Update()
    {
        animator.SetFloat(_animatorSpeed,_playerInput.actions["move"].ReadValue<Vector2>().magnitude);
        animator.SetLayerWeight(_carryingLayer,HasPiece ? 1 :0);
        
    }
    

    public void FixedUpdate()
    {
        // MOVEMENT *************************************************************
        Vector2 moveVector = _playerInput.actions["move"].ReadValue<Vector2>();
        transform.Rotate(0, moveVector.x, 0);
        Vector3 forward = new Vector3(moveVector.x, 0, moveVector.y);
        if (forward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(forward);
        }

        _characterController.SimpleMove(forward * Attributes.Speed);
        //************************************************************************
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Computer"))
        {
            computer = other.GetComponent<Computer>();
        }

        if (other.gameObject.CompareTag("Desk"))
        {
            desk = other.GetComponent<Desk>();
        }

        if (other.gameObject.CompareTag("TrashContainer"))
        {
            trashContainer = other.GetComponent<TrashContainer>();
        }
        
        if (other.gameObject.CompareTag("Shelves"))
        {
            shelves = other.GetComponent<Shelves>();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Computer"))
        {
            computer = other.GetComponent<Computer>();
        }

        if (other.gameObject.CompareTag("Desk"))
        {
            desk = other.GetComponent<Desk>();
        }

        if (other.gameObject.CompareTag("TrashContainer"))
        {
            trashContainer = other.GetComponent<TrashContainer>();
        }
        
        if (other.gameObject.CompareTag("Shelves"))
        {
            shelves = other.GetComponent<Shelves>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        computer = null;
        desk = null;
        trashContainer = null;
        shelves = null;
    }
}