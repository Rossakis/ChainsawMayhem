using ChainsawMan.PlayerClass.States;
using ChainsawMan.PlayerClass.States.Airborne.Combat;
using ChainsawMan.PlayerClass.States.Airborne.Movement;
using ChainsawMan.PlayerClass.States.Grounded;
using ChainsawMan.PlayerClass.States.Grounded.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChainsawMan.PlayerClass
{
    /// <summary>
    /// Player class for making and managing the States of the player character
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    
    public class PlayerController : MonoBehaviour
    {
        public CharacterController2D characterController;
        public Animator animator;
        public PlayerHealth PlayerHealth;
        
        //State variables
        private BaseState _currentState;

        //We make all the states here public so that other scripts can access them upon calling the ChangeState() method.
        //
        //E.g. IdleState calls PlayerController(PlayerController.WalkState) to change the _currentState to WalkState
        
        //General States - That can happen both on ground or in the air 
        [HideInInspector]
        public DashingState Dashing;
        [HideInInspector] 
        public DeathState Dead;
        
        //Grounded - Movement States 
        [HideInInspector]
        public IdleState Idle;
        [HideInInspector]
        public WalkState Walking;
        //Grounded - Combat States 
        [HideInInspector]
        public GroundAttackOne GroundAttackOne;
        [HideInInspector]
        public GroundAttackTwo GroundAttackTwo;
        [HideInInspector]
        public GroundAttackThree GroundAttackThree;
        [HideInInspector]
        public WalkingGroundAttackOne WalkingGroundAttackOne;
        [HideInInspector]
        public WalkingGroundAttackTwo WalkingGroundAttackTwo;
        [HideInInspector]
        public KnockUpAttack KnockUpAttack;

        
        //Airborne - Movement States
        [HideInInspector]
        public JumpingState Jumping;
        [HideInInspector]
        public FallingState Falling;
        //Airborne - Combat States
        [HideInInspector]
        public AerialAttackOne AerialAttackOne;
        [HideInInspector]
        public AerialAttackTwo AerialAttackTwo;
        [HideInInspector]
        public AerialAttackThree AerialAttackThree;
        
        //Enemy Interactions
        [HideInInspector]
        public GrabbedByZombie GrabbedByZombie;


        //In order for different States (Grounded, Airborne, etc...) to see if player has jumped, we make it public through the PlayerController class
        public bool hasJumped;

        private void Awake()
        {
            //Movement Components
            Idle = GetComponentInChildren<IdleState>();
            Walking = GetComponentInChildren<WalkState>();
            Jumping = GetComponentInChildren<JumpingState>();
            Falling = GetComponentInChildren<FallingState>();
            Dashing = GetComponentInChildren<DashingState>();
            Dead = GetComponentInChildren<DeathState>();
            
            //Combat Components - grounded
            GroundAttackOne = GetComponentInChildren<GroundAttackOne>();
            GroundAttackTwo = GetComponentInChildren<GroundAttackTwo>();
            GroundAttackThree = GetComponentInChildren<GroundAttackThree>();
            WalkingGroundAttackOne = GetComponentInChildren<WalkingGroundAttackOne>();
            WalkingGroundAttackTwo = GetComponentInChildren<WalkingGroundAttackTwo>();
            KnockUpAttack = GetComponentInChildren<KnockUpAttack>();
            
            //Combat Components - aerial
            AerialAttackOne = GetComponentInChildren<AerialAttackOne>();
            AerialAttackTwo = GetComponentInChildren<AerialAttackTwo>();
            AerialAttackThree = GetComponentInChildren<AerialAttackThree>();
            
            //Enemy Interaction
            GrabbedByZombie = GetComponentInChildren<GrabbedByZombie>();
        }

        private void Start()
        {
            Initialize(Idle);
        }

        private void Initialize(BaseState startingState)
        {
            _currentState = startingState;
            _currentState.EnterState(this);
        }

        public void ChangeState(BaseState newState)
        {
            _currentState.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            _currentState.UpdateLogic(this);
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdate(this);
        }
        
        //public methods
        public BaseState GetCurrentState()
        {
            return _currentState;
        }
    }
}