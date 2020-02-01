using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PieceContainer))]
public class Player : MonoBehaviour
{
   PlayerInput _playerInput;
   private CharacterController _characterController;
   private PieceContainer _pieceContainer;
   
   public PlayerAttributes Attributes;
   public PieceManager PieceManager;
   
   private Piece _piece;

   public Piece piece
   {
      get { return _piece; }
      set { _piece = value; }
   }

   [Header("Runtime Interactions")] 
   public PieceContainer otherContainer;

   public bool HasPiece => _pieceContainer.Count != 0;

   public void Awake()
   {
      _playerInput = GetComponent<PlayerInput>();
      _characterController = GetComponent<CharacterController>();
      _pieceContainer =  GetComponent<PieceContainer>();
   }
   

   public void Update()
   {
      if (_playerInput.actions["BTNCPU"].triggered)
      {
         Debug.Log("CPU");
      }

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

      if (otherContainer)
      {
         if (HasPiece)
         {
         }
         if (_playerInput.actions["BTNCPU"].triggered)
         {
            
         }
      }
   }

   public void OnTriggerEnter(Collider other)
   {
      PieceContainer oContainer = other.GetComponent<PieceContainer>();
      if (oContainer != null)
      {
         otherContainer = oContainer;
      }
   }

   public void OnTriggerStay(Collider other)
   {
      PieceContainer oContainer = other.GetComponent<PieceContainer>();
      if (oContainer != null)
      {
         otherContainer = oContainer;
      }
   }

   public void OnTriggerExit(Collider other)
   {
      otherContainer = null;
   }
}
