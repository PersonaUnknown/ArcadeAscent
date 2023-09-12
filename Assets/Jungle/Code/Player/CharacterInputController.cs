using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jungle
{
    // This script handles player input by checking bools
    public class CharacterInputController : MonoBehaviour
    {
        private PlayerControls playerControls;
        private bool isMovingLeft;
        private bool isMovingRight;
        private bool isJumping;
        private bool isClimbingUp;
        private bool isClimbingDown;

        private void Awake()
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Enable();
            playerControls.PlayerMovement.MoveLeft.started += OnMoveLeft;
            playerControls.PlayerMovement.MoveLeft.canceled += OnMoveLeftEnd;
            playerControls.PlayerMovement.MoveRight.started += OnMoveRight;
            playerControls.PlayerMovement.MoveRight.canceled += OnMoveRightEnd;
            playerControls.PlayerMovement.Jump.started += OnJump;
            playerControls.PlayerMovement.Jump.canceled += OnJumpEnd;
            playerControls.PlayerMovement.ClimbUp.started += OnClimbUp;
            playerControls.PlayerMovement.ClimbUp.canceled += OnClimbUpEnd;
            playerControls.PlayerMovement.ClimbDown.started += OnClimbDown;
            playerControls.PlayerMovement.ClimbDown.canceled += OnClimbDownEnd;

            InitValues();
        }

        private void InitValues()
        {
            isMovingLeft = false;
            isMovingRight = false;
            isJumping = false;
            isClimbingUp = false;
            isClimbingDown = false;
        }

        private void OnMoveLeft(InputAction.CallbackContext context)
        {
            isMovingLeft = true;
        }
        private void OnMoveLeftEnd(InputAction.CallbackContext context)
        {
            isMovingLeft = false;
        }
        private void OnMoveRight(InputAction.CallbackContext context)
        {
            isMovingRight = true;
        }
        private void OnMoveRightEnd(InputAction.CallbackContext context)
        {
            isMovingRight = false;
        }
        private void OnJump(InputAction.CallbackContext context)
        {
            isJumping = true;
        }
        private void OnJumpEnd(InputAction.CallbackContext context)
        {
            isJumping = false;
        }
        private void OnClimbUp(InputAction.CallbackContext context)
        {
            isClimbingUp = true;
        }
        private void OnClimbUpEnd(InputAction.CallbackContext context)
        {
            isClimbingUp = false;
        }
        private void OnClimbDown(InputAction.CallbackContext context)
        {
            isClimbingDown = true;
        }
        private void OnClimbDownEnd(InputAction.CallbackContext context)
        {
            isClimbingDown = false;
        }
        public bool IsMovingLeft()
        {
            return isMovingLeft;
        }
        public bool IsMovingRight()
        {
            return isMovingRight;
        }
        public bool IsJumping()
        {
            return isJumping;
        }
        public bool IsClimbingUp()
        {
            return isClimbingUp;
        }
        public bool IsClimbingDown()
        {
            return isClimbingDown;
        }
    }
}
