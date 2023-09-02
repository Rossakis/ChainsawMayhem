using System;
using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace ChainsawMan
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EnemyBehaviour))]
    [RequireComponent(typeof(EnemyHealth))]
    public class ZombieRanged : Enemy
    {
        [Tooltip("Whether this enemy can fall when chasing the player")]
        [SerializeField] private bool canFall;

        [SerializeField] private float minAttackingDistance;//the minimum stopping distance
        [SerializeField] private float maxAttackingDistance;//the maximum stopping distance 
        private float attackDistance;
        
        [SerializeField] private float attackCoolDown;
        public AnimationCurve curve;//used for KnifeThrow to calculate the arch it will go through in the air

        [SerializeField] private GameObject knife;
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private float heightY = 3.0f;
        [SerializeField] private float idleTimer = 1.0f;
        
        //Animation referencing
        [SerializeField] private AnimationClip idleAnimation;
        [SerializeField] private AnimationClip walkAnimation;
        [SerializeField] private AnimationClip attackAnimation;
        [SerializeField] private AnimationClip knockedUpAnimation;
        [SerializeField] private AnimationClip deathAnimation;

        private Animator animator;

        //Animation hashing
        private int idleHash;
        private int walkHash;
        private int attackHash;
        private int knockedUpHash;
        private int deathHash;

        private EnemyBehaviour enemyBehaviour;
        private EnemyHealth enemyHealth;
        
        //[SerializeField] private Transform player;
        private float currentHealth;
        
        // The height the projectile will travel upwards before falling.
        private GameObject knifeObj;//the knife gameObject to be instantiated from the Knife prefab
        private float lastAttackTime;
        private bool isOnCooldown;
        private bool hasAttacked;

        private Vector2 playerDirection;//direction towards the player
        private float distanceFromPlayer;

        private void Awake()
        {
            //Animation hashing
            idleHash = Animator.StringToHash(idleAnimation.name);
            walkHash = Animator.StringToHash(walkAnimation.name);
            attackHash = Animator.StringToHash(attackAnimation.name);
            knockedUpHash = Animator.StringToHash(knockedUpAnimation.name);
            deathHash = Animator.StringToHash(deathAnimation.name);

            //Components referencing
            enemyBehaviour = GetComponent<EnemyBehaviour>();
            animator = GetComponent<Animator>();
            enemyHealth = GetComponent<EnemyHealth>();

            //Sprite Renderer - Sorting Layers
            GetComponent<SpriteRenderer>().sortingOrder = Random.Range(1, 3);//make enemies appear dynamically in front or behind the player, to make them seem like a mob

            lastAttackTime = 0;
            isOnCooldown = false;
        }

        private States currentState = States.Patrol;

        private void Start()
        {
            attackDistance = Random.Range(minAttackingDistance, maxAttackingDistance);//the enemy will have attacking distance randomly decided by min and max attacking distances,
                                                                                      //so that enemies don't stack upon each other when attacking
        }

        private void Update()
        {
            CheckAttackCooldown();
            
            playerDirection = transform.position - enemyBehaviour.GetPlayer().position;
            distanceFromPlayer = Vector2.Distance(transform.position, enemyBehaviour.GetPlayer().position);

            //KnockUp
            if (!enemyBehaviour.IsGrounded())//if enemy is in the air, change state to knockedUp
            {
                currentState = States.KnockedUp;
            }

            //Death
            if (enemyHealth.IsDead)
                currentState = States.Dead;
            
            switch (currentState)
            {
                case States.Idle://not used currently
                    Idle();
                    break;
                case States.Patrol:
                    Patrol();
                    break;
                case States.Chase:
                    Chase();
                    break;
                case States.Attack:
                    Attack();
                    break;
                case States.KnockedUp:
                    KnockedUp();
                    break;
                case States.Dead:
                    Death();
                    break;
            }
        }
        
        private void KnockedUp()
        {
            animator.Play(knockedUpHash);
            
            if(enemyBehaviour.rb.velocity.y is >= -0.1f and <= 0.1f)
            {
                currentState = States.Patrol;
            }
        }

        void Idle()
        {
            animator.Play(idleHash);
            StartCoroutine(IdleTimer());
        }

        private IEnumerator IdleTimer()
        {
            yield return new WaitForSeconds(idleTimer);
            enemyBehaviour.ResumeMovement();
            currentState = States.Patrol;
        }

        void Patrol()
        {
            enemyBehaviour.Patrol();
            animator.Play(walkHash);

            //if enemy can't fall 
            if (!canFall)
            {
                //if player is found and enemy hasn't detected an edge, chase him
                if (enemyBehaviour.DetectPlayer() && !enemyBehaviour.DetectEdge())
                    currentState = States.Chase;
            }
            else//if enemy can fall
            {
                //immediately chase player, even if enemy will fall to get him
                if (enemyBehaviour.DetectPlayer())
                    currentState = States.Chase;
            }
        }

        void Chase()
        {
            //If edge of a platform is found, don't continue chasing after the player
            if (enemyBehaviour.DetectEdge())
            {
                currentState = States.Patrol;
                return;
            }

            enemyBehaviour.ChasePlayer();
            animator.Play(walkHash);

            if (distanceFromPlayer > attackDistance) //if no player is found in the attacking distance, swtich to patrol
            {
                currentState = States.Patrol;
            }
            else
            {
                currentState = States.Attack;
            }
        }

        void Attack()
        {
            if (distanceFromPlayer > attackDistance) //if no player is found in the attacking distance, swtich to patrol
                currentState = States.Patrol;
            
            if(knifeObj != null)
                Destroy(knifeObj, 2f);

            if (!isOnCooldown) //if enough time has passed (the attack cooldown time), then you can attack
            {
                animator.Play(attackHash);
                StartCoroutine(KnifeAttackTimer());
            }
            else if(isOnCooldown)
            {
                // If the attack animation is still playing, we want to wait for it to finish.
                animator.Play(idleHash);
            }
            
            //Make the zombie look in the direction of the player
            enemyBehaviour.LookAtPlayer();
        }
        
        //A method to make sure the attackCooldown has passed before the enemy can attack again
        private void CheckAttackCooldown()
        {
            if(Time.time - lastAttackTime < attackCoolDown)
                isOnCooldown = true;
            else
                isOnCooldown = false;
        }

        //A coroutine that times for the knife to be thrown after the attack animation ends
        private IEnumerator KnifeAttackTimer()
        {
            //wait the remaining time of the animation, since normalizedTime is (0,1), so 1 or 0.9f -normalizedTime gives an approximate remaining time
            yield return new WaitForSeconds(1 - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            if (knifeObj == null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9)//only throw knife if there is no other instance of it and if the attack animation has almost reached its end
            {
                lastAttackTime = Time.time; //mark the time the attack happened
                knifeObj = Instantiate(knife, transform.position, Quaternion.identity);

                //Knife Throw trajectory parameters
                if (playerDirection.x > 0) //if player is too far to the left of the enemy
                {
                    StartCoroutine(ThrowKnife(transform.position,
                        (Vector2) enemyBehaviour.GetPlayer().position + Vector2.down * 10 + Vector2.left * Random.Range(0, 6)));//The reason we add Vector2.down and Vector2.right calculations,
                                                                                                            //is because the animation curve is set up in such a way that the knife will stop at player's position during the 
                                                                                                            //fall and won't continue falling downwards without adding a multiplied Vector2.down in its trajectory. The Vector2.right is need so that 
                                                                                                            //the knife doesn't fall too soon before it reached the player, due to the multiplied Vector2.down we added before 
                }
                else if (playerDirection.x < 0) //if player is too far to the right of the enemy
                {
                    StartCoroutine(ThrowKnife(transform.position,
                        (Vector2) enemyBehaviour.GetPlayer().position + Vector2.down * 10 + Vector2.right * Random.Range(0, 6)));
                }
                else //if player is close enough on either side, DON'T add a left or right Vector2 bonus coordinate for the throw
                {
                    StartCoroutine(ThrowKnife(transform.position,
                        (Vector2) enemyBehaviour.GetPlayer().position + Vector2.down * 10 + Vector2.right));
                }
            }
            else
                yield return null;
        }

        //A coroutine for the knife throw itself, that uses Animation curve to calculate the trajectory of the knife
        private IEnumerator ThrowKnife(Vector3 start, Vector2 target)
        {
            float timePassed = 0f;

            Vector2 end = target;//temporary variable

            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;

                float linearT = timePassed / duration;
                float heightT = curve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, heightY, heightT);
                
                if(knifeObj != null)
                    knifeObj.transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

                yield return null;
            }
        }

        private void Death()
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true; //Make it so that before the enemy's body disappears, the player won't be able to hit them anymore
            animator.Play(deathHash);
            enemyBehaviour.DropItems();//make enemy drop items (if successful)
            Destroy(gameObject, 1.5f);
        }
        
        public override States GetCurrentState()
        {
            return currentState;
        }

        
    }
}
