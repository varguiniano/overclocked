using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PieceContainer))]
public class Player : MonoBehaviour
{
   PlayerInput _playerInput;
   private CharacterController _characterController;
   private PieceContainer _pieceContainer;
   
   public PlayerAttributes Attributes;
   public PieceManager PieceManager;

   [Header("Runtime Interactions")] 
   public Computer computer;

   public Desk desk;

   public bool HasPiece => _pieceContainer.HasPiece;

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
      _playerInput = GetComponent<PlayerInput>();
      _characterController = GetComponent<CharacterController>();
      _pieceContainer =  GetComponent<PieceContainer>();
      
   }

   public void OnBTNGFX()
   {
      Debug.Log("GFX");
      ExchangePieceProcedure(PieceManager.GTX);
   }
   public void OnBTNCPU()
   {
      Debug.Log("CPU");
      ExchangePieceProcedure(PieceManager.CPU);
   }
   public void OnBTNHDD()
   {
      Debug.Log("HDD");
      ExchangePieceProcedure(PieceManager.HDD);
   }
   public void OnBTNPS()
   {
      Debug.Log("PS");
      ExchangePieceProcedure(PieceManager.PS);
   }

   public void ExchangePieceProcedure(PieceType pieceType)
   {
      if (computer != null)
      {
         if (HasPiece)
         {
            if (computer.CanReceivePiece(pieceType))
            {
               computer.AddPiece(TakePiece());
            }
         }
         else
         {
            if (computer.CanGivePiece(pieceType))
            {
               AddPiece(computer.TakePiece(pieceType));
            }
         }
      }
      if (desk != null)
      {
         if (HasPiece)
         {
            if (desk.CanReceivePiece(pieceType))
            {
               desk.AddPiece(TakePiece());
            }
         }
         else
         {
            if (desk.CanGivePiece(pieceType))
            {
               AddPiece(desk.TakePiece());
            }
         }
      }
   }
   


   public void Update()
   {

      // MOVEMENT *************************************************************
      Vector2 moveVector = _playerInput.actions["move"].ReadValue<Vector2>();
      transform.Rotate(0, moveVector.x, 0);
      Vector3 forward = new Vector3(moveVector.x,0,moveVector.y);
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
   }

   public void OnTriggerExit(Collider other)
   {
      computer = null;
      desk = null;
   }
}
