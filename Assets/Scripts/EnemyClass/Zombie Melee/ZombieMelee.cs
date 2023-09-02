using System.Collections;
using ChainsawMan.PlayerClass;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

namespace ChainsawMan
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EnemyBehaviour))]
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class ZombieMelee : Enemy
    {
        [Tooltip("Whether this enemy can fall when chasing the player")]
        [SerializeField] private bool canFall;
        
        [SerializeField] private float minAttackingDistance; //the minimum stopping distance
        [SerializeField] private float maxAttackingDistance; //the maximum stopping distance 
        private float attackDistance;
        [SerializeField] private float grabDistance;//the max distance the grab can be activated from
        [Tooltip("When the enemy does the grab animation, add a temporary speed boost to him")]
        [SerializeField] private float grabSpeedBoost;
        [SerializeField] private float damage;
        [SerializeField] private float attackCoolDown;
        [SerializeField] private float grabCoolDown;

        [Tooltip("How long the enemy will remain in the flinch state")]
        [SerializeField] private float flinchTime;
        [SerializeField] private float maxFlinchAmountOfTimes;
        [SerializeField] private float flinchResetTime;
        private float flinchedAmountOfTimes;//how many times the enemy has flinched so far
        
        [Tooltip("How often the player should be damage when grabbed")]
        [SerializeField] private float grabDamageInterval;
        [SerializeField] private float grabDamage;//the damage player will receive each grabDamageInterval
        [Tooltip("How often the chance will be rolled for the next grab to happen (e.g. every 1 second, every 0.5 seconds, etc...)")]
        [SerializeField] private float grabChanceInterval;
        
        [Tooltip("How much chance (out of 100%) is there for the grab action to be performed by the zombie")]
        [SerializeField] private float chanceOfGrabbing;//how much chance
        private bool startedGrabDamaging;//do this check so that enemy doesn't repeatedly attack the player

        
        //Animation referencing
        [SerializeField] private AnimationClip idleAnimation; 
        [SerializeField] private AnimationClip walkAnimation; 
        [SerializeField] private AnimationClip attackAnimation; 
        [SerializeField] private AnimationClip grabAnimation;
        [SerializeField] private AnimationClip knockedUpAnimation; 
        [SerializeField] private AnimationClip flinchAnimation;
        [SerializeField] private AnimationClip deathAnimation;

        private Animator animator;

        //Animation hashing
        private int idleHash;
        private int walkHash;
        private int attackHash;
        private int grabHash;
        private int knockedUpHash;
        private int flinchHash;
        private int deathHash;

        private EnemyBehaviour enemyBehaviour;
        private EnemyHealth enemyHealth;
        private SpriteRenderer spriteRenderer;
        public PlayerController player;

        private float currentHealth;
        private float lastAttackTime;//the last time the enemy has performer an attack (not grab)
        private float lastGrabTime;
        private bool hasAttacked;
        private bool hasBegunRangedAttack;
        private float distanceFromPlayer;
        private bool grabbedPlayer;
        private float grabChanceRandomiser;
        //Temp fields
        private float initialMoveSpeed;//a temporary filed that will store the enemy's initial moveSpeed (which will later be change by grabSpeedBoost), to later restore moveSpeed to the default

        private void Awake()
        {
            //Animation hashing
            idleHash = Animator.StringToHash(idleAnimation.name);
            walkHash = Animator.StringToHash(walkAnimation.name);
            attackHash = Animator.StringToHash(attackAnimation.name);
            grabHash = Animator.StringToHash(grabAnimation.name);

            knockedUpHash = Animator.StringToHash(knockedUpAnimation.name);
            flinchHash =  Animator.StringToHash(flinchAnimation.name);
            deathHash = Animator.StringToHash(deathAnimation.name);

            //Components referencing
            enemyBehaviour = GetComponent<EnemyBehaviour>();
            animator = GetComponent<Animator>();
            enemyHealth = GetComponent<EnemyHealth>();

            //Sprite Renderer - Sorting Layers
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = Random.Range(2, 4);//make enemies appear dynamically in front or behind the player, to make them seem like a mob
            lastAttackTime = 0;
            lastGrabTime = 0;
            
            //Player reference
            player = GameObject.FindWithTag("Player").GetComponentInParent<PlayerController>();
            
            //Temp value assignment
            initialMoveSpeed = enemyBehaviour.GetMoveSpeed();
        }

        private States currentState = States.Patrol;

        private void Start()
        {
            flinchedAmountOfTimes = 0;
            
            attackDistance = Random.Range(minAttackingDistance, maxAttackingDistance); //the enemy will have attacking distance randomly decided by min and max attacking distances,
                                                                                       //so that enemies don't stack upon each other when attacking
        }

        private void Update()
        {
            distanceFromPlayer = Vector2.Distance(transform.position, enemyBehaviour.GetPlayer().position);

            //Grab
            if(player.GetCurrentState() != player.GrabbedByZombie && grabbedPlayer)//if player was grabbed, but now gets released, make this enemy reappear
            {
                spriteRenderer.enabled = true;
                grabbedPlayer = false;
                lastGrabTime = Time.time;
                lastAttackTime = Time.time;
            }

            // if (grabbedPlayer)
            //     currentState = States.Grab;

            //KnockUp state condition
            if (!enemyBehaviour.IsGrounded() && enemyBehaviour.rb.velocity.y >= 0f) 
            {
                currentState = States.KnockedUp;
            }
            //Flinch state condition
            else if (enemyHealth.HasTakenDamage() && flinchedAmountOfTimes < maxFlinchAmountOfTimes)//flinch only on ground, if enemy hasn't already flinched too many times
            {
                flinchedAmountOfTimes++;
                currentState = States.Flinch;
            }
            //Check if enemy has flinched maximum amount of times
            if (flinchedAmountOfTimes >= maxFlinchAmountOfTimes)//if enemy has reached maximum amount of flinches, reset them after a short while
                StartCoroutine(FlinchTimesReset());

            //Dead state condition
            if (enemyHealth.IsDead)
            {
                if (currentState == States.Grab)//If enemy dies while grabbing, release the player from grab state
                    player.ChangeState(player.Idle);
                
                currentState = States.Dead;
            }

            switch (currentState)
            {
                case States.Patrol:
                    Patrol();
                    break;
                case States.Chase:
                    Chase();
                    break;
                case States.Attack:
                    Attack();
                    break;
                case States.Grab:
                    Grab();
                    break;
                case States.KnockedUp:
                    KnockedUp();
                    break;
                case States.Flinch:
                    Flinch();
                    break;
                case States.Dead:
                    Death();
                    break;
            }
        }

        private void KnockedUp()
        {
            animator.Play(knockedUpHash);

            if (enemyBehaviour.IsGrounded())//only change back to patrol once enemy comes back on the ground
            {
                currentState = States.Patrol;
            }
        }

        private void Flinch()
        {
            animator.Play(flinchHash);
            StartCoroutine(FlinchReset());
        }

        //A timer to reset the amount of flinches the enemy can go through at a time
        private IEnumerator FlinchReset()
        {
            yield return new WaitForSeconds(flinchTime);
            
            lastAttackTime = Time.time;//reset attack time so enemy doesn't immediately attack after flinch has ended
            
            if(enemyBehaviour.DetectPlayer())
                currentState = States.Attack;//cut immediately to Attack state
            else 
                currentState = States.Patrol;//cut immediately to Attack state
        }
        
        //Reset flinchedAmountOfTimes back to zero
        private IEnumerator FlinchTimesReset()
        {
            yield return new WaitForSeconds(flinchResetTime);
            flinchedAmountOfTimes = 0;
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
                //immediately chase player, even he fell and enemy will fall to get him
                if (enemyBehaviour.DetectPlayer())
                    currentState = States.Chase;
            }

        }

        void Chase()
        {
            enemyBehaviour.ChasePlayer();
            animator.Play(walkHash);
            
            //Grab chance
            StartCoroutine(GrabChanceRandomizer());//always try the chance to grab the player
            //E.g. 100 - 75 = 25, which means that rand will have to be bigger than 25 which is easy
            
            if (distanceFromPlayer > grabDistance || enemyBehaviour.DetectEdge())//if player is too far or  edge of a platform is found, switch to patrol
                currentState = States.Patrol;
            else if (distanceFromPlayer <= grabDistance && distanceFromPlayer > attackDistance)//if in grab distance, then also check
            {
                if (grabChanceRandomiser >= 100 - chanceOfGrabbing && player.GetCurrentState() != player.GrabbedByZombie && Time.time - lastGrabTime >= grabCoolDown)//if player isn't already grabbed and chances are good and enough time has passed since last grab, then grab
                    currentState = States.Grab;
                else if(distanceFromPlayer <= attackDistance && player.GetCurrentState() == player.GrabbedByZombie)//if player is grabbed by a zombie, attack him
                    currentState = States.Attack;
                else//if neither of those is true, go back to chasing
                    currentState = States.Chase;
            }
            else//if in attack range, turn to attack
                currentState = States.Attack;
          
        }

        //A method that calculates the chance of the next grab happening
        private IEnumerator GrabChanceRandomizer()
        {
            yield return new WaitForSeconds(grabChanceInterval);
            grabChanceRandomiser = Random.Range(0, 100);
        }
        
        void Attack()
        {
            if (Time.time - lastAttackTime <= attackCoolDown) //if attack is still on cooldown, stay idle
            {
                animator.Play(idleHash);
            }
            else
            {
                //make enemy only look at the player during the first half of attack animation, so player can avoid them 
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.65)
                    enemyBehaviour.LookAtPlayer();
                
                animator.Play(attackHash);
            }

            //Once the attack animation ends, mark the time it was done
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
            {
                lastAttackTime = Time.time;
            }
            
            if (distanceFromPlayer > attackDistance) //if no player is found in the attacking distance, swtich to patrol
                currentState = States.Patrol;
        }
        
        void Grab()
        {
            if (player.GetCurrentState() == player.GrabbedByZombie && !grabbedPlayer)//if player is already grabbed by someone other, just attack him
                currentState = States.Attack;
            
            animator.Play(grabHash);//the grab animation has a trigger on it that enables the OnTriggerEnter2D method, to change the player state to GrabbedState
            
            //Increase speed during the grab animation
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime is > 0.3f and < 0.8f)
            {
                enemyBehaviour.ChasePlayer();//make enemy chase the player while doing the animation
                enemyBehaviour.SetMoveSpeed(grabSpeedBoost);
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)//after animation has nearly ended
            {
                enemyBehaviour.SetMoveSpeed(initialMoveSpeed);//return speed to normal
            }
            
            //If the enemy hasn't grabbed the player and the grabWindup animation has ended, mark it as an attack with the lastAttackTime variable and then go to regular attack
             if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !grabbedPlayer)
             {
                 lastGrabTime = Time.time;
                 lastAttackTime = Time.time;
                 currentState = States.Attack;
             }
        }

        private void Death()
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true; //Make it so that before the enemy's body disappears, the player won't be able to hit them anymore
            animator.Play(deathHash);//death animation
            enemyBehaviour.DropItems();//make enemy drop items (if successful)
            Destroy(gameObject, 1.5f);
        }

        public override States GetCurrentState()
        {
            return currentState;
        }

        private IEnumerator GrabDamage()
        {
            yield return new WaitForSeconds(grabDamageInterval);
            
            player.GetComponent<IDamage>().ApplyDamage(grabDamage);
            startedGrabDamaging = false;//can damage again after this coroutine ends
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (currentState == States.Attack)
                    other.GetComponentInParent<IDamage>().ApplyDamage(damage);
                else if (currentState == States.Grab && !grabbedPlayer)//if enemy's state is grab, 
                {
                    grabbedPlayer = true;
                    spriteRenderer.enabled = false;
                    
                    player.ChangeState(player.GrabbedByZombie);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                enemyBehaviour.ChasePlayer();//make enemy chase the player whilst he's invisible, to show up again

                if (player.GetCurrentState() == player.GrabbedByZombie && !startedGrabDamaging) //if player is grabbed currently and grab damage wasn't done already, damage him
                {
                    startedGrabDamaging = true;
                    SoundManager.Instance.EnemySound(gameObject, SoundManager.EnemySounds.EnemyMunching);
                    StartCoroutine(GrabDamage());
                }
            }
        }
    }
}
