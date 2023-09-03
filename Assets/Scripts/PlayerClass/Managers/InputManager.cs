using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChainsawMan.PlayerClass
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour//we implement the IPlayerActions interface to gain access to player input in the PlayerInputActions asset
    {
        //Reference to the C# script we generated from the PlayerInputActions asset
        private PlayerInput PlayerInput;
        //Singleton 
        public static InputManager instance = null;

        //Input Actions - GamePlay
        private InputAction moveAction;
        private InputAction attackAction;
        private InputAction knockupAction;
        private InputAction jumpAction;
        private InputAction dashAction;
        private InputAction crouchAction;

        //Input Actions - UI
        private InputAction pauseAction;
        
        private bool wasAttackPressed;
        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
            }

            PlayerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            PlayerInput.actions.Enable();
            
            moveAction = PlayerInput.actions.FindAction("Move");
            attackAction = PlayerInput.actions.FindAction("Attack");
            knockupAction = PlayerInput.actions.FindAction("KnockUp");
            jumpAction = PlayerInput.actions.FindAction("Jump");
            dashAction = PlayerInput.actions.FindAction("Dash");
            pauseAction = PlayerInput.actions.FindAction("Pause");
            crouchAction = PlayerInput.actions.FindAction("Crouch");

        }

        private void OnDisable()
        {
            PlayerInput.actions.Disable();
        }

        #region Input Public Access Methods
        
        /// <summary>
        /// Get the player's horizontal movement input (-1, 1)
        /// </summary>
        /// <returns></returns>
        public float GetMoveHor()//player's horizontal movement 
        {
            //return moveVector;
            return moveAction.ReadValue<float>();
        }

        /// <summary>
        /// Return true if player pressed the attack button
        /// </summary>
        /// <returns></returns>
        public bool GetAttack()
        {
            return attackAction.WasPerformedThisFrame();
        }
        
        /// <summary>
        /// Return true if player performed the knockUp button
        /// </summary>
        /// <returns></returns>
        public bool GetKnockUpPerformed()
        {
            return knockupAction.WasPerformedThisFrame();
        }
        
        /// <summary>
        /// Return true if player is pressing the knockUp button
        /// </summary>
        /// <returns></returns>
        public bool GetKnockUpPress()
        {
            return knockupAction.IsPressed();
        }
        
        /// <summary>
        /// Return true if player released the knockUp button
        /// </summary>
        /// <returns></returns>
        public bool GetKnockUpRelease()
        {
            return knockupAction.WasReleasedThisFrame();
        }

        /// <summary>
        /// Return true player pressed the jump button  
        /// </summary>
        /// <returns></returns>
        public bool GetJump()
        {
            return jumpAction.WasPressedThisFrame();
        }

        /// <summary>
        /// Return true if player pressed pause button
        /// </summary>
        /// <returns></returns>
        public bool GetPause()
        {
            return pauseAction.WasPerformedThisFrame();
        }

        /// <summary>
        /// Return true if player pressed dash button only once
        /// </summary>
        /// <returns></returns>
        public bool GetDash()
        {
            return dashAction.WasPerformedThisFrame();
        }
        
        /// <summary>
        /// Return true if player pressed the crouch button/axis 
        /// </summary>
        /// <returns></returns>
        public bool GetCrouch()
        {
            return crouchAction.WasPerformedThisFrame();
        }
        
        #endregion
    }
}