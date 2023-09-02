using System.Collections;
using ChainsawMan.PlayerClass.ColliderScripts;
using UnityEngine;

namespace ChainsawMan.PlayerClass
{
    /// <summary>
    /// Class <c>CharacterController2D </c> will be used by other PlayerController State classes to control the player.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(PlatformPhasingDown))]
    public class CharacterController2D : MonoBehaviour
    {
        //Serialized fields
        [SerializeField] private GroundCheck groundCheck;//custom ground check class
        [SerializeField] private Rigidbody2D rigidBody2D;
        [SerializeField] private ForceMode2D jumpForce = ForceMode2D.Force;
        [SerializeField] private float gravityScale;
        [SerializeField] private float maxVelocity;//player's max horizontal movement velocity
        
        //Pubic fields
        [Tooltip("How much time the player has to be able to jump, after leaving a platform.")]
        [Range(0f, 1f)]
        public float coyoteJumpPeriod = 0.25f;

        //Private fields
        private bool isFacingRight;
        private float moveInput;
        
        ///Instead of checking if player isGrounded to be able to jump, we instead see the last time he was grounded to 
        //figure out the time we have for him to be able to jump (even briefly while in air).
        public float? lastGroundedTime;

        private Transform playerTransform;
        
        private void Start()
        {
            isFacingRight = true;
            playerTransform = transform;//we do this because repeatedly using transform.position (aka property access) is inefficient 
        }

        private void Update()
        {
            //Coyote Jump
            if (IsGrounded)
                lastGroundedTime = Time.time;

            #region Movement Parameters

            moveInput = InputManager.instance.GetMoveHor();
            
            //Make the player stop sharply after quickly turning in the opposite direction
            if (rigidBody2D.velocity.x < 0 && moveInput > 0)
            {
                rigidBody2D.velocity = new Vector2(0, rigidBody2D.velocity.y);
            }
            else if (rigidBody2D.velocity.x > 0 && moveInput < 0)
            {
                rigidBody2D.velocity = new Vector2(0, rigidBody2D.velocity.y);
            }
            
            //if player is still gliding on ground, stop him
            if(rigidBody2D.velocity.x != 0 && moveInput == 0f && groundCheck.IsGrounded) 
                rigidBody2D.velocity = new Vector2(0, rigidBody2D.velocity.y);
            
            //Make player fall faster
            if(rigidBody2D.velocity.y < 0)
                rigidBody2D.AddForce(Vector2.down * gravityScale, ForceMode2D.Force);

            // Keep max velocity in check
            if (rigidBody2D.velocity.x > maxVelocity) //if player goes right too fast
            {
                rigidBody2D.velocity = new Vector2(maxVelocity, rigidBody2D.velocity.y);
            }
            else if (rigidBody2D.velocity.x < -maxVelocity) //if player goes left too fast
            {
                rigidBody2D.velocity = new Vector2(-maxVelocity, rigidBody2D.velocity.y);
            }
            
            #endregion
        }

        public Rigidbody2D Rigidbody
        {
            get { return rigidBody2D; }
        }

        //Get or Set the current Vector2 velocity of the player
        public Vector2 Velocity
        {
            get { return rigidBody2D.velocity; }
            set { rigidBody2D.velocity = value; }
        }
        
        public void Jump(float jumpHeight)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);//we do this so that added gravity doesn't stop the player from making a full jump
            rigidBody2D.AddForce(Vector2.up * jumpHeight, jumpForce);
        }

        public void Dash(float dashForce)
        {
            //if player is dashing whilst in the air, stop midair and then dash
            rigidBody2D.velocity = Vector2.zero;//we do this so that added gravity doesn't stop the player from making a full jump

            //Don't take moveInput into calculations, so that player can dash even when idle
            if(isFacingRight)//if player faces right direction, dash in that direction
                rigidBody2D.AddForce(Vector2.right * dashForce, jumpForce);
            else//same for left
                rigidBody2D.AddForce(Vector2.right * -1f * dashForce, jumpForce);

        }
        
        public bool IsGrounded
        {
            get { return groundCheck.IsGrounded;}
        }

        public void Move(float moveSpeed)
        {
            //Normalize movementInput for gamepads
            if (moveInput < 0)
                moveInput = -1f;
            else if (moveInput > 0)
                moveInput = 1;
            
            if (moveInput != 0f) 
                playerTransform.position += transform.right * (moveInput * moveSpeed * Time.deltaTime);

            //if player presses D and he was facing left, flip him
            if (moveInput > 0f && !isFacingRight)
            {
                Flip();
            }
            //if player presses A and he was facing right, flip him
            else if (moveInput < 0f && isFacingRight)
            {
                Flip();
            }

        }
        
        private void Flip()
        {
            isFacingRight = !isFacingRight;

            // Multiply the player's x local scale by -1 to actually flip him.
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        /// <summary>
        /// Return true if player is within the Coyote Jump period since leaving the ground
        /// </summary>
        /// <returns></returns>
        public bool CanCoyoteJump()
        {
            if(Time.time - lastGroundedTime <= coyoteJumpPeriod)
                return true;

            return false;
        }
    }
}