using System;
using System.Collections.Generic;
using ChainsawMan.PlayerClass.ColliderScripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChainsawMan
{
    //A class to manage pathfinding, patrol and other things most enemies will need
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBehaviour : MonoBehaviour
    {
        //Public
        [HideInInspector]
        public Rigidbody2D rb;//enable access to this rigidbody by other classes/objects

        //Serialized fields
        [Tooltip("If canFall is true, this unit will be able to fall off platforms when patrolling")]
        [SerializeField] private float obstacleRayDistance;
        [SerializeField] private LayerMask obstacleLayers;
        [SerializeField] private float moveSpeed;
        [Tooltip("How far the enemy can look for the player horizontally")]
        [SerializeField] private float playerDetectionDistX;
        [Tooltip("How far the enemy can look for the player vertically")]
        [SerializeField] private float playerDetectionDistY;
        [Tooltip("The stopping distance the enemy will stop at when chasing the player")]
        [SerializeField] private float stoppingDistance;

        [Header("Enemy Droppable Items")] 
        [SerializeField] private List<GameObject> dropItems;

        [Range(0, 100)]
        [SerializeField] private float chanceOfDrop;

        private bool droppedItems;//make sure we don't drop items more than one time
        
        //Properties
        public bool IsFacingRight { get; private set; }//which direction the enemy is facing
        public bool IsObstacleOnRight { get; private set; }//Whether the enemy has found an obstacle on his right
        public bool IsObstacleOnLeft { get; private set; }//Whether the enemy has found an obstacle on his left
        
        //Fields
        private Transform player;
        private GroundCheck edgeDetection;//used for detecting the edge of a platform by casting the groundCheck circle by an offset, to the right or left of the enemy
        private float timer;//timer here is used by the EdgeDetection method to make the flip once edge is found not too sudden
        private bool edgeFound;
        private float tempSpeed;//a temporary speed variable used in calculation when enemy is stopped
        private bool isGrounded;
        
        private void Awake()
        {
            //Find the player's transform component by the Player tag
            player = GameObject.FindWithTag("Player").transform;
            if (player == null)
                player = GameObject.Find("Player").transform;
            
            edgeDetection = GetComponent<GroundCheck>();
            rb = GetComponent<Rigidbody2D>();
            
            edgeFound = false;
            tempSpeed = moveSpeed;

            droppedItems = false;
        }

        private void Update()
        {
            //Look for obstacles on both sides
            IsObstacleOnRight = Physics2D.Raycast(transform.position, Vector2.right, obstacleRayDistance, obstacleLayers);
            IsObstacleOnLeft = Physics2D.Raycast(transform.position, Vector2.right * -1f, obstacleRayDistance, obstacleLayers);
        }

        /// <summary>
        /// Make the enemy patrol the flat area he's on
        /// </summary>
        public void Patrol()
        {
            if(IsFacingRight)//move right
                transform.position += transform.right * moveSpeed * Time.deltaTime;
            else//move left
                transform.position += transform.right * -1f * moveSpeed * Time.deltaTime;
            
            //if enemy sees obstacle on left and he was facing left, flip him
            if (IsObstacleOnLeft && !IsFacingRight)
            {
                Flip();
            }
            //if enemy sees obstacle on right and he was facing right, flip him
            else if (IsObstacleOnRight && IsFacingRight)
            {
                Flip();
            }

            DetectEdge(); //make it so that enemy doesn't fall during patrol by detecting the edge
        }

        /// <summary>
        /// Make the enemy chase after the player
        /// </summary>
        public void ChasePlayer()
        {
            Vector3 dir = player.position - transform.position;//direction from enemy to player
            dir.y = 0;
            dir.z = 0;
            
            //Only move and flip sides of the enemy when not too close to the player, because if he does reach player too closely, he will flip too often
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.position += dir.normalized  * moveSpeed * Time.deltaTime;

                LookAtPlayer();
            }
        }

        /// <summary>
        /// Returns true if player was found near the enemy
        /// </summary>
        /// <returns></returns>
        public bool DetectPlayer()
        {
            //The reason we use the ".localToWorldMatrix.GetPosition()" method, is because if the enemy is a child of another game object in the hierarchy (e.g. empty gameObject "Enemies"), 
            //his local position (transform.position) will not give accurate calculations due to being affected by the transform parameter of its parent. As such, we use localToWorldMatrix
            float distX = Mathf.Abs(transform.localToWorldMatrix.GetPosition().x - player.position.x);//horizontal distance from player
            float distY = Mathf.Abs(transform.localToWorldMatrix.GetPosition().y - player.position.y);//vertical distance from player
            
            if (distX <= playerDetectionDistX && distY <= playerDetectionDistY)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Returns true if enemy is nearing the platform's edge, from which he can fall if canFall boolean is enabled.
        /// </summary>
        /// <returns></returns>
        public bool DetectEdge()
        {
            //If enemy is facing right and is soon to leave a platform, flip him to the left
            if (IsFacingRight && !edgeDetection.IsGrounded && timer == 0)
            {
                timer += Time.deltaTime;//we add timer here so that after it is incremented the first timer, the enemy won't flip immediately after
                Flip();
            }
            else if (!IsFacingRight && !edgeDetection.IsGrounded && timer == 0)
            {
                timer += Time.deltaTime;
                Flip();
            }

            if (edgeDetection.IsGrounded)//once enemy is not near edge anymore (isGrounded), we use a timer that so that he doesn't flip too fast when turning in the opposite way
            {
                timer = 0;
                edgeFound = false;
            }
            else
            {
                edgeFound = true;
            }

            return edgeFound;

        }

        /// <summary>
        /// Make the enemy look (and flip) in the direction of the player, if he's inside the detection radius
        /// </summary>
        public void LookAtPlayer()
        {
            if (DetectPlayer())
            {
                Vector3 dir = player.position - transform.position;//direction from enemy to player
                
                if (dir.x >= 0 && !IsFacingRight)//if player is to the right, and enemy is facing left, flip him
                {
                    Flip();
                }
                else if (dir.x < 0 && IsFacingRight)//if player is to the left, and enemy is facing right, flip him
                {
                    Flip();
                }

            }
        }

        /// <summary>
        /// Acquire the Transform component of the player character
        /// </summary>
        /// <returns></returns>
        public Transform GetPlayer()
        {
            return player;
        }
        
        //A method for turning the enemy from left to right, and vice-versa
        private void Flip()
        {
            IsFacingRight = !IsFacingRight;
            edgeDetection.FlipOffset();//make the enemy also flip its ground check circle, to face the correct direction the enemy is looking at

            // Multiply the player's x local scale by -1 to actually flip him.
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        /// <summary>
        /// Stops the enemy in its tracks, by making its movement speed zero
        /// </summary>
        public void StopMovement()
        {
            moveSpeed = 0;
            rb.velocity = Vector2.zero;
        }

        /// <summary>
        /// Resumes the enemy's movement, by returning its movement speed to its original value
        /// </summary>
        public void ResumeMovement()
        {
            moveSpeed = tempSpeed;
        }

        public bool IsGrounded()
        {
            return isGrounded;
        }

       /// <summary>
       /// KnockBack the enemy for a certain amount and apply a certain amount of camera shake
       /// </summary>
       /// <param name="knockBackForce"></param>
       /// <param name="cameraShake">The value should be from 0 to 10, with 1 being zero camera shake</param>
        public void KnockBack(float knockBackForce, float cameraShake)
        {
            //Shake the camera
            CinemachineShaker.Instance.ShakeCamera(cameraShake, 0.1f);//shake the camera
            
            rb.velocity = Vector2.zero;//if enemy was being pushed back before, make his velocity zero

            if(IsFacingRight)
                rb.AddForce(Vector2.right * -1f * knockBackForce, ForceMode2D.Force);//knock back to the left
            else
                rb.AddForce(Vector2.right * knockBackForce, ForceMode2D.Force);//knock back to the right
        }
        
       /// <summary>
       /// KnockUp the enemy for a certain amount and shake the camera for a certain amount
       /// </summary>
       /// <param name="knockUpForce"></param>
       /// <param name="cameraShake"></param>
        public void KnockUp(float knockUpForce, float cameraShake)
        {
            CinemachineShaker.Instance.ShakeCamera(cameraShake, 0.1f);//shake camera depending on how much knockUp Force the enemy receives
            
            rb.velocity = Vector2.zero;//if enemy was falling, before knocking him up again, make his velocity zero
            rb.AddForce(Vector2.up * knockUpForce, ForceMode2D.Force);//knock back to the left
        }

        /// <summary>
        /// Change the enemy's movement speed
        /// </summary>
        /// <param name="speed"></param>
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        /// <summary>
        /// Get the enemy's current moveSpeed
        /// </summary>
        /// <returns></returns>
        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        /// <summary>
        /// Make enemy drop items, if chance of drop is successful
        /// </summary>
        public void DropItems()
        {
            //If items were already dropped, return
            if (droppedItems)
                return;
            
            if (dropItems.Count > 0)//if list isn't empty
            {
                droppedItems = true;
                foreach (var drop in dropItems)//for each items in the list
                {
                    var dropRandomizer = Random.Range(0, 100);
                    if (dropRandomizer >= 100 - chanceOfDrop) //if chance of drop was successful, drop item
                    {
                        Instantiate(drop, transform.position, quaternion.identity); //drop items, a little above the enemy
                    }
                }
            }
            else// throw error
            {
                throw new InvalidOperationException("The items drop list is empty. The operation cannot be performed.");
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}
